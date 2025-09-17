using UnityEngine;
using UnityEngine.SceneManagement;

public class FoodChoiceController : MonoBehaviour
{
    // Deltas configurables por si luego quieres ajustarlos desde el Inspector
    [SerializeField] private float bananaIntelligenceDelta = 1f;
    [SerializeField] private float bananaHappinessDelta = 0.5f;

    [SerializeField] private float chipsIntelligenceDelta = -0.5f;
    [SerializeField] private float chipsHappinessDelta = 5f;

    public void ChooseBanana()
    {
        Debug.Log("Banana!");
        ApplyAndSave(bananaIntelligenceDelta, bananaHappinessDelta);
        SceneManager.LoadScene("BuenaDecision");
    }

    public void ChooseChips()
    {
        Debug.Log("Papas!");
        ApplyAndSave(chipsIntelligenceDelta, chipsHappinessDelta);
        SceneManager.LoadScene("MalaDecision");
    }

    // --- Minimal helper interno ---
    private void ApplyAndSave(float dIntelligence, float dHappiness)
{
    var data = SaveSystem.LoadPlayerData() ?? new PlayerData();

    data.intelligence = Mathf.Clamp(data.intelligence + dIntelligence, 0f, 100f);
    data.happiness    = Mathf.Clamp(data.happiness   + dHappiness,    0f, 100f);

    data.currentSceneName = SceneManager.GetActiveScene().name;
    SaveSystem.SavePlayerData(data);

    // ---- NUEVO: buffer para toast ----
    PlayerPrefs.SetFloat("delta_intelligence", dIntelligence);
    PlayerPrefs.SetFloat("delta_karma",        0f);           // si luego ajustas karma, cámbialo aquí
    PlayerPrefs.SetFloat("delta_happiness",    dHappiness);
    PlayerPrefs.SetInt("stat_toast_pending",   1);
    PlayerPrefs.Save();
}

}
