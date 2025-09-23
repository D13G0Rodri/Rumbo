using UnityEngine;
using UnityEngine.SceneManagement;

// Adjunta este script a un GameObject persistente o a un Canvas de UI en tus escenas de juego.
// Expone métodos públicos para que los enlaces a botones: Pausar, Reanudar, AlternarPausa, ReiniciarDesdeCero.
// ReiniciarDesdeCero limpia PlayerPrefs, borra el archivo de guardado y recarga la primera escena jugable (o la escena actual si prefieres).
public class PauseMenu : MonoBehaviour
{
    [Header("UI Opcional")]
    [Tooltip("Panel que se muestra cuando el juego está en pausa")] public GameObject pausePanel;

    [Header("Configuración de escena inicial")]
    [Tooltip("Nombre de la escena a cargar cuando se reinicie desde cero. Si está vacío, usará la buildIndex 0.")]
    public string initialSceneName = ""; // Puedes definir aquí, o dejar vacío para usar la primera escena del build.

    private bool isPaused;

    void Awake()
    {
        // Asegúrate de no heredar un timeScale pausado si entras a una escena desde otra
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);
        isPaused = false;
    }

    // Llama desde un botón o una tecla
    public void TogglePause()
    {
        if (isPaused) Resume(); else Pause();
    }

    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0f;
        if (pausePanel != null) pausePanel.SetActive(true);


    }

    public void Resume()
    {
        if (!isPaused) return;
        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);

    }

    // Reinicia todo como si el juego empezara de nuevo
    public void ReiniciarDesdeCero()
    {
        // Asegura que el tiempo siga corriendo tras el reinicio
        Time.timeScale = 1f;

        // Limpia preferencias simples
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Borra archivo(s) de guardado si se usa SaveSystem
        try
        {
            SaveSystem.DeleteSave();
        }
        catch { /* Si no existe, continuar sin errores */ }

        // Opcional: si tienes sistemas estáticos/singletons, restablécelos aquí
        // e.g., AudioManager.Reset(), GlobalState.Clear(), etc.

        // Cargar escena inicial
        if (!string.IsNullOrEmpty(initialSceneName))
        {
            SceneManager.LoadScene(initialSceneName);
        }
        else
        {
            // Usa la primera escena del Build Settings
            var idx = 0;
            SceneManager.LoadScene(idx);
        }
    }
    public void Salir()
    {
        // Registrar que ya entró a BebeGatea
        PlayerPrefs.SetInt("BebeGatea_FirstTime", 1);
        PlayerPrefs.Save();

        // Crear PlayerData correctamente
        PlayerData data = new PlayerData();
        data.currentSceneName = SceneManager.GetActiveScene().name;
        data.health = 100;       // valor por defecto
        data.timerCount = 0f;    // valor por defecto
        data.position = new float[3] { 0f, 0f, 0f }; // importante: no null

        // Guardar usando SaveSystem
        SaveSystem.SavePlayerData(data);

        // Cargar menú principal
        SceneManager.LoadScene("MenuPrincipal");
    }


}

