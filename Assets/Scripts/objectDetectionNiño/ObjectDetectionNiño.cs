
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
    void Start()
    {
        // playerController = GetComponent<PlayerControllerBaby>();


        // randomEvent = FindFirstObjectByType<RandomEvent>();

        panelDialogLadron.SetActive(false);
        panelDialogTv.SetActive(false);

        sceneChanger = FindFirstObjectByType<SceneAutoChanger>();
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
            panelDialogTv.SetActive(false);
        }
    }


    void Update()
    {

        if (isNearToThief == true && Input.GetKey(KeyCode.LeftControl))
        {
            panelDialogLadron.SetActive(true);
        }
        if (isNearToTV == true && Input.GetKey(KeyCode.LeftControl))
        {
            sceneChanger.CambiarEscena("videoGame");
        }
    }

    // Se cambió a OnTriggerEnter2D porque tu juego usa físicas 2D (Rigidbody2D en PlayerController).
    
   
}

