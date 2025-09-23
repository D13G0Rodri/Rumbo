using UnityEngine;
using UnityEngine.SceneManagement;

public class TVInteractOpenMenu : MonoBehaviour
{
    public Transform jugador;                  // Aquí arrastrarás tu Player
    public string menuSceneName = "TVMenu";    // Nombre de la escena nueva

    [Header("Configuración de Interacción")]
    [SerializeField] private KeyCode interactKey = KeyCode.E; 

    [Header("Panel de Mensaje")]
    [SerializeField] private GameObject mensajePanel; // Arrastra aquí tu panel de mensaje

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

        // Asegúrate de que el panel esté oculto al inicio
        if (mensajePanel != null)
            mensajePanel.SetActive(false);
    }

    void Update()
    {
        if (enZona && Input.GetKeyDown(interactKey) && jugador != null)
        {
            PlayerControllerBase playerController = jugador.GetComponent<PlayerControllerBase>();
            if (playerController != null)
            {
                playerController.SaveCheckpoint();
            }

            SceneManager.LoadScene(menuSceneName);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enZona = true;

            // Mostrar el panel cuando el jugador entre
            if (mensajePanel != null)
                mensajePanel.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enZona = false;

            // Ocultar el panel cuando el jugador salga
            if (mensajePanel != null)
                mensajePanel.SetActive(false);
        }
    }
}
