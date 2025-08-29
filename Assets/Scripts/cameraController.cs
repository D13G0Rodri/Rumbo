using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float velocidadCamara = 0.025f;
    public Transform transformPlayer;

    public Vector3 desplazamiento;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void LateUpdate(){
        Vector3 posicionObjetivo = transformPlayer.position + desplazamiento;
        Vector3 posicionCamara = Vector3.Lerp(transform.position, posicionObjetivo, velocidadCamara);

        transform.position = posicionCamara;
    }
}


