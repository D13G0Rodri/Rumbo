using UnityEngine;

public class ControlesNiño : MonoBehaviour
{
    [Header("Panel UI")]
    public GameObject panel;  // Arrastra aquí tu panel de controles de Niño

    private void Start()
    {
        // Asegurarse de que el panel esté desactivado al inicio
        if (panel != null)
            panel.SetActive(false);

        // Chequear si es la primera vez que entra a esta escena
        if (!PlayerPrefs.HasKey("EtapaNiño_FirstTime"))
        {
            ShowPanel();
            PlayerPrefs.SetInt("EtapaNiño_FirstTime", 1); // Marca que ya entró
            PlayerPrefs.Save();
        }
    }

    // Mostrar el panel y congelar el juego
    public void ShowPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
            Time.timeScale = 0f; // Congela el juego
        }
    }

    // Método público para asignar al botón "PS"
    public void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
            Time.timeScale = 1f; // Reanuda el juego
        }
    }
}
