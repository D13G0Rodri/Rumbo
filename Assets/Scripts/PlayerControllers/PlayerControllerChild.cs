using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerChild : PlayerControllerBase
{
    [Header("Configuración Niño")]
    public float energy = 100f; // Valor inicial por si no hay datos guardados.
    public float learnRate = 0.5f;
    
    protected override void Start()
    {
        // Primero cargamos datos con la base para tener stats actualizados.
        base.Start();

        // Luego forzamos la aparición en un punto de spawn propio de la escena del niño.
        // Busca un GameObject con tag "ChildSpawn" y usa su posición.
        var spawn = GameObject.FindWithTag("ChildSpawn");
        if (spawn != null)
        {
            transform.position = spawn.transform.position;
            // Actualiza también la posición en memoria para futuros guardados.
            // No guardamos inmediatamente para no sobreescribir si estás probando.
            Debug.Log($"Spawn Niño aplicado en: {transform.position}");
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con tag 'ChildSpawn' en la escena del niño. El personaje puede caer al vacío si la posición guardada no es válida.");
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

    // Sobrescribimos el método para que al recibir daño, también baje la energía.
    public override void ReceiveDamage(float damage)
    {
        // Llamamos a la lógica base para que se encargue de la vida, cooldown, etc.
        base.ReceiveDamage(damage);

        // Añadimos nuestra lógica específica de esta clase.
        if (!isReceivingDamage) // Solo aplicamos esto si el daño fue procesado (para respetar el cooldown)
        {
             energy -= damage * 2f;
        }
    }

    // CAMBIO: Sobrescribimos CreatePlayerData para añadir la energía.
    protected override PlayerData CreatePlayerData()
    {
        // 1. Obtenemos el objeto de datos con toda la información base.
        PlayerData data = base.CreatePlayerData();

        // 2. Añadimos la información específica del niño.
        data.energy = this.energy;
        data.currentSceneName = "Etapa-Adolescente"; // Escena a la que irá después

        // 3. Devolvemos el objeto completo.
        return data;
    }

    // CAMBIO: Sobrescribimos LoadGame para cargar la energía.
     // CAMBIO: Ahora LoadGame recibe el PlayerData del base.LoadGame()
    public override PlayerData LoadGame()
    {
        // 1. Llamamos a la implementación base para cargar los datos comunes.
        //    'data' es el objeto que contiene TODOS los datos del archivo.
        PlayerData data = base.LoadGame(); 
        
        // 2. Si hay datos, aplicamos los específicos de esta clase.
        if (data != null)
        {
            energy = data.energy; // Cargamos la energía del niño
        }

        return data; // Retornamos el objeto data por si alguna clase hija de Child lo necesitara.
    }
}