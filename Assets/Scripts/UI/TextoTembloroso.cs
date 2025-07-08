using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextoTembloroso : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float shakeMagnitude = 5f;
    public float shakeSpeed = 30f;

    private Vector3 originalPosition;
    private bool temblando = false;
    private TextMeshProUGUI texto;

    void Start()
    {
        originalPosition = transform.localPosition;
        texto = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (temblando)
        {
            float x = Mathf.Sin(Time.time * shakeSpeed) * shakeMagnitude;
            transform.localPosition = originalPosition + new Vector3(x, 0, 0);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        temblando = true;
        AudioManagerSFXMenu.Instance?.PlayHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        temblando = false;
        transform.localPosition = originalPosition;
    }
}
