using UnityEngine;

/// <summary>
/// Detecta cuando un objeto empujable entra en la zona, lo desactiva y guarda un checkpoint.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ObjectGoalZone : MonoBehaviour
{
    private void Awake()
    {
        // Asegurarse de que el collider de esta zona sea un trigger.
        Collider2D col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            Debug.LogWarning("El collider de ObjectGoalZone no era un trigger. Se ha configurado automáticamente.", this);
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobar si el objeto que ha entrado tiene el script PushableObject.
        PushableObject pushable = other.GetComponent<PushableObject>();

        if (pushable != null && pushable.enabled)
        {
            Debug.Log($"¡El objeto '{pushable.objectId}' ha llegado a la zona de destino!");

            // 1. Desactivar el script del objeto para que no se pueda volver a mover.
            pushable.enabled = false;

            // Opcional: Forzar su estado a Kinematic y Trigger por si acaso.
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null) 
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            other.isTrigger = true;

            // 2. Encontrar el controlador del jugador.
            PlayerControllerBase player = FindFirstObjectByType<PlayerControllerBase>();
            if (player != null)
            {
                // 3. Guardar un checkpoint, pasando 'true' para indicar que se ha conseguido el logro de ordenar.
                player.SaveCheckpoint(true);
                Debug.Log("Checkpoint guardado con el logro de ordenar.");
            }
            else
            {
                Debug.LogError("No se encontró PlayerControllerBase en la escena para guardar el checkpoint.");
            }
        }
    }
}
