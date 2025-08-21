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
    public float VidaMax = 6f; // CAMBIO: VidaMax es ahora un valor fijo, no depende del 'health' inicial.
    public float damageCooldown = 1f;
    public GameObject gameOverImage;

    // --- Estado del Jugador ---
    [Header("Estados")]
    public float health; // CAMBIO: Separado de VidaMax para claridad.
    public float intelligence = 50f;
    public float concentration = 50f;
    public float hunger = 100f;
    public float bathroom = 100f;
    public float timerCount; // CAMBIO: El jugador es el "dueño" de este dato.

    protected bool isReceivingDamage;
    protected bool isGrounded;
    
    // CAMBIO: Usamos Awake para obtener componentes. Es lo primero que se ejecuta.
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
        // CAMBIO: La carga de datos ahora es lo primero que hacemos.
        LoadGame();
        
        // CAMBIO: La vida inicial se establece aquí. Si no hay datos guardados, será VidaMax.
        health = (health > 0) ? health : VidaMax;
        UpdateHealthUI(); // Actualizamos la UI después de establecer la vida.

        isGrounded = true;

        Debug.Log($"Datos cargados: \n Tiempo de vida={timerCount} \nVida: {health}");
    }

    protected virtual void Update()
    {
        HandleMovement();
        HandleJump();
    }

    // --- Movimiento y Acciones ---
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

    // --- Lógica de Vida y Daño ---
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
    
    // CAMBIO: Función separada para actualizar la UI, para no repetir código.
    public void UpdateHealthUI()
    {
        if (barraVidaImage != null)
        {
            barraVidaImage.fillAmount = health / VidaMax;
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

    // --- Colisiones ---
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

    // --- Sistema de Guardado y Carga ---

    // Nuevo: evento para notificar cuando los datos han sido cargados
    public static event Action<PlayerData> OnGameDataLoaded;
    
    // CAMBIO: Se crea un método virtual para que las clases hijas lo extiendan.
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
            // CAMBIO: Obtenemos el tiempo directamente del timer para asegurar que es el actual.
            timerCount = (timerVida != null) ? timerVida.timerCount : this.timerCount 
        };
    }

    public void SaveGame()
    {
        PlayerData data = CreatePlayerData();
        SaveSystem.SavePlayerData(data);
    }

    // CAMBIO: Ahora LoadGame devuelve un objeto PlayerData.
    // También es virtual para poder ser sobrescrito.
    public virtual PlayerData LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayerData();
        if (data != null)
        {
            // Aplicamos los datos comunes del PlayerControllerBase
            health = data.health;
            transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            intelligence = data.intelligence;
            concentration = data.concentration;
            hunger = data.hunger;
            bathroom = data.bathroom;
            timerCount = data.timerCount;

            // Notificamos a quien esté interesado (por ejemplo, TimerVida)
            OnGameDataLoaded?.Invoke(data);

            // Retornamos el objeto data para que las clases derivadas puedan usarlo
            return data;
        }
        // Si no hay datos guardados, o si el archivo está vacío, retornamos null
        return null; 
    }
}