using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnAfterDelay : MonoBehaviour
{
    public string returnSceneName = "Etapa-Niño"; // pon aquí el nombre de tu escena de la cafetería
    public float delaySeconds = 5f;

    void Start()
    {
        Invoke(nameof(ReturnNow), delaySeconds);
    }

    void ReturnNow()
    {
        SceneManager.LoadScene(returnSceneName);
    }
}
