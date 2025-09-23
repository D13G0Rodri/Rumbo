using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void NuevaPartida()
    {
        SaveSystem.DeleteSave();
        SceneManager.LoadScene("JuegoNacer");
    }

    public void ContinuarPartida()
    {
        var data = SaveSystem.LoadPlayerData();
        if (data != null && !string.IsNullOrEmpty(data.currentSceneName))
        {
            SceneManager.LoadScene(data.currentSceneName);
        }
        else
        {
            Debug.Log("No hay partida guardada o la escena guardada es inválida.");
        }
    }

    public void Salir()
    {
        Application.Quit();
    }
}
