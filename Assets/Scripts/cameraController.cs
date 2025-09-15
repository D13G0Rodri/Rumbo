using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float velocidadCamara = 0.025f;
    public Transform transformPlayer;
    public Vector3 desplazamiento;

    // Límites de la cámara
    public Vector2 minPosicion;  // Esquina inferior izquierda
    public Vector2 maxPosicion;  // Esquina superior derecha

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
