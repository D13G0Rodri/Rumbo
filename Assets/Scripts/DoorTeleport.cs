using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    public Transform destination; // Asigna el destino en el Inspector
    public KeyCode teleportKey = KeyCode.LeftControl; // Tecla para teletransportar

    private bool playerNearby = false;

    void Update()
    {
        // Si el jugador está cerca y presiona la tecla, teletransportar
        if (playerNearby && Input.GetKeyDown(teleportKey))
        {
            TeleportPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Presiona LeftControl para teletransportar.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            Debug.Log("Jugador se alejó de la puerta.");
        }
    }

    private void TeleportPlayer()
    {
        if (destination != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = destination.position;
                Debug.Log("¡Teletransportado a: " + destination.name);
            }
            else
            {
                Debug.LogError("No se encontró al jugador con el tag 'Player'.");
            }
        }
        else
        {
            Debug.LogError("No se ha asignado un destino.");
        }
    }
}
