using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    public float distanceDetection = 2.0f;
    float walkDirectionEnemy;
    public float damage = 1f;
    public float speedEnemy = 2f;
    public Transform player;

    public Animator animator;    

    // Update is called once per frame
    void Update(){
        float distancePlayer = Vector2.Distance(transform.position, player.position);
        if (player != null)
        {
            if(distancePlayer < distanceDetection){
                transform.position = Vector2.MoveTowards(transform.position, player.position, speedEnemy * Time.deltaTime);
                
                // Flip the enemy to face the player
                if (player.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
        walkDirectionEnemy = transform.localScale.x;

        animator.SetFloat("movementEnemy", walkDirectionEnemy);
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el objeto con el que colisionamos tiene la etiqueta "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            
            animator.SetBool("damageEnemy", true);
            // Intenta obtener el componente PlayerController del objeto con el que colisionamos
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            
            // Si el componente PlayerController fue encontrado
            if (player != null)
            {
                // Obtiene el punto de contacto de la colisión
                Vector2 contactPoint = collision.contacts[0].point;
                
                // Llama al método ReceiveDamage del jugador, pasando el punto de contacto y el daño
                player.ReceiveDamage(contactPoint, damage);
            }
        }
    }
    void DontAtack(){
        animator.SetBool("damageEnemy", false);
    }
}
        
    // Este método se llama cuando el colisionador de este objeto
    // entra en contacto con otro colisionador 2D
    