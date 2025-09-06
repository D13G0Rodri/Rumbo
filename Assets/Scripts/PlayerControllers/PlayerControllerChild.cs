using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerChild : PlayerControllerBase
{
    [Header("Configuración Niño")]
    public float energy = 100f;
    public float learnRate = 0.5f;
    
    protected override void Start()
    {
        base.Start();

        var spawn = GameObject.FindWithTag("ChildSpawn");
        if (spawn != null)
        {
            transform.position = spawn.transform.position;
            Debug.Log($"Spawn Niño aplicado en: {transform.position}");
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con tag 'ChildSpawn' en la escena del niño.");
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
        base.ReceiveDamage(damage);
        if (!isReceivingDamage)
        {
             energy -= damage * 2f;
        }
    }

    // --- SISTEMA DE GUARDADO Y CARGA (NUEVO MÉTODO) ---

    /// <summary>
    /// Sobrescribimos este método para añadir los datos específicos del niño (energía).
    /// La clase base (PlayerControllerBase) se encarga de llamar a este método 
    /// tanto al guardar como al cargar.
    /// </summary>
    protected override void AddStageSpecificData(PlayerData data)
    {
        // Al guardar, esto añade la energía al paquete de datos.
        data.energy = this.energy;

        // Al cargar, esto recupera la energía del paquete de datos.
        this.energy = data.energy;
    }

    /// <summary>
    /// Sobrescribimos este método SOLO para definir la escena de la siguiente etapa.
    /// La lógica de añadir los datos ya está centralizada en AddStageSpecificData.
    /// </summary>
    protected override PlayerData CreatePlayerData()
    {
        // 1. Llama a la base para que cree el paquete de datos y añada la energía (vía AddStageSpecificData).
        PlayerData data = base.CreatePlayerData();

        // 2. Define cuál es la siguiente escena para la transición.
        data.currentSceneName = "Etapa-Adolescente";

        return data;
    }

    // No es necesario sobrescribir LoadGame nunca más, la base ya lo gestiona todo.
    // Tampoco es necesario tener un SaveCheckpoint aquí, ya está en la base.
}
