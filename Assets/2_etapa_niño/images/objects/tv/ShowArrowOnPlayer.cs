using UnityEngine;

public class ShowArrowOnPlayer : MonoBehaviour
{
    [Header("Referencia a la flecha")]
    public GameObject arrow;   // arrastra aquí el hijo "Arrow"

    bool playerInside = false;

    void Start()
    {
        if (arrow != null) arrow.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            if (arrow != null) arrow.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (arrow != null) arrow.SetActive(false);
        }
    }
}
