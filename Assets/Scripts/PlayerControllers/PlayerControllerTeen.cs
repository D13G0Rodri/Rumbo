using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerTeen : PlayerControllerBase
{
    [Header("Configuración Adolescente")]
    public float happiness = 100f;
    public float socialImpact = 5f;

    protected override void Start()
    {
        base.Start(); // Carga los datos comunes
        PlayerData data = SaveSystem.LoadPlayerData();
        if (data != null)
        {
            happiness = data.happiness; // Carga la felicidad del adolescente
        }
    }

    protected override void Update()
    {
        base.Update();
        Socialize();
    }

    private void Socialize()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            happiness += socialImpact;
            concentration += socialImpact * 0.5f;
            Debug.Log($"¡Socializaste! Felicidad: {happiness}, Concentración: {concentration}");
        }
    }

    protected override void Dead()
    {
        animator.SetBool("isDead", true);
        Debug.Log("¡El adolescente ha muerto por estrés!");
        SceneManager.LoadScene("Muerte");
    }
    public override void SaveGame()
    {
        PlayerData data = new PlayerData
        {
            health = health,
            position = new float[] { transform.position.x, transform.position.y, transform.position.z },
            intelligence = intelligence,
            concentration = concentration,
            hunger = hunger,
            bathroom = bathroom,
            happiness = happiness, // Guarda la felicidad del adolescente
            currentSceneName = SceneManager.GetActiveScene().name
        };
        SaveSystem.SavePlayerData(data);
    }

    public override void LoadGame()
    {
        base.LoadGame(); // Carga los datos comunes
        PlayerData data = SaveSystem.LoadPlayerData();
        if (data != null)
        {
            happiness = data.happiness; // Carga la felicidad del adolescente
        }
    }
}
