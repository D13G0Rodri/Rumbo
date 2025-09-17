using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Globalization;
using System.Text;

public class CultureQuizManager : MonoBehaviour
{
    [Header("Entradas")]
    public TMP_InputField banderaField;
    public TMP_InputField eiffelField;
    public TMP_InputField huesosField;

    [Header("UI")]
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI titleText; 
    public string classroomSceneName = "Etapa-Niño";

    [Header("Respuestas esperadas (puedes cambiarlas)")]
    public string banderaEsperada = "colombia";
    public string eiffelEsperada = "francia";
    public string huesosEsperada = "206";

    public void VerRespuestas()
    {
        // Si prefieres solo comprobar que NO estén vacías, usa:
        // if (banderaField.text != "" && eiffelField.text != "" && huesosField.text != "") { MostrarMuyBien(); return; }

        bool ok1 = Normalizar(banderaField.text) == Normalizar(banderaEsperada);
        bool ok2 = Normalizar(eiffelField.text) == Normalizar(eiffelEsperada) || Normalizar(eiffelField.text) == "france";
        bool ok3 = Normalizar(huesosField.text) == Normalizar(huesosEsperada);

        if (ok1 && ok2 && ok3)
        {
            MostrarMuyBien();
        }
        else
        {
            feedbackText.text = "NO!";
        }
    }

    string Normalizar(string s)
    {
        s = (s ?? "").Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        StringBuilder sb = new();
        foreach (var c in s)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark) sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    void MostrarMuyBien()
    {
        titleText.text = "¡Muy bien!";
        titleText.color = Color.white; // tiza
        feedbackText.text = "";
        Invoke(nameof(VolverAlSalon), 2f);
    }

    void VolverAlSalon()
    {
        SceneManager.LoadScene(classroomSceneName);
    }
}
