using UnityEngine;
using UnityEngine.SceneManagement;

public class BedInteraction : MonoBehaviour
{
    [SerializeField] private string videoSceneName = "VideoScene"; // Escena del video
    [SerializeField] private GameObject mensajePanel; // Panel del mensaje
    [SerializeField] private AudioSource mensajeAudio; // Sonido al aparecer
    private bool playerIsNear = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
            mensajePanel.SetActive(true); // Muestra el mensaje
            mensajeAudio.Play(); // Reproduce el sonido
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            mensajePanel.SetActive(false); // Oculta el mensaje
        }
    }

    private void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(videoSceneName);
        }
    }
}
