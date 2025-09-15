using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class ShowTPPrompt : MonoBehaviour
{
    [Header("UI compartida")]
    public GameObject panelTP;     // Referencia al PanelTP (único)
    public TMP_Text promptText;    // Referencia al PromptText (TMP)

    [Header("Mensaje de ESTA puerta/zona")]
    [TextArea] public string mensaje = "Presiona E para salir";
    public string playerTag = "Player";

    private void Reset()
    {
        var c = GetComponent<Collider2D>();
        if (c) c.isTrigger = true;          // Asegura que sea Trigger
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (promptText) promptText.text = mensaje;
        if (panelTP) panelTP.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (panelTP) panelTP.SetActive(false);
    }
}
