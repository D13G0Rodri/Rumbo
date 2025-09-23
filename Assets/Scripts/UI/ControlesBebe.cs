using UnityEngine;

public class ControlesBebe : MonoBehaviour
{
    [Header("Panel UI")]
    public GameObject panel;  // Arrastra aquí tu panel de BebeGatea

    private void Start()
    {
        // Asegurarse de que el panel está desactivado al inicio
        if (panel != null)
            panel.SetActive(false);

        // Chequear si es la primera vez que entra a esta escena
        if (!PlayerPrefs.HasKey("BebeGatea_FirstTime"))
        {
            ShowPanel();
            PlayerPrefs.SetInt("BebeGatea_FirstTime", 1); // Marca que ya entró
        }
    }

    // Hacer público para depuración y asegurar que Unity lo detecte
    public void ShowPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
            Time.timeScale = 0f; // Congela el juego
        }
    }

    // Método público para asignar al botón PS
    public void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
            Time.timeScale = 1f; // Reanuda el juego
        }
    }
}
