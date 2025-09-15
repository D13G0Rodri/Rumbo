using UnityEngine;
using UnityEngine.SceneManagement;

public class FoodChoiceController : MonoBehaviour
{
    public void ChooseBanana()
    {
        Debug.Log("Banana!");
        UnityEngine.SceneManagement.SceneManager.LoadScene("BuenaDecision");
    }
    public void ChooseChips()
    {
        Debug.Log("Papas!");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MalaDecision");
    }


}
