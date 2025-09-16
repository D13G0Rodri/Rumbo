using UnityEngine;
using UnityEngine.SceneManagement;

public class MUERTE : MonoBehaviour
{
    private AudioSource audioSource;
    private bool audioIniciado = false;

    void Start()
    {
        // 1. Obtener el componente AudioSource que está en el mismo GameObject.
        // Asegúrate de que tu GameObject tenga un componente AudioSource con un clip de audio asignado en el Inspector.
        audioSource = GetComponent<AudioSource>();

        // 2. Comprobar que el AudioSource y su clip existen, y luego reproducirlo.
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            audioIniciado = true; // Marcamos que el audio ha comenzado.
            SaveSystem.DeleteSave();
        }
        else
        {
            // Si no hay audio, es útil saberlo para depurar.
            Debug.LogWarning("MUERTE.cs: No se encontró un AudioSource o no tiene un AudioClip asignado. Se cambiará de escena inmediatamente.");
            // Si no hay audio configurado, cargamos la escena directamente para no quedarnos atascados.
            SaveSystem.DeleteSave();
            SceneManager.LoadScene("MenuPrincipal");
        }
    }

    void Update()
    {
        // 3. Comprobar si el audio ya se inició y si ha terminado de reproducirse.
        if (audioIniciado && !audioSource.isPlaying)
        {
            // Cuando el audio termina, !audioSource.isPlaying será true y cambiaremos de escena.
            
            SceneManager.LoadScene("MenuPrincipal");
        }
    }
}
