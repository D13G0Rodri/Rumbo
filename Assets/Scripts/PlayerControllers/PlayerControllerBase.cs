using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControllerBase : MonoBehaviour
{
    // --- Componentes y Referencias ---
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Image barraVidaImage;

    // --- Configuración ---
    [Header("Configuración Básica")]
    public float speed = 5f;
    public float jumpForce = 3f;
    public float VidaMax = 6f;
    public float damageCooldown = 1f;
    public GameObject gameOverImage;

    // --- Estado del Jugador ---
    [Header("Estados")]
    public float health;
    public float intelligence = 50f;
    public float concentration = 50f;
    public float hunger = 100f;
    public float bathroom = 0f;
    public float timerCount;
    public bool presentationPanelShown;

    protected bool isReceivingDamage;
    protected bool isGrounded;

    public float velocidadCargaCaca = 5f;

    public static event Action<PlayerData> OnGameDataLoaded;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameObject barraVidaObj = GameObject.FindWithTag("barraVida");
        if (barraVidaObj != null)
            barraVidaImage = barraVidaObj.GetComponent<Image>();
    }

    protected virtual void Start()
    {
        PlayerData loaded = LoadGame();
        if (loaded == null)
        {
            health = VidaMax;
            hunger = 100f;
            bathroom = 0f;
            timerCount = 0f;
            presentationPanelShown = false;
        }
        else
        {
            if (health <= 0) health = VidaMax;
        }
        UpdateHealthUI();
        isGrounded = true;
        Debug.Log($"Datos iniciales: Tiempo={timerCount} Vida={health} Hunger={hunger} Bathroom={bathroom}");
    }

    protected virtual void Update()
    {
        HandleMovement();
        HandleJump();
        UpdateHunger();
        UpdateBathroom();
    }

    protected virtual void HandleMovement()
    {
        float movimiento = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(movimiento * speed, rb.linearVelocity.y);
        animator.SetFloat("movement", Mathf.Abs(movimiento));
        if (movimiento > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (movimiento < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    protected virtual void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }

    public virtual void ReceiveDamage(float damage)
    {
        if (!isReceivingDamage)
        {
            isReceivingDamage = true;
            health -= damage;
            UpdateHealthUI();
            if (health <= 0)
                animator.SetBool("isDead", true);
            Invoke(nameof(ResetDamage), damageCooldown);
        }
    }

    public void UpdateHealthUI()
    {
        if (barraVidaImage != null && VidaMax > 0)
        {
            barraVidaImage.fillAmount = Mathf.Clamp01(health / VidaMax);
        }
    }

    protected virtual void Dead()
    {
        if (health <= 0)
        {
            animator.SetBool("isDead", true);
            Debug.Log("¡El personaje ha muerto!");
            SceneManager.LoadScene("Muerte");
        }
    }

    protected virtual void ResetDamage()
    {
        isReceivingDamage = false;
        animator.SetBool("isElectrocuted", false);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Piso"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Piso"))
            isGrounded = false;
    }

    // --- SISTEMA DE GUARDADO Y CARGA (REFACTORIZADO) ---

    protected virtual void AddStageSpecificData(PlayerData data)
    {
        // Las clases hijas sobrescribirán este método
    }

    // --- NUEVO: Método público para exponer AddStageSpecificData ---
    public void ApplyStageSpecificData(PlayerData data)
    {
        AddStageSpecificData(data);
    }

    public void SaveGame()
    {
        PlayerData data = CreatePlayerData();
        SaveSystem.SavePlayerData(data);
        Debug.Log("SaveGame() para transición de etapa ejecutado.");
    }

    public void SaveCheckpoint(bool orderingAchievement = false)
    {
        PlayerData data = SaveSystem.LoadPlayerData() ?? new PlayerData();

        data.health = this.health;
        data.position = new float[] { transform.position.x, transform.position.y, transform.position.z };
        data.intelligence = this.intelligence;
        data.concentration = this.concentration;
        data.hunger = this.hunger;
        data.bathroom = this.bathroom;
        data.currentSceneName = SceneManager.GetActiveScene().name;
        data.timerCount = (FindFirstObjectByType<TimerVida>() != null) ? FindFirstObjectByType<TimerVida>().timerCount : this.timerCount;
        data.hasOrderingAchievement = orderingAchievement || data.hasOrderingAchievement;
        data.presentationPanelShown = this.presentationPanelShown;

        if (data.pushableObjectPositions == null)
            data.pushableObjectPositions = new SerializableDictionary<string, SerializableVector3>();

        PushableObject[] pushables = FindObjectsByType<PushableObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var obj in pushables)
        {
            if (!string.IsNullOrEmpty(obj.objectId))
            {
                data.pushableObjectPositions[obj.objectId] = new SerializableVector3(obj.transform.position);
            }
        }

        AddStageSpecificData(data);
        SaveSystem.SavePlayerData(data);
        Debug.Log("SaveCheckpoint() ejecutado. Logro de ordenar: " + data.hasOrderingAchievement);
    }

    protected virtual PlayerData CreatePlayerData()
    {
        PlayerData data = new PlayerData
        {
            health = this.health,
            position = new float[] { transform.position.x, transform.position.y, transform.position.z },
            intelligence = this.intelligence,
            concentration = this.concentration,
            hunger = this.hunger,
            bathroom = this.bathroom,
            currentSceneName = SceneManager.GetActiveScene().name,
            timerCount = (FindFirstObjectByType<TimerVida>() != null) ? FindFirstObjectByType<TimerVida>().timerCount : this.timerCount,
            presentationPanelShown = this.presentationPanelShown
        };

        AddStageSpecificData(data);
        return data;
    }

    public virtual PlayerData LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayerData();
        if (data != null)
        {
            health = data.health;
            if (data.position != null && data.position.Length >= 3)
                transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            else
                Debug.LogWarning("Datos de posición inválidos o nulos en el archivo de guardado.");

            intelligence = data.intelligence;
            concentration = data.concentration;
            hunger = data.hunger;
            bathroom = data.bathroom;
            timerCount = data.timerCount;
            TimerVida timer = FindFirstObjectByType<TimerVida>();
            if (timer != null)
                timer.timerCount = data.timerCount;

            presentationPanelShown = data.presentationPanelShown;

            AddStageSpecificData(data);

            if (data.pushableObjectPositions != null)
            {
                PushableObject[] pushables = FindObjectsByType<PushableObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (var obj in pushables)
                {
                    if (obj == null || string.IsNullOrEmpty(obj.objectId))
                        continue;

                    if (data.pushableObjectPositions.TryGetValue(obj.objectId, out SerializableVector3 pos))
                        obj.transform.position = pos.ToVector3();
                }
            }

            OnGameDataLoaded?.Invoke(data);
            Debug.Log("LoadGame() cargó datos.");
            return data;
        }
        return null;
    }

    void UpdateHunger()
    {
        hunger -= Time.deltaTime * 5f;
        if (hunger < 0) hunger = 0f;
    }

    void UpdateBathroom()
    {
        bathroom += Time.deltaTime * velocidadCargaCaca;
        if (bathroom > 100f) bathroom = 100f;
    }
}
