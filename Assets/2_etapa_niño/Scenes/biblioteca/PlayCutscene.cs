using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(VideoPlayer))]
public class PlayCutscene : MonoBehaviour
{
    [SerializeField] private string backToSceneName = "Etapa-Niño"; // tu escena de juego

    void Start()
    {
        var vp = GetComponent<VideoPlayer>();
        vp.loopPointReached += OnVideoEnd;
        vp.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(backToSceneName);
    }
}
