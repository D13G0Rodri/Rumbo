using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HandHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Movimiento al pasar el mouse")]
    public RectTransform target;          // el RectTransform del propio botón
    public float nudgePixels = 20f;       // cuánto se mueve
    public float nudgeTime = 0.12f;       // velocidad

    [Header("Tooltip")]
    public TextMeshProUGUI tooltip;       // texto arriba
    [TextArea] public string tooltipText;

    [Header("Al hacer click")]
    public string sceneToLoad;            // escena de animación a cargar

    Vector2 originalPos;
    bool animating;

    void Awake()
    {
        if (!target) target = GetComponent<RectTransform>();
        originalPos = target.anchoredPosition;
        if (tooltip) tooltip.gameObject.SetActive(false);
        // Asegura que el botón no tiene texto por defecto
        var tmp = GetComponentInChildren<TextMeshProUGUI>();
        if (tmp) tmp.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip)
        {
            tooltip.text = tooltipText;
            tooltip.gameObject.SetActive(true);
        }
        StartCoroutine(NudgeTo(originalPos + new Vector2(0, nudgePixels)));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip) tooltip.gameObject.SetActive(false);
        StartCoroutine(NudgeTo(originalPos));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }

    System.Collections.IEnumerator NudgeTo(Vector2 targetPos)
    {
        if (animating) yield break;
        animating = true;
        float t = 0f;
        Vector2 start = target.anchoredPosition;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / nudgeTime;
            target.anchoredPosition = Vector2.Lerp(start, targetPos, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
        target.anchoredPosition = targetPos;
        animating = false;
    }
}
