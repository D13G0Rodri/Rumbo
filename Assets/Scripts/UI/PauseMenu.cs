using UnityEngine;
using UnityEngine.SceneManagement;

// Adjunta este script a un GameObject persistente o a un Canvas de UI en tus escenas de juego.
// Expone métodos públicos para que los enlaces a botones: Pausar, Reanudar, AlternarPausa, ReiniciarDesdeCero.
// ReiniciarDesdeCero limpia PlayerPrefs, borra el archivo de guardado y recarga la primera escena jugable (o la escena actual si prefieres).
public class PauseMenu : MonoBehaviour
{
    [Header("UI Opcional")]
    [Tooltip("Panel que se muestra cuando el juego está en pausa")]
    public GameObject pausePanel;

    [Header("Configuración de escena inicial")]
    [Tooltip("Nombre de la escena a cargar cuando se reinicie desde cero. Si está vacío, usará la buildIndex 0.")]
    public string initialSceneName = "";

    private bool isPaused;

    void Awake()
    {
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
        Time.timeScale = 1f;

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        try
        {
            SaveSystem.DeleteSave();
        }
        catch { }

        if (!string.IsNullOrEmpty(initialSceneName))
        {
            SceneManager.LoadScene(initialSceneName);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void Salir()
    {
        PlayerPrefs.SetInt("BebeGatea_FirstTime", 1);
        PlayerPrefs.Save();

        // Buscar cualquier jugador que herede de PlayerControllerBase
        PlayerControllerBase player = FindFirstObjectByType<PlayerControllerBase>();
        if (player == null)
        {
            Debug.LogWarning("No se encontró ningún PlayerControllerBase en la escena. Se creará un guardado vacío.");
        }

        // Cargar datos existentes o crear nuevos
        PlayerData data = SaveSystem.LoadPlayerData() ?? new PlayerData();

        if (player != null)
        {
            // --- DATOS DEL JUGADOR ---
            data.health = player.health;
            data.position = new float[] { player.transform.position.x, player.transform.position.y, player.transform.position.z };
            data.intelligence = player.intelligence;
            data.concentration = player.concentration;
            data.hunger = player.hunger;
            data.bathroom = player.bathroom;
            TimerVida timer = FindFirstObjectByType<TimerVida>();
            data.timerCount = (timer != null) ? timer.timerCount : player.timerCount;

            data.presentationPanelShown = player.presentationPanelShown;

            // Llamar a AddStageSpecificData para guardar cualquier dato específico de la etapa
            player.ApplyStageSpecificData(data);
        }

        // Guardar la escena actual
        data.currentSceneName = SceneManager.GetActiveScene().name;

        // --- POSICIONES DE OBJETOS EMPUJABLES ---
        var pushables = FindObjectsByType<PushableObject>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );
        foreach (var obj in pushables)
        {
            if (obj == null || string.IsNullOrEmpty(obj.objectId))
                continue;

            SerializableVector3 pos = new SerializableVector3(obj.transform.position);
            if (data.pushableObjectPositions.ContainsKey(obj.objectId))
                data.pushableObjectPositions[obj.objectId] = pos;
            else
                data.pushableObjectPositions.Add(obj.objectId, pos);
        }

        // Guardar datos completos
        SaveSystem.SavePlayerData(data);

        // Cambiar a menú principal
        SceneManager.LoadScene("MenuPrincipal");
    }
}
