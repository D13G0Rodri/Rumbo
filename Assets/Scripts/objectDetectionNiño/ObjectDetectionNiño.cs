
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectDetectionNiño : MonoBehaviour
{

    // public Animator animator;
    // public RandomEvent randomEvent;// Asigna esto desde el Inspector de Unity
    // public PlayerControllerBaby playerController;
    public SceneAutoChanger sceneChanger;

    private bool isNearToThief = false;
    private bool isNearToTV = false;

    //Escena niño

    public GameObject panelDialogLadron;
    public GameObject panelDialogTv;
    public KeyCode interactionKey = KeyCode.E;
    void Start()
    {
        // playerController = GetComponent<PlayerControllerBaby>();


        // randomEvent = FindFirstObjectByType<RandomEvent>();

        if (panelDialogLadron != null)
            panelDialogLadron.SetActive(false);
        else
            Debug.LogWarning("Panel de diálogo del ladrón no asignado.");

        if (panelDialogTv != null)
            panelDialogTv.SetActive(false);
        else
            Debug.LogWarning("Panel de diálogo de la TV no asignado.");

        sceneChanger = FindFirstObjectByType<SceneAutoChanger>();
        if (sceneChanger == null)
            Debug.LogWarning("SceneAutoChanger no encontrado en la escena.");
    }

    void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.CompareTag("Ladron"))
        {
            isNearToThief = true;
        }
        if (Collision.CompareTag("Obj_TV"))
        {
            isNearToTV = true;
            if (panelDialogTv != null)
                panelDialogTv.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D Collision)
    {
        if (Collision.CompareTag("Ladron"))
        {
            isNearToThief = false;
        }
        if (Collision.CompareTag("Obj_TV"))
        {
            isNearToTV = false;
            if (panelDialogTv != null)
                panelDialogTv.SetActive(false);
        }
    }

    void Update()
    {
        if (isNearToThief && Input.GetKey(interactionKey))
        {
            if (panelDialogLadron != null)
                panelDialogLadron.SetActive(true);
        }

        if (isNearToTV && Input.GetKey(interactionKey))
        {
            if (sceneChanger != null)
                sceneChanger.CambiarEscena("videoGame");
            else
                Debug.LogError("SceneAutoChanger no está disponible para cambiar de escena.");
        }
    }

    // Se cambió a OnTriggerEnter2D porque tu juego usa físicas 2D (Rigidbody2D en PlayerController).
    
   
}

