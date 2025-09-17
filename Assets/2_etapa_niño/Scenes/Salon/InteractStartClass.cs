using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractStartClass : MonoBehaviour
{
    [Header("Detección")]
    public string playerTag = "Player";

    [Header("UI")]
    public GameObject hintUI;        // arrastra aquí el HintText
    public string bookSceneName = "LibroScene";

    bool inside = false;

    void Start()
    {
        if (hintUI) hintUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            inside = true;
            if (hintUI) hintUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            inside = false;
            if (hintUI) hintUI.SetActive(false);
        }
    }

    void Update()
    {
        if (inside && Input.GetKeyDown(KeyCode.E))
        {
            if (hintUI) hintUI.SetActive(false);
            SceneManager.LoadScene(bookSceneName);
        }
    }
}
