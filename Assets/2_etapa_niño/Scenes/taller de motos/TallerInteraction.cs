using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(BoxCollider2D))]
public class TallerInteraction : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] string playerTag = "Player";

    [Header("Referencias del jugador (opcionales)")]
    [SerializeField] Transform questionMark;     // Si lo dejas vacío, lo busco en runtime (hijo "QuestionMark" o tag "QuestionMark")
    [SerializeField] Behaviour playerMovement;   // Si lo dejas vacío, lo busco en runtime (cualquier *PlayerController*)

    [Header("Video Overlay")]
    [SerializeField] Canvas videoCanvas;         // Canvas donde está el RawImage (VideoView)
    [SerializeField] VideoPlayer videoPlayer;    // El Video Player (VideoPlayerGO)
    [SerializeField] VideoClip videoClip;        // MP4 a reproducir

    Transform player;
    Vector3 savedPos;
    bool inside;
    bool isPlaying;

    void Start()
    {
        if (videoCanvas) videoCanvas.gameObject.SetActive(false);
        if (questionMark) questionMark.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        inside = true;
        player = other.transform;

        // Guardamos posición exacta
        savedPos = player.position;

        // Autodetectar questionMark si no fue asignado
        if (!questionMark)
        {
            // 1) Por nombre de hijo
            var qm = player.Find("QuestionMark");
            if (qm) questionMark = qm;
            else
            {
                // 2) Por tag opcional
                var go = GameObject.FindWithTag("QuestionMark");
                if (go && go.transform.IsChildOf(player)) questionMark = go.transform;
            }
        }

        if (questionMark)
        {
            // Colócalo sobre la cabeza (ajusta altura si quieres)
            if (questionMark.parent != player) questionMark.SetParent(player);
            questionMark.localPosition = new Vector3(0f, 1.1f, 0f);
            questionMark.gameObject.SetActive(true);
        }

        // Autodetectar el componente que mueve al jugador si no fue asignado
        if (!playerMovement)
            playerMovement = FindPlayerMovementOn(player);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        inside = false;
        if (questionMark) questionMark.gameObject.SetActive(false);
        player = null;
    }

    void Update()
    {
        if (inside && !isPlaying && Input.GetKeyDown(KeyCode.E))
            StartCoroutine(PlayVideoAndReturn());
    }

    System.Collections.IEnumerator PlayVideoAndReturn()
    {
        isPlaying = true;
        inside = false; // evita reentradas

        if (questionMark) questionMark.gameObject.SetActive(false);
        if (player) savedPos = player.position; // por si se movió un pixel

        // Desactiva el movimiento del jugador
        if (playerMovement) playerMovement.enabled = false;

        // Pausar mundo (el VideoPlayer ignora timeScale)
        float prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        // Preparar y reproducir
        if (videoCanvas) videoCanvas.gameObject.SetActive(true);

        if (videoPlayer && videoClip)
        {
            videoPlayer.clip = videoClip;
            videoPlayer.isLooping = false;

            bool finished = false;
            // Limpia subscripciones previas para evitar duplicados
            videoPlayer.loopPointReached -= OnVideoFinished;
            videoPlayer.loopPointReached += OnVideoFinished;

            void OnVideoFinished(VideoPlayer vp) { finished = true; }

            videoPlayer.Play();

            while (!finished) yield return null;

            // Por limpieza
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
        else
        {
            // Si falta algo, espera un frame y continúa, así no se bloquea el juego
            Debug.LogWarning("[TallerInteraction] Falta asignar VideoPlayer o VideoClip.");
            yield return null;
        }

        // Ocultar overlay y reanudar
        if (videoCanvas) videoCanvas.gameObject.SetActive(false);
        Time.timeScale = prevTimeScale;

        // Regresar EXACTO al mismo lugar
        if (player) player.position = savedPos;

        // Reactivar control
        if (playerMovement) playerMovement.enabled = true;

        isPlaying = false;
    }

    // Busca un componente que probablemente sea el controlador del jugador
    Behaviour FindPlayerMovementOn(Transform root)
    {
        if (!root) return null;

        // 1) Busca por tipos conocidos (si existen en tu proyecto)
        var known = root.GetComponentsInChildren<Behaviour>(true);
        foreach (var b in known)
        {
            var n = b.GetType().Name;
            // Ajusta/añade aquí si tu controlador tiene otro nombre
            if (n.Contains("PlayerController"))
                return b;
        }

        // 2) Si no encontró nada, devuelve null para no romper
        return null;
    }
}
