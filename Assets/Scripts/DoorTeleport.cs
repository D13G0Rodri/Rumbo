using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class DoorTeleport : MonoBehaviour
{
    [Header("Teletransporte")]
    public Transform destination;                      // punto de llegada
    public KeyCode teleportKey = KeyCode.LeftControl;  // tecla para entrar

    [Header("Cinemachine (CM3)")]
    public CinemachineCamera vcam;  // arrastra tu CM vcam1
    public Transform player;         // arrastra el jugador

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
            // Debug.Log($"Presiona {teleportKey} para teletransportar.");
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

        // ---- mover al jugador ----
        Vector3 oldPos = player.position;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.linearVelocity = Vector2.zero; // Unity 6
            rb.angularVelocity = 0f;
        }

        player.position = destination.position;
        Physics2D.SyncTransforms(); // que el confiner/colisiones vean la nueva pos ya

        // Avisar a Cinemachine que el target "saltó"
        // (ayuda a resetear estados internos de seguimiento)
        try { vcam.OnTargetObjectWarped(player, player.position - oldPos); } catch {}

        // Snap de la cámara para evitar que el Confiner la deje pegada al polígono anterior
        StartCoroutine(SnapCinemachineOneFrame());
    }

    IEnumerator SnapCinemachineOneFrame()
    {
        var confiner = vcam.GetComponent<CinemachineConfiner2D>();
        var composer = vcam.GetComponent<CinemachinePositionComposer>();

        // Guardar damping y desactivarlo un frame
        Vector2 oldDamping = Vector2.zero;
        if (composer != null)
        {
            oldDamping = composer.Damping;
            composer.Damping = Vector2.zero;
        }

        // Desactivar Confiner por 1 frame para permitir el "salto"
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

        // Reactivar confiner y reconstruir la forma (método CM3)
        if (confiner)
        {
            confiner.enabled = confWasEnabled;
            confiner.InvalidateBoundingShapeCache();
        }

        // Restaurar damping
        if (composer != null) composer.Damping = oldDamping;
    }
}
