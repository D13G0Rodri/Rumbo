using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class ShowTPPrompt : MonoBehaviour
{
    [Header("UI compartida")]
    public GameObject panelTP;
    public TMP_Text promptText;

    [Header("Mensaje de esta puerta")]
    [TextArea] public string mensaje = "Presiona E para salir";
    public string playerTag = "Player";

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip appearSound;
    [Range(0f, 1f)] public float appearVolume = 0.5f;

    private void Reset()
    {
        var c = GetComponent<Collider2D>();
        if (c) c.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (promptText != null) promptText.text = mensaje;

        // Solo si el panel estaba apagado, lo encendemos y sonamos el ding
        if (panelTP != null && !panelTP.activeSelf)
        {
            panelTP.SetActive(true);

            if (audioSource != null && appearSound != null)
                audioSource.PlayOneShot(appearSound, appearVolume);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (panelTP != null && panelTP.activeSelf) panelTP.SetActive(false);
    }
}
