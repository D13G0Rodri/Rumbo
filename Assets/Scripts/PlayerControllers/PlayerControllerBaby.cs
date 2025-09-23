using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerBaby : PlayerControllerBase
{
    // No necesitamos re-declarar danioPorCiclo aquí si el daño lo gestiona el evento.

    [Header("Spawn Point")]
    public Transform spawnPoint; // Arrastra aquí el BabySpawnPoint desde el Inspector

    protected override void Start()
    {
        base.Start();

        // Esto asegura que la física esté activa al cargar la escena
        Time.timeScale = 1f; // Desbloquea el tiempo si venías de pausa

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Activa la física del bebé
        }

        // --- NUEVO: poner al bebé en el spawn ---
        if (spawnPoint != null)
            transform.position = spawnPoint.position;
    }

    protected override void Update()
    {
        base.Update(); // Reutiliza movimiento y salto
        Cry();
    }

    private void Cry()
    {
        if (Input.GetKeyDown(KeyCode.L))
            animator.SetBool("isCrying", true);
        if (Input.GetKeyUp(KeyCode.L))
            animator.SetBool("isCrying", false);
    }

    // CAMBIO: Esta función ahora recibe el daño como parámetro.
    // El evento es quien decide cuánto daño hacer.
    public void DañoPorNoLimpiar(float damageAmount)
    {
        ReceiveDamage(damageAmount); // Reutilizamos la función de daño base.
        Debug.Log($"¡Daño por falta de limpieza! Vida: {health}");
    }

    public void ModificateState()
    {
        animator.SetBool("isElectrocuted", false);
    }

    // CAMBIO: Sobrescribimos el método para crear datos, en lugar de todo el SaveGame.
    protected override PlayerData CreatePlayerData()
    {
        // Obtenemos los datos base (vida, posición, etc.)
        PlayerData data = base.CreatePlayerData();

        // Modificamos solo lo que es específico del bebé al pasar de etapa
        data.currentSceneName = "Etapa-Niño"; // Próxima escena

        return data;
    }

    // CAMBIO: Ahora LoadGame recibe el PlayerData del base.LoadGame()
    public override PlayerData LoadGame()
    {
        // Llamamos a la implementación base para cargar los datos comunes
        PlayerData data = base.LoadGame(); 
        
        // No necesitamos cargar nada específico aquí, ya que el bebé no tiene campos únicos en el guardado.
        // Pero mantenemos el override por si acaso en el futuro se añaden.
        
        return data; // Devolvemos el mismo objeto data para mantener la cadena (aunque no sea estrictamente necesario aquí)
    }
}
