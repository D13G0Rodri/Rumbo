using UnityEngine;
using UnityEngine.SceneManagement;

public class CafeteriaTrigger : MonoBehaviour
{
    [Header("Config")]
    public string choiceSceneName = "ElegirComida";

    [Header("UI")]
    public GameObject pressEPanel; // referencia al texto/panel “Presiona E”

    bool inRange = false;

    void Start()
    {
        if (pressEPanel != null) pressEPanel.SetActive(false);
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(choiceSceneName);
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            if (pressEPanel != null) pressEPanel.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            if (pressEPanel != null) pressEPanel.SetActive(false);
        }
    }
}
