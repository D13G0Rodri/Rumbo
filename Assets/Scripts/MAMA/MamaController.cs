using UnityEngine;
using UnityEngine.SceneManagement;

public class MamaController : MonoBehaviour
{
    [Header("Configuración de la Mamá")]
    public Transform bebe; // Asigna el transform del bebé en el Inspector
    public float velocidadMovimiento = 3f;
    public float distanciaMinima = 0.5f; // Distancia para detenerse frente al bebé
    private string nombreEscenaVideo = "MamaAlimentaBebe"; // Nombre de la escena del video

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

    // Método llamado desde el evento OnBebeHambriento en ComidaManager
    public void IrHaciaBebe()
    {
        moviendoHaciaBebe = true;
        animator.SetBool("isWalking", true); // Activa la animación de caminar
        Debug.Log("¡La mamá va hacia el bebé!");
    }

    void MoverHaciaBebe()
    {
        if (bebe == null) return;
        
        // Calcula la dirección hacia el bebé solo en el eje horizontal
        Vector2 direccion = new Vector2(bebe.position.x - transform.position.x, 0).normalized;

        // Girar el sprite para que mire hacia el bebé
        if (direccion.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direccion.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        
        rb.linearVelocity = direccion * velocidadMovimiento;

        // Detenerse frente al bebé
        if (Mathf.Abs(transform.position.x - bebe.position.x) < distanciaMinima)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false); // Detiene la animación de caminar
            moviendoHaciaBebe = false;
            Debug.Log("YA LO TOCÓOOOOOOOOOOOOOOOOOO");
            // Guardar los datos del juego antes de cambiar de escena
            PlayerControllerBase playerController = FindFirstObjectByType<PlayerControllerBase>();
            if (playerController != null)
            {
                playerController.hunger = 100f; // Restablecer la comida
                playerController.SaveCheckpoint();
                Debug.Log("Datos guardados antes de cargar la escena del video.");
            }

            // Cargar la escena del video
            SceneManager.LoadScene(nombreEscenaVideo);
            Debug.Log("Cargando escena del video: " + nombreEscenaVideo);
        }
    }
}
