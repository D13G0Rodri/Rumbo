using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InteractLoadScene : MonoBehaviour
{
    [Header("Player detection")]
    [SerializeField] private string playerTag = "Player";

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI pressEText;

    [Header("Scene to load")]
    [SerializeField] private string choiceSceneName = "EleccionLibros";

    bool playerInside;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInside = true;
            if (pressEText) pressEText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInside = false;
            if (pressEText) pressEText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            // Oculta el mensaje y abre escena de elección
            if (pressEText) pressEText.gameObject.SetActive(false);
            SceneManager.LoadScene(choiceSceneName, LoadSceneMode.Single);
        }
    }
}
