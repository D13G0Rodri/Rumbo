using UnityEngine;
using UnityEngine.SceneManagement;

public class PapaController : MonoBehaviour
{
    [Header("Configuración del Papá")]
    public Transform bebe; // Asigna el transform del bebé en el Inspector
    public float velocidadMovimiento = 3f;
    public float distanciaMinima = 0.5f; // Distancia para detenerse frente al bebé
    public string nombreEscenaVideo = "EscenaVideo"; // Nombre de la escena del video

    private Rigidbody2D rb;
    private Animator animator;
    private bool moviendoHaciaBebe = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (moviendoHaciaBebe)
        {
            MoverHaciaBebe();
        }
    }

    // Método llamado desde el evento OnBebeLlora en CacaManager
    public void IrHaciaBebe()
    {
        moviendoHaciaBebe = true;
        animator.SetBool("isWalking", true); // Activa la animación de caminar
        Debug.Log("¡El papá va hacia el bebé!");
    }

    void MoverHaciaBebe()
    {
        if (bebe == null) return;
        
        // Calcula la dirección hacia el bebé
        Vector2 direccion = (bebe.position - transform.position).normalized;
        rb.linearVelocity = direccion * velocidadMovimiento;

        // Detenerse frente al bebé
        if (Vector2.Distance(transform.position, bebe.position) < distanciaMinima)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false); // Detiene la animación de caminar
            moviendoHaciaBebe = false;

            // Guardar los datos del juego antes de cambiar de escena
            PlayerControllerBase playerController = FindFirstObjectByType<PlayerControllerBase>();
            if (playerController != null)
            {
                playerController.SaveGame();
                Debug.Log("Datos guardados antes de cargar la escena del video.");
            }

            // Cargar la escena del video
            SceneManager.LoadScene(nombreEscenaVideo);
            Debug.Log("Cargando escena del video: " + nombreEscenaVideo);
        }
    }
}
