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
    public float hunger = 100f;   // empieza en 100
    public float bathroom = 0f;   // empieza en 0 (nivel caca)
    public float timerCount;

    protected bool isReceivingDamage;
    protected bool isGrounded;

    public float velocidadCargaCaca = 5f;

    // Nuevo: evento para notificar cuando los datos han sido cargados
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
        // Intentar cargar partida
        PlayerData loaded = LoadGame();

        if (loaded == null)
        {
            // no había datos guardados: valores por defecto
            health = VidaMax;
            hunger = 100f;
            bathroom = 0f;
            timerCount = 0f;
        }
        else
        {
            // health ya fue asignada en LoadGame; si por alguna razón es <=0, asignamos VidaMax
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

    // --- Guardado / Carga ---

    protected virtual PlayerData CreatePlayerData()
    {
        TimerVida timerVida = FindFirstObjectByType<TimerVida>();
        return new PlayerData
        {
            health = this.health,
            position = new float[] { transform.position.x, transform.position.y, transform.position.z },
            intelligence = this.intelligence,
            concentration = this.concentration,
            hunger = this.hunger,
            bathroom = this.bathroom,
            currentSceneName = SceneManager.GetActiveScene().name,
            timerCount = (timerVida != null) ? timerVida.timerCount : this.timerCount
        };
    }

    public void SaveGame()
    {
        PlayerData data = CreatePlayerData();
        SaveSystem.SavePlayerData(data);
        Debug.Log("SaveGame() ejecutado.");
    }

    public virtual PlayerData LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayerData();
        if (data != null)
        {
            health = data.health;
            transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            intelligence = data.intelligence;
            concentration = data.concentration;
            hunger = data.hunger;
            bathroom = data.bathroom;
            timerCount = data.timerCount;

            OnGameDataLoaded?.Invoke(data);
            Debug.Log("LoadGame() cargó datos.");
            return data;
        }
        return null;
    }

    void UpdateHunger()
    {
        hunger -= Time.deltaTime * 0.1f; // ajustable
        if (hunger < 0) hunger = 0f;
    }

    void UpdateBathroom()
    {
        // aumentamos nivel de caca con el tiempo (0 -> 100)
        bathroom += Time.deltaTime * velocidadCargaCaca; // ajusta velocidad en inspector si quieres
        if (bathroom > 100f) bathroom = 100f;
    }
}
