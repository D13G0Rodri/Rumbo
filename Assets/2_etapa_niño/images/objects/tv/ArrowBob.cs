using UnityEngine;

public class ArrowBob : MonoBehaviour
{
    [Header("Movimiento")]
    public float amplitud = 0.2f;   // qué tanto sube/baja (unidades de mundo)
    public float velocidad = 2f;    // qué tan rápido
    public bool usarTiempoNoEscalado = true; // para que siga moviéndose si pausas con Time.timeScale = 0

    Vector3 posInicial;

    void Awake()
    {
        posInicial = transform.localPosition;   // bob alrededor de su posición local original
    }

    void Update()
    {
        float t = usarTiempoNoEscalado ? Time.unscaledTime : Time.time;
        float offsetY = Mathf.Sin(t * velocidad) * amplitud;
        transform.localPosition = new Vector3(posInicial.x, posInicial.y + offsetY, posInicial.z);
    }

    // si el objeto se activa/desactiva, resetea la base del bob
    void OnEnable() => posInicial = transform.localPosition;
}
