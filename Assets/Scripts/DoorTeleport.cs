using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class DoorTeleport : MonoBehaviour
{
    [Header("Teletransporte")]
    public Transform destination;                   // punto de llegada
    public KeyCode teleportKey = KeyCode.E;         // tecla para entrar

    [Header("Cinemachine (CM3)")]
    public CinemachineCamera vcam;                  // arrastra tu CM vcam1
    public Transform player;                        // arrastra el jugador

    [Header("Transición")]
    public float transitionSeconds = 2f;            // duración total de la transición
    public AudioClip transitionClip;                // sonido opcional (whoosh/cadenas)

    bool playerNearby = false;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(teleportKey))
            TeleportPlayer();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player)
        {
            playerNearby = true;
            // Aquí podrías mostrar un "Presiona E" si quieres
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == player)
            playerNearby = false;
    }

    void TeleportPlayer()
    {
        if (destination == null || player == null || vcam == null)
        {
            Debug.LogError("DoorTeleport: faltan referencias (destination / player / vcam).");
            return;
        }

        // Secuencia EXACTA pedida:
        // Fade-out sin pausar → TP + cámara en negro → pausar → mini espera → fade-in → reanudar
        TransitionManager.Instance.FadeCustom(
            transitionSeconds,
            transitionClip,
            () => { PerformTeleportImmediately(); } // se ejecuta en negro total
        );
    }

    // === Tu lógica original de TP y ajuste de cámara, sin cambios de intención ===
    void PerformTeleportImmediately()
    {
        Vector3 oldPos = player.position;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb)
        {
            // Unity 6 (Physics 2D):
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // Mover al jugador
        player.position = destination.position;
        Physics2D.SyncTransforms(); // que confiner/colisiones vean la nueva pos

        // Avisar a Cinemachine del "warp"
        try
        {
            Vector3 delta = player.position - oldPos;
            vcam.OnTargetObjectWarped(player, delta);
        }
        catch {}

        // Snap de un frame para asegurar la cámara
        StartCoroutine(SnapCinemachineOneFrame());
    }

    IEnumerator SnapCinemachineOneFrame()
    {
        var confiner = vcam.GetComponent<CinemachineConfiner2D>();
        var composer = vcam.GetComponent<CinemachinePositionComposer>();

        // Guardar damping y desactivarlo 1 frame
        Vector2 oldDamping = Vector2.zero;
        if (composer != null)
        {
            oldDamping = composer.Damping;
            composer.Damping = Vector2.zero;
        }

        // Desactivar confiner por 1 frame para permitir el “salto”
        bool confWasEnabled = confiner && confiner.enabled;
        if (confiner) confiner.enabled = false;

        // Romper blends y colocar la VCam sobre el player
        var oldFollow = vcam.Follow;
        vcam.Follow = null;
        vcam.transform.position = new Vector3(
            player.position.x,
            player.position.y,
            vcam.transform.position.z
        );

        // Esperar 1 frame
        yield return null;

        // Volver a seguir al jugador
        vcam.Follow = player;

        // Reactivar confiner y reconstruir cache
        if (confiner)
        {
            confiner.enabled = confWasEnabled;
            confiner.InvalidateBoundingShapeCache();
        }

        // Restaurar damping
        if (composer != null) composer.Damping = oldDamping;
    }
}
