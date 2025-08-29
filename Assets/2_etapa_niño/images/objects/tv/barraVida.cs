using UnityEngine;
using UnityEngine.UI;

public class barraVida : MonoBehaviour
{
    public Image ImgBarraVida;
    private PlayerController player;
    private float maxHealth;

    void Start()
    {
        player = GameObject.Find("Player")?.GetComponent<PlayerController>();
        if (player != null)
        {
            maxHealth = player.health;
        }
        else
        {
            Debug.LogError("PlayerController not found on Player GameObject");
        }

        if (ImgBarraVida == null)
        {
            Debug.LogError("ImgBarraVida is not assigned in the inspector");
        }
    }

    void Update()
    {
        if (player != null && ImgBarraVida != null)
        {
            ImgBarraVida.fillAmount = player.health / maxHealth; 
            //El maximo de fillAmount es 1 entonces la vida maxima que es 5 seria igual a 1 y si divides 3 vidas(fictisias) entre 5
            //te dará 0.6 por lo tanto el fill amount no estaría en 1 sino en 0.6 simulando que tiene 3 vidas.
        }
    }
}