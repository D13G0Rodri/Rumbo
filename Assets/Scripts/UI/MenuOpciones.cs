using UnityEngine;
using UnityEngine.Audio;

public class MenuOpciones : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void PantallaCompleta(bool PantallaCompleta)
    {
        Screen.fullScreen = PantallaCompleta;
    }

    public void CambiarVolumenMusica(float volumen)
    {
        audioMixer.SetFloat("VolumenMusica", Mathf.Log10(Mathf.Max(volumen, 0.001f)) * 20);
    }

    public void CambiarVolumenSFX(float volumen)
    {
        audioMixer.SetFloat("VolumenSFX", Mathf.Log10(Mathf.Max(volumen, 0.001f)) * 20);
    }

    public void CambiarCalidad(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
}
