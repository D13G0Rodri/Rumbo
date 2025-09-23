using UnityEngine;
using UnityEngine.SceneManagement;

public class BedInteraction : MonoBehaviour
{
    [SerializeField] private string videoSceneName = "VideoScene"; // Escena del video
    private bool playerIsNear = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerIsNear = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerIsNear = false;
    }

    private void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(videoSceneName);
        }
    }
}
