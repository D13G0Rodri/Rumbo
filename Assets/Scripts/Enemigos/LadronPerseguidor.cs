using UnityEngine;

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

    private bool perseguir = false;
    private float walkDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!perseguir || objetivo == null) 
        {
            animator.SetFloat("movementLadron", 0f);
            return;
        }

        float distanciaJugador = Vector2.Distance(transform.position, objetivo.position);
        
        if (distanciaJugador < distanciaDeteccion)
        {
            // Movimiento hacia el jugador
            transform.position = Vector2.MoveTowards(
                transform.position, 
                objetivo.position, 
                velocidad * Time.deltaTime
            );
            
            // Flip para mirar al jugador
            if (objetivo.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        walkDirection = Mathf.Abs(transform.localScale.x);
        animator.SetFloat("movementLadron", walkDirection);
    }

    // Llama a esta función para que el Ladrón empiece a perseguir al Player
    public void EmpezarPersecucion()
    {
        if (panelDialogLadron != null)
            panelDialogLadron.SetActive(false);

        if (objetivo == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) objetivo = p.transform;
        }

        perseguir = objetivo != null;
    }

    // Llama a esta función para detener la persecución
    public void DetenerPersecucion()
    {
        perseguir = false;
        animator.SetFloat("movementLadron", 0f);
    }
}