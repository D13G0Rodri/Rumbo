using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerTeen : PlayerControllerBase
{
    [Header("Configuración Adolescente")]
    public float happiness = 100f; // Valor inicial por si no hay datos guardados.
    public float socialImpact = 5f;

    protected override void Start()
    {
        // CAMBIO: Dejamos que base.Start() maneje la inicialización y la llamada a LoadGame.
        base.Start();
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
            // Asegúrate de que los valores no se pasen de un límite si es necesario
            happiness = Mathf.Clamp(happiness, 0, 100); 
            
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

    // CAMBIO: Sobrescribimos CreatePlayerData para añadir la felicidad.
    protected override PlayerData CreatePlayerData()
    {
        // 1. Obtenemos los datos base.
        PlayerData data = base.CreatePlayerData();

        // 2. Añadimos la información específica del adolescente.
        data.happiness = this.happiness;
        // data.currentSceneName = "Etapa-Adulto"; // Si hubiera una siguiente etapa

        // 3. Devolvemos el objeto completo.
        return data;
    }

    // CAMBIO: Ahora LoadGame recibe el PlayerData del base.LoadGame()
    public override PlayerData LoadGame()
    {
        // 1. Llamamos a la implementación base para cargar los datos comunes.
        PlayerData data = base.LoadGame();

        // 2. Si hay datos, aplicamos los específicos de esta clase.
        if (data != null)
        {
            happiness = data.happiness; // Cargamos la felicidad del adolescente
        }
        
        return data; // Retornamos el objeto data.
    }
}