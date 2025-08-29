using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isReceivingDamage;
    public GameObject gameOverImage;
    public float speed = 5f;
    public float jumpForce = 3f;
    public float health = 6;
    public bool isGrounded;
    public Animator animator;
    public float damageCooldown = 1f; // Tiempo de espera antes de poder recibir daño nuevamente
    Vector2 PosicionInicio;
    private float VidaMax; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        VidaMax = health;
    }

    void Update()
    {
        float movimiento = Input.GetAxisRaw("Horizontal");

        if (movimiento > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movimiento < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        animator.SetFloat("movement", movimiento);
        rb.linearVelocity = new Vector2(movimiento * speed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        animator.SetBool("recibeDanio", isReceivingDamage);

    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector2(rb.linearVelocity.x, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("isGrounded", true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Piso"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Piso"))
        {
            isGrounded = false;
        }
    }

    public void ReceiveDamage(Vector2 direction, float damage)
    {
        if (!isReceivingDamage)
        {
            isReceivingDamage = true;
            health -= damage; // Reduce la salud del jugador y esta variable es el dividendo de la vida maxima que seria la vida maxima.
            Vector2 rebote = new Vector2(direction.x, 1).normalized;
            rb.AddForce(rebote * 5, ForceMode2D.Impulse);

            if (health <= 0)
            {
                // Manejar la muerte del jugador
                dead();
            }

            // Restablecer el estado de recibir daño después de un tiempo
            Invoke(nameof(DeactivateDamage), damageCooldown);
        }
    }

    public void dead()
        {
            Debug.Log("Player has died");
            animator.SetBool("isDead", true);
        }

    public void DeactivateDamage()
    {
        isReceivingDamage = false;
        
    }

    public void Respawn(){
        rb.position = PosicionInicio;
        animator.SetBool("isDead", false);
        health = VidaMax;
        
    }
}