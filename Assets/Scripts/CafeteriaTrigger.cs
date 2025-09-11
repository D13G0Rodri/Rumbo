using UnityEngine;
using UnityEngine.SceneManagement;

public class CafeteriaTrigger : MonoBehaviour
{
    public GameObject pressEText; // el mensaje "Presiona E"
    public string sceneToLoad = "ElegirComida"; // la pantalla siguiente

    bool playerInside = false;

    void Start()
    {
        if (pressEText != null) pressEText.SetActive(false);
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            if (pressEText != null) pressEText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (pressEText != null) pressEText.SetActive(false);
        }
    }
}
