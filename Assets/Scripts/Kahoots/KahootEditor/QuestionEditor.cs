
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestionEditor : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private TMP_InputField questionInput;
    [SerializeField] private TMP_Dropdown answerDropdown;

    [Header("Templates")]
    [SerializeField] private GameObject template2Answers;
    [SerializeField] private GameObject template3Answers;
    [SerializeField] private GameObject template4Answers;

    [Header("2 Answers")]
    [SerializeField] private TMP_InputField[] answerInputs2;
    [SerializeField] private Toggle[] answerChecks2;

    [Header("3 Answers")]
    [SerializeField] private TMP_InputField[] answerInputs3;
    [SerializeField] private Toggle[] answerChecks3;

    [Header("4 Answers")]
    [SerializeField] private TMP_InputField[] answerInputs4;
    [SerializeField] private Toggle[] answerChecks4;

    [Header("Question Settings")]
    [SerializeField] private TMP_InputField durationInput;

    private void Start()
    {
        answerDropdown.onValueChanged.AddListener(ChangeTemplate);
        ChangeTemplate(answerDropdown.value);
    }

    private void ChangeTemplate(int option)
    {
        template2Answers.SetActive(option == 0);
        template3Answers.SetActive(option == 1);
        template4Answers.SetActive(option == 2);
    }

    public PreguntaData GetQuestionData()
    {
        PreguntaData questionData = new PreguntaData();

        questionData.pregunta = questionInput.text.Trim();
        int duration;

        if (int.TryParse(durationInput.text, out duration))
        {
            questionData.duracion = duration;
        }
        else
        {
            questionData.duracion = 0;
        }
        questionData.respuestas = new List<RespuestaData>();

        TMP_InputField[] selectedInputs = answerInputs2;
        Toggle[] selectedChecks = answerChecks2;

        if (answerDropdown.value == 1)
        {
            selectedInputs = answerInputs3;
            selectedChecks = answerChecks3;
        }
        else if (answerDropdown.value == 2)
        {
            selectedInputs = answerInputs4;
            selectedChecks = answerChecks4;
        }

        for (int i = 0; i < selectedInputs.Length; i++)
        {
            RespuestaData answer = new RespuestaData();

            answer.texto = selectedInputs[i].text.Trim();
            answer.correcta = selectedChecks[i].isOn;

            questionData.respuestas.Add(answer);
        }

        return questionData;
    }

    private void OnDestroy()
    {
        answerDropdown.onValueChanged.RemoveListener(ChangeTemplate);
    }


    public void SetQuestionData(PreguntaData data)
    {
        questionInput.text = data.pregunta;
        durationInput.text = data.duracion.ToString();

        int answerCount = data.respuestas.Count;

        // Seleccionamos el template correspondiente.
        answerDropdown.value = answerCount - 2;
        ChangeTemplate(answerDropdown.value);

        TMP_InputField[] selectedInputs = answerInputs2;
        Toggle[] selectedChecks = answerChecks2;

        if (answerCount == 3)
        {
            selectedInputs = answerInputs3;
            selectedChecks = answerChecks3;
        }
        else if (answerCount == 4)
        {
            selectedInputs = answerInputs4;
            selectedChecks = answerChecks4;
        }

        // Introducimos las respuestas.
        for (int i = 0; i < answerCount; i++)
        {
            selectedInputs[i].text = data.respuestas[i].texto;
            selectedChecks[i].isOn = data.respuestas[i].correcta;
        }
    }

}
