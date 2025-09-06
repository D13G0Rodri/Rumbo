using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneChanger : MonoBehaviour
{
    [Header("Configuración del Video")]
    [Tooltip("Asigna el VideoPlayer que reproducirá el video.")]
    public VideoPlayer videoPlayer;

    private bool videoPrepared = false;
    
    [SerializeField]
    public string nextSceneName; // Ahora es privado y se asigna por método

    void Start()
    {
        // Obtén el VideoPlayer si no está asignado
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
            if (videoPlayer == null)
            {
                Debug.LogError("No se encontró el componente VideoPlayer en este GameObject.");
                return;
            }
        }

        // Configuración del VideoPlayer
        videoPlayer.playOnAwake = false;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        videoPrepared = true;
        vp.Play();
        Debug.Log("Video preparado y reproduciéndose.");
    }

    void Update()
    {
        // Si el video ya se preparó y terminó de reproducirse, cambia de escena
        if (videoPrepared && videoPlayer != null && !videoPlayer.isPlaying && !string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
            Debug.Log($"Cambiando a la escena: {nextSceneName}");
        }
    }

    // Método público para asignar la escena de destino y empezar el proceso
    public void PlayVideoAndChangeScene(string sceneName)
    {
        nextSceneName = sceneName;
        Debug.Log($"Escena de destino configurada: {nextSceneName}");
    }
}
