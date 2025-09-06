using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CacaManager : MonoBehaviour
{
    [Header("Configuración Caca")]
    [Tooltip("Tiempo en segundos para que la barra de incomodidad se llene y aplique daño")]
    public float tiempoLimiteLimpieza = 20f;
    [Tooltip("Cantidad de daño por ciclo cuando no se limpia")]
    [Range(0f, 10f)] public float danioPorCiclo = 0.1f;

    [Header("Referencias UI")]
    [Tooltip("Asigna la imagen de la barra de caca aquí")]
    public Image barraCaca;

    private Animator animator;
    private PlayerControllerBase playerController;
    private ComidaManager comidaManager;
    private PapaController papaController;

    private float contadorIncomodidadCaca = 0f;
    private bool isCryingInvoked = false;

    public UnityEvent OnBebeLlora;

    public bool yaSeHizo = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerControllerBase>();
        comidaManager = GetComponent<ComidaManager>();
        papaController = GetComponent<PapaController>();

        if (barraCaca == null)
        {
            GameObject objBarraCaca = GameObject.FindWithTag("barraCaca");
            if (objBarraCaca != null)
                barraCaca = objBarraCaca.GetComponent<Image>();
        }
    }

    void Update()
    {
        if (playerController == null) return;

        // La variable 'bathroom' del playerController se incrementa automáticamente en PlayerControllerBase.
        // Este script solo reacciona a esos cambios.
        if (barraCaca != null)
        {
            barraCaca.fillAmount = Mathf.Clamp01(playerController.bathroom / 100f);
        }

        if (playerController.bathroom >= 100f)
        {
            if (!yaSeHizo)
            {
                animator.SetBool("isPoop", true);
                yaSeHizo = true;
            }

            contadorIncomodidadCaca += Time.deltaTime;
            if (contadorIncomodidadCaca >= tiempoLimiteLimpieza)
            {
                contadorIncomodidadCaca = 0f;
                // Llamamos a la función de daño genérica del PlayerControllerBase
                playerController.ReceiveDamage(danioPorCiclo);
                Debug.Log($"Daño por no limpiar aplicado: {danioPorCiclo}");
            }
        }
        else
        {
            // Si el bebé está limpio, reseteamos el estado
            animator.SetBool("isPoop", false);
            contadorIncomodidadCaca = 0f;
            isCryingInvoked = false;
            yaSeHizo = false;
        }
    }

    // Este método es la forma principal de limpiar al bebé.
    public void LimpiarBebe()
    {
        if (playerController != null)
        {
            playerController.bathroom = 0f;
            // La lógica en Update se encargará de resetear la animación y los contadores.
            Debug.Log("El bebé ha sido limpiado");
        }
    }

    // Se conserva por si es usado por eventos.
    public void BebéEmpiezaALlorar()
    {
        if (playerController != null && playerController.bathroom >= 100f)
        {
            OnBebeLlora?.Invoke();
            Debug.Log("¡El bebé esta cagado! El papa está llendo a limpiarlo.");

        }
        if(playerController != null && playerController.hunger <= 0f)
        {
            comidaManager.BebéEmpiezaALlorar();
            Debug.Log("¡El bebé tiene hambre! La mamá está llendo a alimentarlo.");
        }  
    }

    public void dejarDeCagar()
    {
        animator.SetBool("isPoop", false);
        
    }
}