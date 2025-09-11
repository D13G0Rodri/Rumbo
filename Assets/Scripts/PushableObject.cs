using UnityEngine;

/// <summary>
/// Gestiona el comportamiento de objetos que solo pueden ser empujados por el jugador desde una dirección específica.
/// </summary>
public class PushableObject : MonoBehaviour
{
    /// <summary>
    /// Enum para definir la dirección desde la que se puede empujar el objeto.
    /// </summary>
    public enum PushDirection
    {
        RequiresPushingFromLeft, // El jugador debe estar a la izquierda para empujar.
        RequiresPushingFromRight // El jugador debe estar a la derecha para empujar.
    }

    [Header("Configuración de Empuje")]
    [Tooltip("Define desde qué lado debe estar el jugador para poder empujar este objeto.")]
    public PushDirection pushDirection;

    [Tooltip("El tag asignado al objeto del jugador.")]
    public string playerTag = "Player";

    [Tooltip("ID único para este objeto. Usado para guardar y cargar su posición. E.g., \"CocheBebe\", \"PelotaNiño\".")]
    public string objectId;

    [Header("Referencias de Componentes")]
    private Rigidbody2D rb;
    private Collider2D col; // Referencia al Collider2D del objeto
    private Transform playerTransform;

    void Start()
    {
        // Obtener la referencia al componente Rigidbody2D de este objeto.
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("El script PushableObject requiere un componente Rigidbody2D.", this);
            this.enabled = false;
            return;
        }

        // Obtener la referencia al componente Collider2D de este objeto.
        col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("El script PushableObject requiere un componente Collider2D.", this);
            this.enabled = false;
            return;
        }

        // Configurar el estado inicial como bloqueado pero atravesable.
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.isTrigger = true;

        // Encontrar el GameObject del jugador usando su tag.
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogWarning($"No se encontró un jugador con el tag '{playerTag}' en la escena.", this);
        }
    }

    void Update()
    {
        if (playerTransform == null)
        {
            return;
        }

        CheckPlayerPosition();
    }

    /// <summary>
    /// Comprueba la posición del jugador y ajusta la física y el collider en consecuencia.
    /// </summary>
    private void CheckPlayerPosition()
    {
        bool canBePushed;

        // Determinar si el jugador está en la posición correcta para empujar.
        if (pushDirection == PushDirection.RequiresPushingFromLeft)
        {
            canBePushed = playerTransform.position.x < transform.position.x;
        }
        else // Esto cubre el caso RequiresPushingFromRight
        {
            canBePushed = playerTransform.position.x > transform.position.x;
        }

        // Aplicar el estado correspondiente
        if (canBePushed)
        {
            // Estado: Empujable y Sólido
            // El objeto puede ser afectado por la física y tiene un collider sólido para ser empujado.
            rb.bodyType = RigidbodyType2D.Dynamic;
            col.isTrigger = false;
        }
        else
        {
            // Estado: Bloqueado y Atravesable
            // El objeto está fijo en su lugar y su collider es un trigger para que el jugador lo atraviese.
            rb.bodyType = RigidbodyType2D.Kinematic;
            col.isTrigger = true;
        }
    }
}
