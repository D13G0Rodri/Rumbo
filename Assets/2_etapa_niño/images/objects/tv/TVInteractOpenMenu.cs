using UnityEngine;
using UnityEngine.SceneManagement;

public class TVInteractOpenMenu : MonoBehaviour
{
    public Transform jugador;                  // Aquí arrastrarás tu Player
    public string menuSceneName = "TVMenu";    // Nombre de la escena nueva

    [Header("Configuración de Interacción")]
    [SerializeField] private KeyCode interactKey = KeyCode.E; 
    // <-- Puedes elegir cualquier tecla desde el Inspector

    private bool enZona = false;

    void Update()
    {
        // Solo si el jugador está dentro de la zona invisible
        if (enZona)
        {
            if (Input.GetKeyDown(interactKey))
            {
                // Guardar un checkpoint usando el sistema de guardado principal
                PlayerControllerBase playerController = jugador.GetComponent<PlayerControllerBase>();
                if (playerController != null)
                {
                    playerController.SaveCheckpoint();
                }

                // Cargar la nueva escena
                SceneManager.LoadScene(menuSceneName);
            }
        }
    }

    // Cuando el jugador entra al área
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) enZona = true;
    }

    // Cuando el jugador sale del área
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) enZona = false;
    }
}
