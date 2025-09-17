using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class LadronPerseguidor : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Se asigna automáticamente por Tag 'Player' si se deja vacío al iniciar la persecución.")]
    public Transform objetivo;

    private Rigidbody2D rb;
    private Animator animator;

    [Header("Movimiento")]
    [SerializeField] private float velocidad = 3.5f;
    [SerializeField] private float distanciaDeteccion = 10f;

    [Header("UI")]
    public GameObject panelDialogLadron;
    [SerializeField] private Button btnAyudar;
    [SerializeField] private Button btnIgnorar;
    [SerializeField] private StatChangeToast toastUI;

    [Header("Respawn")]
    [Tooltip("Tiempo en segundos para que reaparezca el vagabundo después de desaparecer")]
    [SerializeField] private float respawnDelay = 120f;

    // --- control interno ---
    private bool perseguir = false;
    private float walkDirection;
    private bool isHidden = false;
    private SpriteRenderer[] renderers;
    private Collider2D[] colliders;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // cachea todo lo visible/colisionable (incluye hijos)
        renderers = GetComponentsInChildren<SpriteRenderer>(true);
        colliders = GetComponentsInChildren<Collider2D>(true);

        if (btnAyudar)  btnAyudar.onClick.AddListener(OnAyudar);
        if (btnIgnorar) btnIgnorar.onClick.AddListener(OnIgnorar);

        if (panelDialogLadron) panelDialogLadron.SetActive(false);
    }

    void Update()
    {
        if (isHidden) return; // mientras está “despawneado”, no hace nada

        if (!perseguir || objetivo == null)
        {
            animator.SetFloat("movementLadron", 0f);
            return;
        }

        float distanciaJugador = Vector2.Distance(transform.position, objetivo.position);

        if (distanciaJugador < distanciaDeteccion)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                objetivo.position,
                velocidad * Time.deltaTime
            );

            // Flip para mirar al jugador
            if (objetivo.position.x < transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }

        walkDirection = Mathf.Abs(transform.localScale.x);
        animator.SetFloat("movementLadron", walkDirection);
    }

    // === BOTONES ===
    void OnAyudar()
    {
        ApplyAndBufferDeltas(0.1f, 0.1f, +1);

        if (panelDialogLadron) panelDialogLadron.SetActive(false);
        if (toastUI) toastUI.ShowNowFromBuffer();

        // Desaparece y reaparece luego
        StartCoroutine(RespawnRoutine());
    }

    void OnIgnorar()
    {
        ApplyAndBufferDeltas(0f, 0f, -1);

        if (panelDialogLadron) panelDialogLadron.SetActive(false);
        if (toastUI) toastUI.ShowNowFromBuffer();

        // También desaparece (evitamos la persecución para simplificar)
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        HideSelf();                            // “despawn suave”
        yield return new WaitForSeconds(respawnDelay);
        ShowSelf();                            // reaparecer
    }

    void HideSelf()
    {
        if (isHidden) return;
        isHidden = true;
        perseguir = false;

        // apaga render, colisiones y físicas
        foreach (var r in renderers) if (r) r.enabled = false;
        foreach (var c in colliders) if (c) c.enabled = false;

        if (rb) rb.simulated = false;
        if (animator) animator.enabled = false;

        // por si acaso
        if (panelDialogLadron) panelDialogLadron.SetActive(false);
    }

    void ShowSelf()
    {
        if (!isHidden) return;
        isHidden = false;

        // enciende render, colisiones y físicas
        foreach (var r in renderers) if (r) r.enabled = true;
        foreach (var c in colliders) if (c) c.enabled = true;

        if (rb) rb.simulated = true;
        if (animator) { animator.enabled = true; animator.SetFloat("movementLadron", 0f); }

        // no perseguir de inmediato
        perseguir = false;
    }

    // === APLICAR CAMBIOS EN STATS Y GUARDAR ===
    void ApplyAndBufferDeltas(float dIntelligence, float dHappiness, int dKarma)
    {
        var data = SaveSystem.LoadPlayerData() ?? new PlayerData();

        data.intelligence = Mathf.Clamp(data.intelligence + dIntelligence, 0f, 100f);
        data.happiness    = Mathf.Clamp(data.happiness    + dHappiness,    0f, 100f);
        data.karma        = Mathf.Clamp(data.karma        + dKarma,        0f, 100f);

        SaveSystem.SavePlayerData(data);

        PlayerPrefs.SetFloat("delta_intelligence", dIntelligence);
        PlayerPrefs.SetFloat("delta_happiness",    dHappiness);
        PlayerPrefs.SetFloat("delta_karma",        dKarma);
        PlayerPrefs.SetInt("stat_toast_pending",   1);
        PlayerPrefs.Save();
    }

    // === (mantengo por si luego vuelves a usar persecución) ===
    public void EmpezarPersecucion()
    {
        if (panelDialogLadron != null)
            panelDialogLadron.SetActive(false);

        if (objetivo == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) objetivo = p.transform;
        }

        perseguir = objetivo != null && !isHidden;
    }

    public void DetenerPersecucion()
    {
        perseguir = false;
        if (animator) animator.SetFloat("movementLadron", 0f);
    }
}
