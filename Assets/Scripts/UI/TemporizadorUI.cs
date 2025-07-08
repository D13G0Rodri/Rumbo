using UnityEngine;
using TMPro;

public class TemporizadorUI : MonoBehaviour
{
    public NacimientoScript nacimientoScript; // Referencia al script principal
    private TMP_Text timerText;

    public float pulsationSpeed = 3f;     // Latidos por segundo (ajustado)
    public float pulsationMagnitude = 0.5f; // Aumento de tamaño (50%)

    private Vector3 originalScale;
    private Color originalColor = Color.white;
    private Color peakColor = Color.red;

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
        originalScale = transform.localScale;
        originalColor = timerText.color;

        if (nacimientoScript == null)
        {
            Debug.LogError("Referencia a NacimientoScript no asignada en TemporizadorUI.");
        }
    }

    void Update()
    {
        if (nacimientoScript != null && nacimientoScript.TimerActivo)
        {
            float tiempoRestante = nacimientoScript.TiempoRestante;
            timerText.text = tiempoRestante.ToString("F1") + "s";

            // Escalado tipo latido fuerte
            float t = Mathf.PingPong(Time.time * pulsationSpeed, 1f);
            float scaleFactor = 1 + t * pulsationMagnitude;
            transform.localScale = originalScale * scaleFactor;

            // Cambio de color según la fase
            timerText.color = Color.Lerp(originalColor, peakColor, t);
        }
        else
        {
            // Reiniciar valores
            transform.localScale = originalScale;
            timerText.color = originalColor;
        }
    }
}
