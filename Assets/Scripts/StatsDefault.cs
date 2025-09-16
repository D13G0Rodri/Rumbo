using UnityEngine;

public class StatsDefaultsOnFirstVisit : MonoBehaviour
{
    [Header("Valores iniciales (0–100)")]
    public float defaultIntelligence = 20f;
    public float defaultKarma        = 50f;
    public float defaultHappiness    = 50f;

    // Flag local para recordar que ya inicializamos
    private const string InitFlagKey = "child_stats_initialized_v1";

    void Awake()
    {
        // 1) Si ya marcamos que se inicializó, no hacemos nada
        if (PlayerPrefs.GetInt(InitFlagKey, 0) == 1)
            return;

        // 2) Si NO hay guardado, lo creamos con defaults
        if (!SaveSystem.HasSave())
        {
            var d = new PlayerData();
            d.health = 100f;
            d.position = new float[] { 0f, 0f, 0f };
            d.currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            d.intelligence = Mathf.Clamp(defaultIntelligence, 0, 100);
            d.karma        = Mathf.Clamp(defaultKarma,        0, 100);
            d.happiness    = Mathf.Clamp(defaultHappiness,    0, 100);

            SaveSystem.SavePlayerData(d);
            MarkInitialized();
            return;
        }

        // 3) Si HAY guardado, solo inicializamos si esas stats están "vacías" o fuera de rango.
        //    Esto evita sobreescribir progreso real.
        var data = SaveSystem.LoadPlayerData();
        bool needsInit =
            data == null ||
            !IsValid01to100(data.intelligence) ||
            !IsValid01to100(data.karma) ||
            !IsValid01to100(data.happiness);

        if (needsInit)
        {
            if (data == null) data = new PlayerData();
            data.intelligence = Mathf.Clamp(defaultIntelligence, 0, 100);
            data.karma        = Mathf.Clamp(defaultKarma,        0, 100);
            data.happiness    = Mathf.Clamp(defaultHappiness,    0, 100);
            if (data.position == null || data.position.Length < 3)
                data.position = new float[] { 0f, 0f, 0f };
            if (string.IsNullOrEmpty(data.currentSceneName))
                data.currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            SaveSystem.SavePlayerData(data);
        }

        MarkInitialized();
    }

    private bool IsValid01to100(float v) => v >= 0f && v <= 100f;

    private void MarkInitialized()
    {
        PlayerPrefs.SetInt(InitFlagKey, 1);
        PlayerPrefs.Save();
        Debug.Log("[StatsDefaultsOnFirstVisit] Inicialización hecha una sola vez.");
    }
}
