using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoEndToMenu : MonoBehaviour
{
    [SerializeField] private string nextScene = "MenuPrincipal";
    private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached; // Evento cuando termina el video
    }

    void EndReached(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextScene);
    }
}
