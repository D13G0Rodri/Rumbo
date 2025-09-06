using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ComidaManager : MonoBehaviour
{
    private PlayerControllerBase playerController;
    private Image barraComida;

    public UnityEvent OnBebeHambriento;

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

//ESTE METODO ES IGUAL AL QUE ESTÁ EN CacaManager entonces se usará ese para activar la el onBebeHambriento
    public void BebéEmpiezaALlorar()
    {
        if (playerController != null && playerController.hunger <= 0f)
        {
            OnBebeHambriento?.Invoke();
            Debug.Log("¡El bebé tiene hambre! Se invocó OnBebeHambriento.");
        }
    }

    //ESTE METODO ES INNECESARIO PERO LO DEJO POR SI EN ALGUN MOMENTO HAY QUE USARLO
    // public void BebéEmpiezaALlorar()
    // {
    //     playerController.hunger = 100f;
    // }
}
