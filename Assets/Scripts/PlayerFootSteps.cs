using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerFootstepsGroundNormal : MonoBehaviour
{
    [Header("Audio")]
    public float stepInterval = 0.4f;
    [Range(0f,1f)] public float volume = 0.4f;

    [Header("Movimiento")]
    public float minSpeedForSteps = 0.1f;        // umbral velocidad horizontal
    public float jumpCutoff = 0.01f;             // si vy > esto: cortar YA (subiendo)
    public float fallCutoff = 0.01f;             // si -vy > esto: considerar caída (no sonar)

    [Header("Suelo (preciso y sin capas)")]
    public float downProbe = 0.02f;              // barrido hacia abajo: salir/entrar sin delay
    public float normalMin = 80f, normalMax = 100f; // contacto “por debajo” (ángulo normal)

    private AudioSource src;
    private Rigidbody2D rb;
    private Collider2D col;
    private float timer;
    private bool wasMoving;
    private ContactFilter2D groundFilter;
    private bool landedThisFrame;

    void Awake()
    {
        src = GetComponent<AudioSource>();
        rb  = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        src.playOnAwake = false;
        src.loop = false;
        src.volume = volume;
        src.spatialBlend = 0f;
        src.dopplerLevel = 0f;

        groundFilter.useTriggers   = false;
        groundFilter.useLayerMask  = false;    // sin capas
        groundFilter.useNormalAngle = true;    // solo contacto desde abajo
        groundFilter.SetNormalAngle(normalMin, normalMax);
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        bool movingH = Mathf.Abs(h) > 0.1f;
        float vx = Mathf.Abs(rb.linearVelocity.x);
        float vy = rb.linearVelocity.y;

        // 1) ¿Hay contacto por debajo (sin delay)?
        //    Si hay cualquier mínima separación, este Cast ya no devuelve hits.
        var hits = new RaycastHit2D[1];
        bool touchingBelow = col.Cast(Vector2.down, hits, downProbe) > 0;

        // 2) ¿Contacto con normal hacia arriba? (evita paredes/techos)
        bool normalOK = col.IsTouching(groundFilter);

        // 3) Corta inmediatamente al despegar o al caer
        bool inJump = vy > jumpCutoff;     // subiendo
        bool inFall = -vy > fallCutoff;    // cayendo

        bool grounded = touchingBelow && normalOK && !inJump && !inFall;

        // ¿podemos sonar?
        bool canStep = movingH && grounded && vx > minSpeedForSteps;

        if (canStep)
        {
            // Si acaba de aterrizar, dispara rápido el primer paso
            if (landedThisFrame) timer = 0f;

            timer -= Time.deltaTime;
            if (!wasMoving) timer = 0f;

            if (timer <= 0f && src.clip != null && !src.isPlaying)
            {
                src.volume = volume;
                src.Play();               // sin OneShot para evitar solapes
                timer = stepInterval;
            }
        }
        else
        {
            if (src.isPlaying) src.Stop();
            timer = 0f;
        }

        wasMoving = canStep;
        landedThisFrame = false; // se resetea al final del frame
    }

    // 3) Detectar aterrizaje en el MISMO frame (normal hacia arriba)
    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var c in collision.contacts)
        {
            if (Vector2.Angle(c.normal, Vector2.up) <= 10f) // ~80–100°
            {
                landedThisFrame = true;
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (col == null) col = GetComponent<Collider2D>();
        Vector3 bottom = col.bounds.center - new Vector3(0, col.bounds.extents.y, 0);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(bottom, bottom + Vector3.down * downProbe);
    }
}
