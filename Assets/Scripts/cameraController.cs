using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float velocidadCamara = 0.025f;
    public Transform transformPlayer;
    public Vector3 desplazamiento;

    // Límites de la cámara
    public Vector2 minPosicion;  // Esquina inferior izquierda
    public Vector2 maxPosicion;  // Esquina superior derecha

    void Awake()
    {
        // Comprueba si hay un componente CinemachineBrain en este GameObject, usando una búsqueda por nombre para evitar errores de compilación.
        var cinemachineBrain = GetComponent("Cinemachine.CinemachineBrain");
        if (cinemachineBrain != null)
        {
            // Si se encuentra, desactiva este script y muestra una advertencia.
            this.enabled = false;
            Debug.LogWarning("CameraController: Se ha encontrado un componente CinemachineBrain. El script CameraController se ha desactivado para evitar conflictos. Utiliza Cinemachine para controlar la cámara o desactiva/elimina el componente CinemachineBrain.");
        }
    }

    void LateUpdate()
    {
        // Calculamos la posición deseada
        Vector3 posicionObjetivo = transformPlayer.position + desplazamiento;

        // Aplicamos suavizado
        Vector3 posicionCamara = Vector3.Lerp(transform.position, posicionObjetivo, velocidadCamara);

        // Limitamos la posición X e Y para que la cámara no se salga del mapa
        posicionCamara.x = Mathf.Clamp(posicionCamara.x, minPosicion.x, maxPosicion.x);
        posicionCamara.y = Mathf.Clamp(posicionCamara.y, minPosicion.y, maxPosicion.y);

        // Actualizamos la posición final de la cámara
        transform.position = posicionCamara;
    }
}
