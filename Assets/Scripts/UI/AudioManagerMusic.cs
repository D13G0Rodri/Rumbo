using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerMusica : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip musicaFondo;
    public AudioMixer audioMixer;

    void Start()
    {
        if (musicaFondo != null && musicSource != null)
        {
            musicSource.clip = musicaFondo;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void CambiarVolumenMusica(float volumen)
    {
        audioMixer.SetFloat("VolumenMusica", Mathf.Log10(Mathf.Max(volumen, 0.001f)) * 20);
    }

    public void DetenerMusica()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}
