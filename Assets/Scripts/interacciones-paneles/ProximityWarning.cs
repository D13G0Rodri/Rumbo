using UnityEngine;
using TMPro; // Solo si usas TextMeshPro

public class ProximityWarning : MonoBehaviour
{
    [Header("Referencia al texto")]
    [SerializeField] private GameObject warningText; // El objeto de texto UI
    [SerializeField] [TextArea] private string message = "Advertencia por defecto";

    private void Start()
    {
        if (warningText != null)
        {
            warningText.SetActive(false); // Oculta el texto al inicio
            warningText.GetComponent<TextMeshProUGUI>().text = message;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && warningText != null)
        {
            warningText.SetActive(true); // Muestra el texto al acercarse
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && warningText != null)
        {
            warningText.SetActive(false); // Oculta al alejarse
        }
    }
}
