using System;
using UnityEngine;
using UnityEngine.UI;

public class RandomEvent : MonoBehaviour
{
    // === CONFIGURACIÓN EDITABLE DESDE INSPECTOR ===
    [Header("Configuración")]
    [Tooltip("Tiempo entre posibles eventos de caca (segundos)")]
    public float intervaloEntreCacas = 15f;

    [Tooltip("Tiempo para limpiar antes de que ocurra daño (segundos)")]
    public float tiempoLimiteLimpieza = 20f;

    [Tooltip("Cantidad de daño por ciclo (0-1)")]
    [Range(0, 1)] public float danioPorCiclo = 0.1f;

    // === REFERENCIAS INTERNAS ===
    private Animator animator;
    private PlayerControllerBaby playerController;

    private Image barraVidaImage;
    private Image barraCaca;

    // === ESTADO INTERNO ===
    private float tiempoProximaCaca;
    private float contadorIncomodidad;
    private float conteoProximaCaca = 0f;

    private bool necesitaLimpieza;

    // === MÉTODO DE INICIO ===
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerControllerBaby>();

        // Buscar y asignar la barra de caca
        GameObject objBarraCaca = GameObject.FindWithTag("barraCaca");
        if (objBarraCaca)
            barraCaca = objBarraCaca.GetComponent<Image>();

        // Buscar y asignar la barra de vida
        GameObject barraVidaObj = GameObject.FindWithTag("barraVida");
        if (barraVidaObj != null)
        {
            barraVidaImage = barraVidaObj.GetComponent<Image>();
            if (barraVidaImage == null)
                Debug.LogError("El objeto con tag 'barraVida' no tiene componente Image");
        }
        else
        {
            Debug.LogError("No se encontró objeto con tag 'barraVida'");
        }

        // Inicializar estados
        tiempoProximaCaca = Time.time + intervaloEntreCacas;
        contadorIncomodidad = 0f;
        necesitaLimpieza = false;
    }

    // === LOOP PRINCIPAL ===
    void Update()
    {
        if (!necesitaLimpieza)
        { 
            actualizarImagenCaca();
        }
        

        // Verifica si es momento de "cagar"
        if (Time.time >= tiempoProximaCaca && !necesitaLimpieza)
        {
            Cagar();
        }

        // Si el bebé está sucio, empieza a acumular incomodidad
        if (necesitaLimpieza)
        {
            contadorIncomodidad += Time.deltaTime;

            if (contadorIncomodidad >= tiempoLimiteLimpieza)
            {
                contadorIncomodidad = 0f;
                playerController.DañoPorNoLimpiar();
            }
        }
    }

    // === FUNCIONES DE EVENTOS ===
    void Cagar()
    {
        animator.SetBool("isPoop", true);
        necesitaLimpieza = true;
        tiempoProximaCaca = Time.time + intervaloEntreCacas;
        conteoProximaCaca = 0;
        Debug.Log("¡El bebé se ha cagado! Necesita limpieza");
    }

    public void LimpiarBebe()
    {
        animator.SetBool("isPoop", false);
        necesitaLimpieza = false;
        contadorIncomodidad = 0f;
        Debug.Log("El bebé ha sido limpiado");
    }

    public void dejaDeCagar()
    {
        animator.SetBool("isPoop", false);
    }

    // === BARRA DE PROGRESO ===
    public void actualizarImagenCaca()
    {
        conteoProximaCaca += Time.deltaTime;
        barraCaca.fillAmount = conteoProximaCaca / intervaloEntreCacas;
    }
}