using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TimerVida : MonoBehaviour
{
    public float timerCount = 0f;
    [SerializeField] public float maxTime = 1200f;
    private Image barraTiempo;

    public GameObject panelTextToFinish;
    void Start()
    {
        GameObject objBarraTiempo = GameObject.FindWithTag("barraTiempo");
        if (objBarraTiempo)
        {
            barraTiempo = objBarraTiempo.GetComponent<Image>();
        }
        if (panelTextToFinish)
        {
            Debug.Log("Se encontró el objeto");
            panelTextToFinish.SetActive(false);
        }
        
    }


    void Update()
    {

        if (timerCount >= maxTime)
        {
            PlayerControllerBase player = FindFirstObjectByType<PlayerControllerBase>();
            if (player != null)
                player.SaveGame();
            panelTextToFinish.SetActive(true);
        }
        else
        {
            timerCount += Time.deltaTime;
            barraTiempo.fillAmount = timerCount / maxTime;
            // Debug.Log(barraTiempo.fillAmount);
        }
    }
}

