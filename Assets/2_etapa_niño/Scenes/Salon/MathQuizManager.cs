using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class MathQuestion
{
    [TextArea] public string texto;
    public string respuesta;
}

public class MathQuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TMP_InputField answerField;
    public TextMeshProUGUI feedbackText;
    public string classroomSceneName = "Etapa-Niño"; // pon aquí el nombre exacto de tu salón

    public MathQuestion[] preguntas;

    int index = 0;

    void Start()
    {
        feedbackText.text = "";
        index = 0;
        MostrarPregunta();
    }

    void MostrarPregunta()
    {
        if (index >= preguntas.Length)
        {
            Finalizar();
            return;
        }

        questionText.text = preguntas[index].texto;
        answerField.text = "";
        answerField.ActivateInputField();
    }

    public void Enviar()
    {
        string given = answerField.text.Trim().Replace(",", ".");
        string expected = preguntas[index].respuesta.Trim().Replace(",", ".");

        if (given.Equals(expected, System.StringComparison.InvariantCultureIgnoreCase))
        {
            index++;
            feedbackText.text = "";
            MostrarPregunta();
        }
        else
        {
            feedbackText.text = "Intenta otra vez";
        }
    }

    void Finalizar()
    {
        questionText.text = "¡Muy bien!";
        questionText.color = Color.white; // efecto tiza
        feedbackText.text = "";
        Invoke(nameof(VolverAlSalon), 2f);
    }

    void VolverAlSalon()
    {
        SceneManager.LoadScene(classroomSceneName);
    }
}
