using UnityEngine;
using UnityEngine.Video;

public class SaltarEscenaAMuerteCorrectamente : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public SceneAutoChanger sceneChanger;
    private bool videoPrepared = false;

    void Start()
    {
        // Obtén el componente VideoPlayer del objeto al que está adjunto este script
        videoPlayer = GetComponent<VideoPlayer>();

        // Asegúrate de que el VideoPlayer no sea nulo
        if (videoPlayer == null)
        {
            Debug.LogError("No se encontró el componente VideoPlayer.");
            return;
        }

        // Configura el VideoPlayer para que reproduzca automáticamente al inicio
        videoPlayer.playOnAwake = true;

        // Prepara el video para su reproducción
        videoPlayer.Prepare();

        // Suscríbete al evento prepareCompleted para comenzar la reproducción una vez que el video esté listo
        videoPlayer.prepareCompleted += Prepared;

        // Obtén el componente SceneAutoChanger
        sceneChanger = FindFirstObjectByType<SceneAutoChanger>();

        // Asegúrate de que el SceneAutoChanger no sea nulo
        if (sceneChanger == null)
        {
            Debug.LogError("No se encontró el componente SceneAutoChanger.");
        }
    }

    void Prepared(VideoPlayer vp)
    {
        videoPrepared = true;
        vp.Play();
        Debug.Log("Video preparado y reproduciéndose.");
    }

    void Update()
    {
        // Verifica si el VideoPlayer no está reproduciéndose y si el video ya fue preparado
        if (videoPrepared && videoPlayer != null && !videoPlayer.isPlaying)
        {
            // Cambia la escena si el video no está reproduciéndose
            if (sceneChanger != null)
            {
                sceneChanger.CambiarEscena("Muerte");
            }
        }
    }
}
