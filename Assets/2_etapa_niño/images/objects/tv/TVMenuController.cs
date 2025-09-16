using UnityEngine;
using UnityEngine.SceneManagement;

public class TVMenuController : MonoBehaviour
{
    public string escenaBuena = "TV_Buena"; // pantalla “buena”
    public string escenaMala  = "TV_Mala";  // pantalla “mala”

    public void ElegirDocumental() // botón VERDE
    {
        SceneManager.LoadScene(escenaBuena);
    }

    public void ElegirJugar() // botón ROJO
    {
        SceneManager.LoadScene(escenaMala);
    }

    // Opcional: tecla ESC para volver sin elegir
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && ReturnPoint.hasData)
            SceneManager.LoadScene(ReturnPoint.sceneName);
    }
}
