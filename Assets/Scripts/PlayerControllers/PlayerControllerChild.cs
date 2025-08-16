using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerControllerChild : PlayerControllerBase
{
    [Header("Configuración Niño")]
    public float energy = 100f;
    public float learnRate = 0.5f;


    protected override void Start()
    {
        base.Start(); // Carga los datos comunes
        PlayerData data = SaveSystem.LoadPlayerData();
        if (data != null)
        {
            energy = data.energy; // Carga la energía del niño
        }
    }
    
    protected override void Update()
    {
        base.Update();
        Learn();
    }

    private void Learn()
    {
        if (Input.GetKeyDown(KeyCode.E) && energy > 0)
        {
            intelligence += learnRate;
            energy -= 10f;
            Debug.Log($"¡Aprendiste! Inteligencia: {intelligence}, Energía: {energy}");
        }
    }

    public override void ReceiveDamage(float damage)
    {
        if (!isReceivingDamage && barraVidaImage != null)
        {
            isReceivingDamage = true;
            health -= damage;
            energy -= damage * 2f;
            barraVidaImage.fillAmount = health / VidaMax;
            if (health <= 0)
                Dead();
            Invoke(nameof(ResetDamage), damageCooldown);
        }
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
            energy = energy, // Guarda la energía del niño
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
            energy = data.energy; // Carga la energía del niño
        }
    }



}
