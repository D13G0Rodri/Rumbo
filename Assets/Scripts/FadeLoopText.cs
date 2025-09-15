using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeLoop : MonoBehaviour
{
    public float velocidad = 2f;      // qué tan rápido parpadea
    public float minAlpha = 0.3f;     // opacidad mínima
    public float maxAlpha = 1f;       // opacidad máxima

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        // Oscila suavemente entre min y max usando Mathf.PingPong
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.PingPong(Time.time * velocidad, 1)));
        canvasGroup.alpha = alpha;
    }
}
