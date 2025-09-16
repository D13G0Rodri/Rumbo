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
                // Guardar la posición actual del jugador
                if (jugador != null)
                {
                    ReturnPoint.playerPos = jugador.position;
                    ReturnPoint.sceneName = SceneManager.GetActiveScene().name;
                    ReturnPoint.hasData = true;
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
