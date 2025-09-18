using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void NuevaPartida()
    {
        if (GameMemory.I != null)
        {
            GameMemory.I.ResetSavedPosition();
        }
        SceneManager.LoadScene("nacimiento");
    }

    public void ContinuarPartida()
    {
        SceneManager.LoadScene("BebeGatea");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
