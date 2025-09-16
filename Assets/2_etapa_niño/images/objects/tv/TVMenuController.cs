using UnityEngine;
using UnityEngine.SceneManagement;

public class TVMenuController : MonoBehaviour
{
    // Escenas (igual que antes)
    public string escenaBuena = "TV_Buena"; // pantalla “buena”
    public string escenaMala  = "TV_Mala";  // pantalla “mala”

    // Deltas configurables (por si luego los ajustas desde el Inspector)
    [Header("Deltas de TV")]
    [SerializeField] private float documentalIntelligenceDelta = 1f;   // inteligencia +1
    [SerializeField] private float documentalHappinessDelta   = -5f;  // felicidad -5

    [SerializeField] private float jugarIntelligenceDelta     = -1f;  // inteligencia -1
    [SerializeField] private float jugarHappinessDelta        = 15f;  // felicidad +15

    public void ElegirDocumental() // botón VERDE
    {
        ApplyAndSave(documentalIntelligenceDelta, documentalHappinessDelta);
        SceneManager.LoadScene(escenaBuena);
    }

    public void ElegirJugar() // botón ROJO
    {
        ApplyAndSave(jugarIntelligenceDelta, jugarHappinessDelta);
        SceneManager.LoadScene(escenaMala);
    }

    // --- Helper interno idéntico al patrón usado en FoodChoiceController ---
    private void ApplyAndSave(float dIntelligence, float dHappiness)
    {
        var data = SaveSystem.LoadPlayerData() ?? new PlayerData();

        data.intelligence = Mathf.Clamp(data.intelligence + dIntelligence, 0f, 100f);
        data.happiness    = Mathf.Clamp(data.happiness    + dHappiness,    0f, 100f);

        data.currentSceneName = SceneManager.GetActiveScene().name;
        SaveSystem.SavePlayerData(data);

        // Buffer para el StatChangeToast (idéntico al otro controlador)
        PlayerPrefs.SetFloat("delta_intelligence", dIntelligence);
        PlayerPrefs.SetFloat("delta_karma",        0f);          // si luego decides afectar karma, ajústalo aquí
        PlayerPrefs.SetFloat("delta_happiness",    dHappiness);
        PlayerPrefs.SetInt("stat_toast_pending",   1);
        PlayerPrefs.Save();
    }

    // Opcional: tecla ESC para volver sin elegir (sin cambios)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && ReturnPoint.hasData)
            SceneManager.LoadScene(ReturnPoint.sceneName);
    }
}
