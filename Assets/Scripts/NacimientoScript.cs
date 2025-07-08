using UnityEngine;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;

public class NacimientoScript : MonoBehaviour
{
    private TMP_Text keyToPress;
    List<Char> keys = new List<Char>("QWERTYUIOPASDFGHJKLZXCVBNM".ToCharArray());
    public int numberOfLetters = 10;
    private int currentNumberOfLetters;
    char currentKey;

    int successes = 0;
    int failed = 0;

    private float timeLimit = 2.0f;
    private float timer = 0.0f;

    public float TiempoRestante => Mathf.Clamp(timeLimit - timer, 0, timeLimit);
    public bool TimerActivo => currentNumberOfLetters < numberOfLetters;

    private AudioManagerSFX audioManager;
    private AudioManagerMusica audioManagerMusica; // <- nuevo

    void Start()
    {
        GameObject textObject = GameObject.FindWithTag("PresionarTecla");
        if (textObject != null)
        {
            keyToPress = textObject.GetComponent<TextMeshProUGUI>();
            GenerateNewLetter();
        }
        else
        {
            Debug.LogError("No se encontró un objeto con el tag 'PresionarTecla'.");
        }

        audioManager = FindAnyObjectByType<AudioManagerSFX>();
        if (audioManager == null)
            Debug.LogError("No se encontró el AudioManagerSFX en la escena.");

        audioManagerMusica = FindAnyObjectByType<AudioManagerMusica>(); // <- nuevo
        if (audioManagerMusica == null)
            Debug.LogWarning("No se encontró el AudioManagerMusica en la escena.");
    }

    void Update()
    {
        if (currentNumberOfLetters >= numberOfLetters) return;

        timer += Time.deltaTime;

        if (timer >= timeLimit)
        {
            failed++;
            Debug.Log("Failed (Timeout)");
            audioManager?.ReproducirFallo();
            currentNumberOfLetters++;
            timer = 0.0f;

            ContinuarJuego();
        }

        if (Input.inputString.Length > 0)
        {
            char keyPress = Input.inputString[0];
            if (char.ToUpper(keyPress) == currentKey)
            {
                successes++;
                Debug.Log("Correct");
                audioManager?.ReproducirAcierto();
            }
            else
            {
                failed++;
                Debug.Log("Failed");
                audioManager?.ReproducirFallo();
            }

            currentNumberOfLetters++;
            timer = 0.0f;
            ContinuarJuego();
        }
    }

    void ContinuarJuego()
    {
        if (currentNumberOfLetters < numberOfLetters)
        {
            GenerateNewLetter();
        }
        else
        {
            EndGame();
        }
    }

    void GenerateNewLetter()
    {
        int randomIndex = UnityEngine.Random.Range(0, keys.Count);
        currentKey = keys[randomIndex];
        keyToPress.text = currentKey.ToString();
        timer = 0.0f;
    }

    void EndGame()
    {
        Debug.Log("Correctas: " + successes);
        Debug.Log("Fallidas: " + failed);

        // Detener música antes de reproducir sonidos finales
        audioManagerMusica?.DetenerMusica();

        float duracion = 1f;

        if (successes > failed)
        {
            keyToPress.text = "Sobreviviste al nacimiento";
            duracion = audioManager?.ReproducirVictoria() ?? 1f;
            StartCoroutine(CambiarEscenaDespues(duracion, 1));
        }
        else if (successes == failed)
        {
            keyToPress.text = "Sobreviviste... a qué costo";
            duracion = audioManager?.ReproducirVictoria() ?? 1f;
            StartCoroutine(CambiarEscenaDespues(duracion, 1));
        }
        else
        {
            keyToPress.text = "No sobreviviste al nacimiento";
            duracion = audioManager?.ReproducirDerrota() ?? 1f;
            StartCoroutine(CambiarEscenaDespues(duracion, -1));
        }
    }

    IEnumerator CambiarEscenaDespues(float delay, int offset)
    {
        yield return new WaitForSeconds(delay);
        int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene + offset);
    }
}
