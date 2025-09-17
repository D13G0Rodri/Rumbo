using UnityEngine;
using UnityEngine.SceneManagement;

public class BookChoiceUI : MonoBehaviour
{
    [Header("Escenas")]
    public string mathSceneName = "MatematicasScene";
    public string cultureSceneName = "CulturaScene";

    public void GoMath()    { SceneManager.LoadScene(mathSceneName); }
    public void GoCulture() { SceneManager.LoadScene(cultureSceneName); }
}
