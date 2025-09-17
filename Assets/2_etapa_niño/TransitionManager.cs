using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [Header("Referencias")]
    [SerializeField] CanvasGroup overlay;        // CanvasGroup del BlackOverlay
    [SerializeField] AudioSource audioSource;    // AudioSource para sonido opcional

    [Header("Parámetros")]
    [SerializeField] float defaultDuration = 2f; // duración por defecto
    [SerializeField] AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] float cameraSettleDelay = 0.5f; // tiempo para que la cámara se acomode antes de pausar

    bool isTransitioning;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (overlay)
        {
            overlay.alpha = 0f;
            overlay.blocksRaycasts = false;
        }
    }

    // ========= API PÚBLICA (clásica) =========
    public void FadeAndLoadScene(string sceneName, float duration = -1f, AudioClip optionalClip = null)
    {
        if (isTransitioning) return;
        StartCoroutine(Co_FadeAndLoadScene(sceneName, duration < 0 ? defaultDuration : duration, optionalClip));
    }

    public void FadeAndTeleport(Transform player, Vector3 targetPos, float duration = -1f, AudioClip optionalClip = null)
    {
        if (isTransitioning) return;
        StartCoroutine(Co_FadeAndTeleport(player, targetPos, duration < 0 ? defaultDuration : duration, optionalClip));
    }

    // ========= API PÚBLICA (flujo exacto que pediste) =========
    // Fade-out SIN pausar → ejecutar TP/cámara en negro → esperar cameraSettleDelay → pausar → mini espera → fade-in → reanudar
    public void FadeCustom(float totalDuration, AudioClip clip, Action midAction)
    {
        if (isTransitioning) return;
        StartCoroutine(Co_FadeCustom(Mathf.Max(0.2f, totalDuration), clip, midAction));
    }

    // ========= CORRUTINAS =========
    IEnumerator Co_FadeAndLoadScene(string sceneName, float totalDuration, AudioClip clip)
    {
        isTransitioning = true;
        float half = Mathf.Max(0.01f, totalDuration * 0.5f);

        if (overlay) overlay.blocksRaycasts = true;
        PlayOptional(clip);

        Time.timeScale = 0f;
        yield return Fade(0f, 1f, half);

        var async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone) yield return null;

        yield return new WaitForSecondsRealtime(0.05f);
        yield return Fade(1f, 0f, half);

        Time.timeScale = 1f;

        StopOptional();
        if (overlay) overlay.blocksRaycasts = false;
        isTransitioning = false;
    }

    IEnumerator Co_FadeAndTeleport(Transform player, Vector3 targetPos, float totalDuration, AudioClip clip)
    {
        isTransitioning = true;
        float half = Mathf.Max(0.01f, totalDuration * 0.5f);

        if (overlay) overlay.blocksRaycasts = true;
        PlayOptional(clip);

        Time.timeScale = 0f;
        yield return Fade(0f, 1f, half);

        if (player) player.position = targetPos;

        yield return new WaitForSecondsRealtime(0.05f);
        yield return Fade(1f, 0f, half);

        Time.timeScale = 1f;

        StopOptional();
        if (overlay) overlay.blocksRaycasts = false;
        isTransitioning = false;
    }

    // ======== NUEVA VERSIÓN CON DELAY PARA CÁMARA ========
    IEnumerator Co_FadeCustom(float totalDur, AudioClip clip, System.Action midAction)
    {
        isTransitioning = true;
        float half = Mathf.Max(0.05f, totalDur * 0.5f);

        if (overlay) overlay.blocksRaycasts = true;
        PlayOptional(clip);

        // 0) Congelar de inmediato
        Time.timeScale = 0f;

        // 1) Fade OUT a negro (unscaled)
        yield return Fade(0f, 1f, half);

        // 2) En negro: ejecutar TP + ajustes de cámara (warp/snap/confiner)
        try { midAction?.Invoke(); } catch (System.Exception e) { Debug.LogError(e); }

        // 3) **Descongelar mientras sigue negro** para que la cámara termine de acomodarse
        //    Usa tu campo `cameraSettleDelay` del manager (ajusta 0.2–0.6s según necesites)
        Time.timeScale = 1f;
        if (cameraSettleDelay > 0f)
            yield return new WaitForSecondsRealtime(cameraSettleDelay);

        // 4) Fade IN (sigue negro → aparece)
        yield return Fade(1f, 0f, half);

        // 5) Listo (mantener juego corriendo)
        StopOptional();
        if (overlay) overlay.blocksRaycasts = false;
        isTransitioning = false;
    }



    IEnumerator Fade(float from, float to, float duration)
    {
        if (!overlay) yield break;

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // funciona incluso con timeScale = 0
            float p = Mathf.Clamp01(t / duration);
            float curved = fadeCurve.Evaluate(p);
            overlay.alpha = Mathf.Lerp(from, to, curved);
            yield return null;
        }
        overlay.alpha = to;
    }

    void PlayOptional(AudioClip clip)
    {
        if (!audioSource) return;
        if (clip) audioSource.clip = clip;
        if (audioSource.clip && !audioSource.isPlaying) audioSource.Play();
    }

    void StopOptional()
    {
        if (!audioSource) return;
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = null;
    }
}
