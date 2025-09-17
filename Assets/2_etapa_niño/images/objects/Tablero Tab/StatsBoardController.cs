using UnityEngine;
using TMPro;

public class StatsBoardController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource chainSound;
    [SerializeField] private bool pauseGameWhenOpen = true;

    [Header("UI Text (TMP)")]
    [SerializeField] private TMP_Text intelligenceText;
    [SerializeField] private TMP_Text karmaText;
    [SerializeField] private TMP_Text happinessText;

    [Header("Formato")]
    [SerializeField, Range(0, 3)] private int decimalPlaces = 1; // 0, 1 o 2 decimales

    private bool isOpen;

    void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!chainSound) chainSound = GetComponent<AudioSource>();
        SetOpen(false, instant: true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Toggle();

        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            Toggle();
    }

    public void Toggle() => SetOpen(!isOpen);

    public void SetOpen(bool open, bool instant = false)
    {
        isOpen = open;
        animator.SetBool("IsOpen", open);

        if (pauseGameWhenOpen)
        {
            if (open)
            {
                Time.timeScale = 0f;
                animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            }
            else
            {
                Time.timeScale = 1f;
                animator.updateMode = AnimatorUpdateMode.Normal;
            }
        }

        if (open && chainSound != null)
            chainSound.Play();

        if (open)
            RefreshFromSave();

        if (instant)
            animator.Update(0f);
    }

    private void RefreshFromSave()
    {
        var data = SaveSystem.LoadPlayerData();

        if (data == null)
        {
            SetTexts("--", "--", "--");
            return;
        }

        float intel = Clamp01To100(data.intelligence);
        float k     = Clamp01To100(data.karma);
        float happ  = Clamp01To100(data.happiness);

        string fmt = "F" + decimalPlaces; // "F0", "F1", "F2"
        SetTexts(intel.ToString(fmt), k.ToString(fmt), happ.ToString(fmt));
    }

    private void SetTexts(string intel, string karma, string happ)
    {
        if (intelligenceText) intelligenceText.text = intel;
        if (karmaText)        karmaText.text        = karma;
        if (happinessText)    happinessText.text    = happ;
    }

    private static float Clamp01To100(float v)
    {
        return Mathf.Clamp(v, 0f, 100f);
    }
}
