using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class ShowTalkPrompt : MonoBehaviour
{
    [Header("UI compartida")]
    public GameObject panelPrompt;   // GameObject que contiene el texto (ej: "Hablar [Ctrl]")
    public TMP_Text promptText;      // Texto TMP dentro del panel

    [Header("Mensaje de este objeto")]
    [TextArea] public string mensaje = "Presiona [Ctrl] para hablar";
    public string playerTag = "Player";

    [Header("Audio (opcional)")]
    public AudioSource audioSource;
    public AudioClip appearSound;
    [Range(0f, 1f)] public float appearVolume = 0.5f;

    private void Reset()
    {
        var c = GetComponent<Collider2D>();
        if (c) c.isTrigger = true; // aseguramos que sea trigger
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (promptText != null) promptText.text = mensaje;

        // Solo si estaba apagado, lo encendemos y reproducimos sonido
        if (panelPrompt != null && !panelPrompt.activeSelf)
        {
            panelPrompt.SetActive(true);

            if (audioSource != null && appearSound != null)
                audioSource.PlayOneShot(appearSound, appearVolume);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (panelPrompt != null && panelPrompt.activeSelf)
            panelPrompt.SetActive(false);
    }
}
