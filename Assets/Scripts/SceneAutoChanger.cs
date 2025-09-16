using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAutoChanger : MonoBehaviour
{
    public void CambiarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }
}
