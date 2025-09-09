// SnapCinemachineAfterTeleport_CM3.cs
// Unity 6 + Cinemachine 3.x

using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class SnapCinemachineAfterTeleport_CM3 : MonoBehaviour
{
    [Header("Referencias")]
    public CinemachineCamera vcam;   // Arrastra tu CM vcam1
    public Transform player;         // Arrastra tu jugador (niño)

    /// Llama a esto justo DESPUÉS de mover al jugador (teleport).
    public void SnapNow()
    {
        if (isActiveAndEnabled)
            StartCoroutine(SnapRoutine());
    }

    private IEnumerator SnapRoutine()
    {
        if (vcam == null || player == null) yield break;

        // Asegura que físicas y posición estén al día este frame
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.linearVelocity = Vector2.zero; // nuevo en Unity 6
            rb.angularVelocity = 0f;          // opcional: por si traía giro
        }

        Physics2D.SyncTransforms();

        // 1) Deshabilita confiner 1 frame para que no bloquee el salto
        var confiner = vcam.GetComponent<CinemachineConfiner2D>();
        bool confWasEnabled = confiner && confiner.enabled;
        if (confiner) confiner.enabled = false;

        // 2) Rompe cualquier blend y "teleporta" la VCam sobre el jugador
        var oldFollow = vcam.Follow;
        vcam.Follow = null; // corta blends/estado previo
        vcam.transform.position = new Vector3(
            player.position.x,
            player.position.y,
            vcam.transform.position.z
        );

        // 3) Espera 1 frame y vuelve a seguir al jugador
        yield return null;
        vcam.Follow = player;

        // 4) Reactiva confiner y rehace el cache de la forma (método nuevo en CM3)
        if (confiner)
        {
            confiner.enabled = confWasEnabled;
            confiner.InvalidateBoundingShapeCache(); // <- reemplaza a InvalidateCache()
        }
    }
}