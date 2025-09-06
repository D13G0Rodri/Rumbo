using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerBaby : PlayerControllerBase
{
    // No necesitamos re-declarar danioPorCiclo aquí si el daño lo gestiona el evento.

    protected override void Start()
    {
        // CAMBIO: Eliminamos la llamada duplicada a LoadGame.
        // base.Start() se encargará de todo el proceso de inicialización y carga.
        base.Start();
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