using UnityEngine;
using UnityEngine.SceneManagement;

// Coloca este script en un GameObject en la(s) escena(s) de entrada (por ejemplo, la escena del bebé)
// para redirigir automáticamente a la última escena guardada si no coincide.
public class AutoContinueOnStart : MonoBehaviour
{
    [Tooltip("Si es true, forzará continuar a la escena guardada cuando no coincida.")]
    public bool autoContinue = true;

    private void Start()
    {
        var data = SaveSystem.LoadPlayerData();
        if (!autoContinue || data == null || string.IsNullOrEmpty(data.currentSceneName))
            return;

        string escenaActual = SceneManager.GetActiveScene().name;
        // Si la escena guardada es distinta y existe el archivo, redirigimos.
        if (data.currentSceneName != escenaActual)
        {
            Debug.Log($"AutoContinue: redirigiendo de '{escenaActual}' a '{data.currentSceneName}' según guardado.");
            SceneManager.LoadScene(data.currentSceneName);
        }
    }
}

