using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAutoChanger : MonoBehaviour
{
    [Header("Tiempo antes de cambiar de escena (segundos)")]
    [Tooltip("Tiempo en segundos antes de cambiar de escena automáticamente")]
    public float tiempoDeEspera = 15f;

    private void Start()
    {
        Invoke("CambiarEscena", tiempoDeEspera);
    }

    public void CambiarEscena(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
