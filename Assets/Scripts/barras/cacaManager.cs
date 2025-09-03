using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CacaManager : MonoBehaviour
{
    [Header("Configuración Caca")]
    [Tooltip("Tiempo en segundos para que la barra de incomodidad llegue a 100% después de que caca esté en 100")]
    public float tiempoLimiteLimpieza = 20f;
    [Tooltip("Cantidad de daño por ciclo (valor absoluto, p.ej. 0.1)")]
    [Range(0f, 10f)] public float danioPorCiclo = 0.1f;

    [Header("Referencias UI (opcional asignar en Inspector)")]
    public Image barraCaca; // si prefieres asignar desde inspector

    private Animator animator;
    private PlayerControllerBase playerController;

    private float contadorIncomodidadCaca = 0f;
    private bool isCryingInvoked = false;

    public UnityEvent OnBebeLlora;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerControllerBase>();

        // fallback por tag si no asignaste en inspector
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

        // Barra de caca: directamente proporcional (0 limpio -> 100 sucio)
        if (barraCaca != null)
        {
            barraCaca.fillAmount = Mathf.Clamp01(playerController.bathroom / 100f);
        }

        // Si la caca alcanzó el 100% -> empezar a llenar incomodidad
        if (playerController.bathroom >= 100f)
        {
            // Invocar el llanto una sola vez por evento hasta que limpies
            if (!isCryingInvoked)
            {
                BebéEmpiezaALlorar(); // invoca evento OnBebeLlora internamente
                isCryingInvoked = true;
            }

            // llenar la barra de incomodidad con el tiempo
            contadorIncomodidadCaca += Time.deltaTime;
            float incomodidadPct = Mathf.Clamp01(contadorIncomodidadCaca / tiempoLimiteLimpieza);

            // cuando la incomodidad llega a 100% -> aplicar daño y resetear contador para siguiente ciclo
            if (contadorIncomodidadCaca >= tiempoLimiteLimpieza)
            {
                contadorIncomodidadCaca = 0f; // reset para futuros ciclos
                if (playerController != null)
                {
                    playerController.ReceiveDamage(danioPorCiclo);
                    Debug.Log($"Daño por no limpiar aplicado: {danioPorCiclo}");
                }
            }
        }
        else
        {
            // si la caca baja por debajo de 100, resetear incomodidad y bandera de llanto
            if (contadorIncomodidadCaca > 0f) contadorIncomodidadCaca = 0f;
            isCryingInvoked = false;
        }
    }

    // Método que puede llamarse desde animaciones o inputs
    public void Cagar()
    {
        // incrementa caca (por ejemplo al realizar la acción o a intervalos)
        if (playerController != null)
        {
            playerController.bathroom = Mathf.Clamp(playerController.bathroom + 20f, 0f, 100f);
            animator.SetBool("isPoop", true);
            Debug.Log("El bebé ha hecho caca. Nivel ahora: " + playerController.bathroom);
        }
    }

    public void LimpiarBebe()
    {
        if (playerController != null)
        {
            playerController.bathroom = 0f;
            animator.SetBool("isPoop", false);
            contadorIncomodidadCaca = 0f;
            isCryingInvoked = false;
            Debug.Log("El bebé ha sido limpiado");
        }
    }

    public void BebéEmpiezaALlorar()
    {
        if (playerController != null && playerController.bathroom >= 100f)
        {
            OnBebeLlora?.Invoke();
            Debug.Log("¡El bebé está llorando! Se invocó OnBebeLlora.");
        }
    }
}
