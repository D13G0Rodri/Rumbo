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

    /// <summary>
    /// NUEVO: Método virtual para que las clases hijas añadan sus datos específicos.
    /// En la clase base, este método no hace nada.
    /// </summary>
    protected virtual void AddStageSpecificData(PlayerData data)
    {
        // Las clases hijas (Child, Teen) sobrescribirán este método.
    }

    /// <summary>
    /// MÉTODO PRINCIPAL PARA GUARDADO DE TRANSICIÓN DE ETAPA.
    /// Llama a este método cuando quieras pasar a la siguiente etapa.
    /// </summary>
    public void SaveGame()
    {
        PlayerData data = CreatePlayerData();
        SaveSystem.SavePlayerData(data);
        Debug.Log("SaveGame() para transición de etapa ejecutado.");
    }

    /// <summary>
    /// MÉTODO PRINCIPAL PARA GUARDADO DE CHECKPOINT.
    /// Llama a este método cuando quieras guardar el progreso DENTRO de una etapa.
    /// </summary>
    public void SaveCheckpoint()
    {
        // 1. Crea el contenedor de datos base (con el nombre de la escena actual).
        PlayerData data = new PlayerData
        {
            health = this.health,
            position = new float[] { transform.position.x, transform.position.y, transform.position.z },
            intelligence = this.intelligence,
            concentration = this.concentration,
            hunger = this.hunger,
            bathroom = this.bathroom,
            currentSceneName = SceneManager.GetActiveScene().name,
            timerCount = (FindFirstObjectByType<TimerVida>() != null) ? FindFirstObjectByType<TimerVida>().timerCount : this.timerCount
        };

        // 2. Pide a la clase hija que añada sus datos específicos (energía, felicidad, etc.).
        AddStageSpecificData(data);

        // 3. Guarda el archivo.
        SaveSystem.SavePlayerData(data);
        Debug.Log("SaveCheckpoint() ejecutado para la escena: " + data.currentSceneName);
    }

    /// <summary>
    /// Crea el contenedor de datos para una TRANSICIÓN.
    /// Las clases hijas lo sobrescriben para definir la PRÓXIMA escena.
    /// </summary>
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
            timerCount = (FindFirstObjectByType<TimerVida>() != null) ? FindFirstObjectByType<TimerVida>().timerCount : this.timerCount
        };
        
        // Pide a la clase hija que añada sus datos.
        AddStageSpecificData(data);

        return data;
    }

    /// <summary>
    /// Carga los datos del archivo y los aplica al estado del jugador.
    /// </summary>
    public virtual PlayerData LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayerData();
        if (data != null)
        {
            // Carga los datos base
            health = data.health;
            transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            intelligence = data.intelligence;
            concentration = data.concentration;
            hunger = data.hunger;
            bathroom = data.bathroom;
            timerCount = data.timerCount;

            // Pide a la clase hija que cargue sus datos específicos
            AddStageSpecificData(data);

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