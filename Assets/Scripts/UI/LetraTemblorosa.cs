using UnityEngine;
using TMPro;

public class LetraTemblorosa : MonoBehaviour
{
    public float shakeMagnitude = 10f;
    public float shakeSpeed = 25f;

    private RectTransform rectTransform;
    private Vector3 originalPosition;

    public bool vibrar = true;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        if (vibrar)
        {
            float x = Mathf.Sin(Time.time * shakeSpeed) * shakeMagnitude;
            rectTransform.anchoredPosition = originalPosition + new Vector3(x, 0, 0);
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition;
        }
    }
}
