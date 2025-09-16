using UnityEngine;
using UnityEngine.SceneManagement;

public class TVInteractOpenMenu : MonoBehaviour
{
    public Transform jugador;          // Aquí arrastrarás tu Player
    public string menuSceneName = "TVMenu"; // Nombre de la escena nueva
    bool enZona = false;

    void Update()
    {
        // Solo si el jugador está dentro de la zona invisible
        if (enZona)
        {
            // Si presiona la tecla Control
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
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
