using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class StatChangeToast : MonoBehaviour
{
    [Header("Referencias a Texts (TMP)")]
    [SerializeField] private TMP_Text intelligenceDeltaText;
    [SerializeField] private TMP_Text karmaDeltaText;
    [SerializeField] private TMP_Text happinessDeltaText;

    [Header("Formato y color")]
    [SerializeField, Range(0,2)] private int decimalPlaces = 1;
    [SerializeField] private Color gainColor = new Color(0.2f, 0.9f, 0.3f); // verde
    [SerializeField] private Color lossColor = new Color(0.95f, 0.25f, 0.25f); // rojo

    [Header("Timing")]
    [SerializeField] private float showSeconds = 1.3f;   // tiempo visible
    [SerializeField] private float fadeSeconds = 0.35f;  // desvanecer
    [SerializeField] private Vector3 popOffset = new Vector3(0f, 40f, 0f); // leve subida

    [Header("Audio")]
    [SerializeField] private AudioSource toastSound; // arrastra aquí tu AudioSource en el Inspector

    private CanvasGroup cg;
    private Vector3 basePos;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        basePos = transform.localPosition;
        HideInstant();
    }

    void Start()
    {
        // ¿Hay toast pendiente?
        if (PlayerPrefs.GetInt("stat_toast_pending", 0) != 1)
            return;

        float dInt = PlayerPrefs.GetFloat("delta_intelligence", 0f);
        float dKar = PlayerPrefs.GetFloat("delta_karma",        0f);
        float dHap = PlayerPrefs.GetFloat("delta_happiness",    0f);

        // Armar textos (activar solo los que tengan delta != 0)
        string fmt = "F" + decimalPlaces;

        SetLine(intelligenceDeltaText, "Inteligencia", dInt, fmt);
        SetLine(karmaDeltaText,        "Karma",        dKar, fmt);
        SetLine(happinessDeltaText,    "Felicidad",    dHap, fmt);

        // Si todo es cero, no mostramos nada
        if (!HasAnyLine())
        {
            ClearFlag();
            return;
        }

        // Reproducir sonido al aparecer (solo si hay algo para mostrar)
        if (toastSound != null)
        {
            toastSound.Play();
        }

        // Mostrar con animación corta
        StartCoroutine(ShowAndFade());
    }

    private void SetLine(TMP_Text text, string label, float delta, string fmt)
    {
        if (!text) return;
        if (Mathf.Abs(delta) < 0.0001f) { text.gameObject.SetActive(false); return; }

        string sign = delta > 0 ? "+" : ""; // ToString ya pone el menos
        text.text = $"{label} = {sign}{delta.ToString(fmt)}";
        text.color = delta >= 0 ? gainColor : lossColor;
        text.gameObject.SetActive(true);
    }

    private bool HasAnyLine()
    {
        bool any =
            (intelligenceDeltaText && intelligenceDeltaText.gameObject.activeSelf) ||
            (karmaDeltaText        && karmaDeltaText.gameObject.activeSelf) ||
            (happinessDeltaText    && happinessDeltaText.gameObject.activeSelf);
        return any;
    }

    private System.Collections.IEnumerator ShowAndFade()
    {
        // pop-in
        cg.alpha = 0f;
        transform.localPosition = basePos - popOffset;

        // fade-in corto
        float t = 0f;
        while (t < 0.15f)
        {
            t += Time.unscaledDeltaTime;
            float k = t / 0.15f;
            cg.alpha = Mathf.SmoothStep(0f, 1f, k);
            transform.localPosition = Vector3.Lerp(basePos - popOffset, basePos, k);
            yield return null;
        }
        cg.alpha = 1f;
        transform.localPosition = basePos;

        // visible un rato
        float wait = showSeconds;
        while (wait > 0f)
        {
            wait -= Time.unscaledDeltaTime;
            yield return null;
        }

        // fade-out
        float f = 0f;
        while (f < fadeSeconds)
        {
            f += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, f / fadeSeconds);
            transform.localPosition = Vector3.Lerp(basePos, basePos + popOffset*0.5f, f / fadeSeconds);
            yield return null;
        }
        cg.alpha = 0f;

        // limpiar buffer para no repetir
        ClearFlag();
    }

    private void HideInstant()
    {
        cg.alpha = 0f;
        if (intelligenceDeltaText) intelligenceDeltaText.gameObject.SetActive(false);
        if (karmaDeltaText)        karmaDeltaText.gameObject.SetActive(false);
        if (happinessDeltaText)    happinessDeltaText.gameObject.SetActive(false);
    }

    private void ClearFlag()
    {
        PlayerPrefs.DeleteKey("delta_intelligence");
        PlayerPrefs.DeleteKey("delta_karma");
        PlayerPrefs.DeleteKey("delta_happiness");
        PlayerPrefs.DeleteKey("stat_toast_pending");
        PlayerPrefs.Save();
    }
}
