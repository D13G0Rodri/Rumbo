using UnityEngine;
using UnityEngine.UI;

public class ComidaManager : MonoBehaviour
{
    private PlayerControllerBase playerController;
    private Image barraComida;

    void Start()
    {
        playerController = GetComponent<PlayerControllerBase>();

        if (barraComida == null)
        {
            GameObject objBarraComida = GameObject.FindWithTag("barraComida");
            if (objBarraComida != null)
                barraComida = objBarraComida.GetComponent<Image>();
        }
    }

    void Update()
    {
        if (playerController == null) return;

        // Barra de comida: directo (100 = full, 0 = hambriento)
        if (barraComida != null)
        {
            barraComida.fillAmount = Mathf.Clamp01(playerController.hunger / 100f);
        }
    }

    public void AlimentarBebe()
    {
        if (playerController != null)
        {
            playerController.hunger = 100f;
            Debug.Log("El bebé ha sido alimentado");
        }
    }
}
