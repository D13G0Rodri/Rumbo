using UnityEngine;

public class AudioManagerSFXMenu : MonoBehaviour
{
    // Esta es la propiedad que causa el error si no está
    public static AudioManagerSFXMenu Instance { get; private set; }

    [Header("Fuentes de audio")]
    public AudioSource sfxSource;

    [Header("Clips de audio")]
    public AudioClip clickSound;
    public AudioClip hoverSound;

    void Awake()
    {
        // Singleton simple
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayClick()
    {
        sfxSource?.PlayOneShot(clickSound);
    }

    public void PlayHover()
    {
        sfxSource?.PlayOneShot(hoverSound);
    }
    
}
