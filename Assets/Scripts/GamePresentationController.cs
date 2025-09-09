using UnityEngine;
using System.Collections; // Necesario para usar Corrutinas

/// <summary>
/// Controla la aparición de un panel de presentación y la activación de objetos
/// en un punto específico del temporizador del juego.
/// </summary>
public class GamePresentationController : MonoBehaviour
{
    [Header("Configuración de Presentación")]
    [Tooltip("Arrastra aquí el objeto del panel que quieres mostrar.")]
    public GameObject presentationPanel;

    [Tooltip("Referencia al script TimerVida que controla el tiempo del juego.")]
    public TimerVida timerVida;

    private const float activationPercentage = 0.5f; // El panel se activa al 50% del tiempo

    [Tooltip("El tiempo en segundos que el panel permanecerá visible antes de desaparecer.")]
    public float panelDisplayDuration = 4f; // Nueva variable para el tiempo de visualización

    private PushableObject[] pushableObjects;
    private bool hasActivated = false;
    private PlayerControllerBase playerBase;

    void Start()
    {
        // 1. Encontrar todos los objetos que se pueden empujar en la escena.
                pushableObjects = FindObjectsByType<PushableObject>(FindObjectsInactive.Include, FindObjectsSortMode.None); // Método moderno para incluir objetos inactivos

        // 2. Desactivar cada script PushableObject individualmente.
        foreach (var obj in pushableObjects)
        {
            obj.enabled = false;
        }

        // 3. Asegurarse de que el panel de presentación esté oculto al empezar.
        if (presentationPanel != null)
        {
            presentationPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No se ha asignado un panel de presentación en el GamePresentationController.", this);
        }

        // 4. Si no se asignó el TimerVida en el inspector, intentar encontrarlo en la escena.
        if (timerVida == null)
        {
            timerVida = FindFirstObjectByType<TimerVida>();
            if (timerVida == null)
            {
                Debug.LogError("No se encontró una instancia de TimerVida en la escena. El controlador no funcionará.", this);
                this.enabled = false; // Desactivar este script si no hay temporizador.
            }
        }

        // 5. Encontrar el controlador del jugador y sincronizar el estado 'hasActivated'.
        playerBase = FindFirstObjectByType<PlayerControllerBase>();
        if (playerBase != null)
        {
            hasActivated = playerBase.presentationPanelShown;
        }
        else
        {
            Debug.LogWarning("No se encontró PlayerControllerBase en la escena. El estado de presentación no se puede sincronizar.");
        }
    }

    void Update()
    {
        // No hacer nada si la acción ya se ejecutó o si no hay temporizador.
        if (hasActivated || timerVida == null)
        {
            return;
        }

        // Calcular el porcentaje actual del tiempo.
        float currentPercentage = timerVida.timerCount / timerVida.maxTime;

        // Comprobar si hemos alcanzado o superado el porcentaje de activación.
        if (currentPercentage >= activationPercentage)
        {
            // Iniciar la secuencia de mostrar y ocultar el panel.
            if (presentationPanel != null)
            {
                StartCoroutine(ShowAndHidePanelRoutine());
            }

            // Reactivar todos los scripts de los objetos empujables.
            foreach (var obj in pushableObjects)
            {
                obj.enabled = true;
            }

            // Marcar como activado para que este bloque de código no se vuelva a ejecutar.
            hasActivated = true;

            // Guardar el estado para que no se repita en futuras sesiones
            if (playerBase != null)
            {
                playerBase.presentationPanelShown = true;
                playerBase.SaveCheckpoint();
            }

            Debug.Log($"Presentación activada al {currentPercentage:P1}. Objetos empujables habilitados.");
        }
    }

    /// <summary>
    /// Corrutina que activa el panel, espera un tiempo y luego lo desactiva.
    /// </summary>
    private IEnumerator ShowAndHidePanelRoutine()
    {
        // 1. Activar el panel.
        presentationPanel.SetActive(true);

        // 2. Esperar el tiempo especificado en la variable pública.
        yield return new WaitForSeconds(panelDisplayDuration);

        // 3. Desactivar el panel.
        presentationPanel.SetActive(false);
    }
}