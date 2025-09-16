using UnityEngine;

public class GameMemory : MonoBehaviour
{
    public static GameMemory I;

    [Header("Posición a la que queremos volver")]
    public Vector3 savedPosition;
    public bool hasSavedPosition = false;

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject); // NO se destruye al cambiar de escena
    }
}
