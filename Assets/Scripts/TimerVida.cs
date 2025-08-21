using UnityEngine;
using UnityEngine.UI;

public class TimerVida : MonoBehaviour
{
    public float timerCount = 0f;
    [SerializeField] public float maxTime = 1200f;
    private Image barraTiempo;
    public GameObject panelTextToFinish;

    // Nuevo: referencias y flag para guardar una sola vez al completar
    private PlayerControllerBase playerBase;
    private bool hasSavedOnComplete = false;

    void Start()
    {
        GameObject objBarraTiempo = GameObject.FindWithTag("barraTiempo");
        if (objBarraTiempo)
        {
            barraTiempo = objBarraTiempo.GetComponent<Image>();
        }
        if (panelTextToFinish)
        {
            panelTextToFinish.SetActive(false);
        }

        // CAMBIO: El timer ahora busca al jugador y carga su propio estado DESDE el jugador.
        // Como el Start() del jugador ya cargó los datos, aquí obtendremos el valor correcto.
        playerBase = FindFirstObjectByType<PlayerControllerBase>();
        if (playerBase != null)
        {
            // Inicializamos el contador con el valor que el jugador cargó desde el archivo.
            timerCount = playerBase.timerCount;
        }

        // Fuerza actualizar la UI al valor inicial cargado
        if (barraTiempo != null)
        {
            barraTiempo.fillAmount = timerCount / maxTime;
        }

        // Suscripción al evento de carga de datos para sincronizar si el orden de ejecución cambia
        PlayerControllerBase.OnGameDataLoaded += HandleGameDataLoaded;
    }

    void Update()
    {
        timerCount += Time.deltaTime; // Siempre incrementamos.
        
        if (timerCount >= maxTime)
        {
            timerCount = maxTime; // Evita que el tiempo siga corriendo
            if (!panelTextToFinish.activeSelf)
            {
                panelTextToFinish.SetActive(true);
            }

            // Nuevo: Guardado automático al completar (solo una vez)
            if (!hasSavedOnComplete)
            {
                hasSavedOnComplete = true;
                if (playerBase != null)
                {
                    float vidaAntesDeGuardar = playerBase.health;
                    playerBase.SaveGame();
                    Debug.Log($"Guardado automático al 100% de tiempo. Vida guardada={vidaAntesDeGuardar}, Tiempo={timerCount}");
                }
                else
                {
                    Debug.LogWarning("No se encontró PlayerController para guardar al 100% del tiempo.");
                }
            }
        }
        
        // Actualizamos la barra de tiempo en cada frame.
        if (barraTiempo != null)
        {
            barraTiempo.fillAmount = timerCount / maxTime;
        }
    }

    private void OnDestroy()
    {
        PlayerControllerBase.OnGameDataLoaded -= HandleGameDataLoaded;
    }

    private void HandleGameDataLoaded(PlayerData data)
    {
        // Actualizamos el contador y la barra con el valor cargado del archivo
        timerCount = data.timerCount;
        if (barraTiempo != null)
        {
            barraTiempo.fillAmount = timerCount / maxTime;
        }
        
        // Si ya está completo al cargar, mostramos el panel y no volvemos a guardar automáticamente
        if (timerCount >= maxTime)
        {
            timerCount = maxTime;
            if (panelTextToFinish && !panelTextToFinish.activeSelf)
                panelTextToFinish.SetActive(true);
            hasSavedOnComplete = true; // Evita disparo de guardado inmediato post-carga
        }

        Debug.Log($"Timer sincronizado desde guardado: {timerCount}/{maxTime}");
    }
}
