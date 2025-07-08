using UnityEngine;

public class AudioManagerSFX : MonoBehaviour
{
    [Header("Fuentes de audio")]
    public AudioSource sfxSource;

    [Header("Clips de audio")]
    public AudioClip acierto;
    public AudioClip fallo;
    public AudioClip victoria;
    public AudioClip derrota;

    public float ReproducirAcierto()
    {
        sfxSource.PlayOneShot(acierto);
        return acierto.length;
    }

    public float ReproducirFallo()
    {
        sfxSource.PlayOneShot(fallo);
        return fallo.length;
    }

    public float ReproducirVictoria()
    {
        sfxSource.PlayOneShot(victoria);
        return victoria.length;
    }

    public float ReproducirDerrota()
    {
        sfxSource.PlayOneShot(derrota);
        return derrota.length;
    }
}
