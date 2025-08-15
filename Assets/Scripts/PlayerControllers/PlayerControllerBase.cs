using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControllerBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Image barraVidaImage;
    protected float VidaMax;
    protected bool isReceivingDamage;
    protected bool isGrounded;

    [Header("Configuración Básica")]
    public float speed = 5f;
    public float jumpForce = 3f;
    public float health = 6f;
    public float damageCooldown = 1f;
    public GameObject gameOverImage;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        VidaMax = health;

        GameObject barraVidaObj = GameObject.FindWithTag("barraVida");
        if (barraVidaObj != null)
            barraVidaImage = barraVidaObj.GetComponent<Image>();

        isGrounded = true;
    }

    protected virtual void Update()
    {
        HandleMovement();
        HandleJump();
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
        if (!isReceivingDamage && barraVidaImage != null)
        {
            isReceivingDamage = true;
            health -= damage;
            barraVidaImage.fillAmount = health / VidaMax;

            if (health <= 0)
                Dead();

            Invoke(nameof(ResetDamage), damageCooldown);
        }
    }

    protected virtual void Dead()
    {
        animator.SetBool("isDead", true);
        Debug.Log("¡El personaje ha muerto!");
        SceneManager.LoadScene("Muerte");
    }

    protected virtual void ResetDamage()
    {
        isReceivingDamage = false;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Piso"))
            isGrounded = true;
            animator.SetBool("isGrounded", true);
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Piso"))
            isGrounded = false;
    }
}
