using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ReturnAfterDelayToSaved : MonoBehaviour
{
    public float segundos = 5f;

    void Start()
    {
        StartCoroutine(Volver());
    }

    IEnumerator Volver()
    {
        // usa tiempo REAL por si pausas el juego
        yield return new WaitForSecondsRealtime(segundos);

        if (ReturnPoint.hasData)
            SceneManager.LoadScene(ReturnPoint.sceneName);
        else
            SceneManager.LoadScene("Etapa-Niño"); // por si faltan datos
    }
}
