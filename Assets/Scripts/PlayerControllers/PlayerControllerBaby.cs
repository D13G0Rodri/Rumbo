using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControllerBaby : PlayerControllerBase
{
    [Header("Configuración Bebé")]
    public float danioPorCiclo = 0.1f;
    private bool animationDead = false;

    protected override void Start()
    {
        PlayerControllerBase player = FindFirstObjectByType<PlayerControllerBase>();
        if (player != null)
            player.LoadGame();
        base.Start();
    }

    protected override void Update()
    {
        base.Update(); // Reutiliza movimiento y salto
        Cry();
    }

    // Llanto del bebé
    private void Cry()
    {
        if (Input.GetKeyDown(KeyCode.L))
            animator.SetBool("isCrying", true);
        if (Input.GetKeyUp(KeyCode.L))
            animator.SetBool("isCrying", false);
    }

    // Daño por falta de limpieza
    public void DañoPorNoLimpiar()
    {
        if (barraVidaImage != null)
        {
            health -= danioPorCiclo;
            barraVidaImage.fillAmount = health / VidaMax;
            Debug.Log($"¡Daño por falta de limpieza! Vida: {health}");
            if (health <= 0)
                Dead();
        }
    }

    // Animación de "hacer popó"
    public void hacerLaPopo()
    {
        int numCagar = 2;
        int numeroAleatorio = Random.Range(1, 10);
        if (numCagar == numeroAleatorio)
            animator.SetTrigger("hacerPopo");
    }

    // Sobrescribe Dead para mensaje específico
    protected override void Dead()
    {
        animator.SetBool("isDead", true);
        Debug.Log("¡El bebé ha muerto por falta de cuidados!");
        SceneManager.LoadScene("Muerte");
    }

    public override void SaveGame()
    {
        TimerVida timer = FindFirstObjectByType<TimerVida>();
        PlayerData data = new PlayerData
        {
            health = health,
            position = new float[] { transform.position.x, transform.position.y, transform.position.z },
            intelligence = intelligence,
            concentration = concentration,
            hunger = hunger,
            bathroom = bathroom,
            timerCount = timer != null ? timer.timerCount : 0f, // Guarda el tiempo del bebé
            currentSceneName = "Etapa-Niño" // Próxima escena
        };
        SaveSystem.SavePlayerData(data);
    }

    public override void LoadGame()
    {
        base.LoadGame(); // Carga los datos comunes
    }

}
