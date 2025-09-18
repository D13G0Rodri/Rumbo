using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class InteractionHint : MonoBehaviour
{
    [Header("Shared UI")]
    public GameObject promptPanel;
    public TMP_Text promptText;

    [Header("Message for this object")]
    [TextArea] public string message = "Press [E] to interact";
    public string playerTag = "Player";

    [Header("Audio (optional)")]
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

        if (promptText != null) promptText.text = message;

        if (promptPanel != null && !promptPanel.activeSelf)
        {
            promptPanel.SetActive(true);

            if (audioSource != null && appearSound != null)
                audioSource.PlayOneShot(appearSound, appearVolume);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (promptPanel != null && promptPanel.activeSelf)
            promptPanel.SetActive(false);
    }
}
