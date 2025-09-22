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

    void Start()
    {
        // Si el jugador no ha sido asignado en el Inspector, búscalo por su tag.
        if (jugador == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                jugador = playerObject.transform;
            }
            else
            {
                Debug.LogError("No se pudo encontrar al jugador. Asegúrate de que el objeto del jugador tenga el tag 'Player'.");
            }
        }
    }

    void Update()
    {
        // Solo si el jugador está dentro de la zona invisible
        if (enZona)
        {
            if (Input.GetKeyDown(interactKey) && jugador != null)
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
