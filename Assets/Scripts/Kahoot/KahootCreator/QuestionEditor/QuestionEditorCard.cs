using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

public class QuestionEditorCard : MonoBehaviour
{
    [Header("Question")]
    [SerializeField] private TMP_InputField questionInput;
    [SerializeField] private TMP_InputField durationInput;
    [SerializeField] private TMP_Dropdown answersDropdown;

    [Header("Answer Panels")]
    [SerializeField] private GameObject answers2Panel;
    [SerializeField] private GameObject answers3Panel;
    [SerializeField] private GameObject answers4Panel;

    [Header("Answers 2")]
    [SerializeField] private TMP_InputField[] answers2Input;
    [SerializeField] private Toggle[] answers2Correct;

    [Header("Answers 3")]
    [SerializeField] private TMP_InputField[] answers3Input;
    [SerializeField] private Toggle[] answers3Correct;

    [Header("Answers 4")]
    [SerializeField] private TMP_InputField[] answers4Input;
    [SerializeField] private Toggle[] answers4Correct;

    private void Start()
    {
        answersDropdown.onValueChanged.AddListener(UpdateAnswerPanels);
        UpdateAnswerPanels(answersDropdown.value);
    }

    private void UpdateAnswerPanels(int value)
    {
        answers2Panel.SetActive(value == 0);
        answers3Panel.SetActive(value == 1);
        answers4Panel.SetActive(value == 2);
    }

    public PreguntaData GetQuestionData()
    {
        PreguntaData question = new PreguntaData();

        question.pregunta = questionInput.text;
        question.duracion = int.Parse(durationInput.text);
        question.imagen = "";

        int answerCount = answersDropdown.value + 2;

        question.respuestas = new List<RespuestaData>();

        TMP_InputField[] inputs;
        Toggle[] toggles;

        switch (answerCount)
        {
            case 2:
                inputs = answers2Input;
                toggles = answers2Correct;
                break;

            case 3:
                inputs = answers3Input;
                toggles = answers3Correct;
                break;

            default:
                inputs = answers4Input;
                toggles = answers4Correct;
                break;
        }

        for (int i = 0; i < answerCount; i++)
        {
            RespuestaData answer = new RespuestaData();

            answer.texto = inputs[i].text;
            answer.correcta = toggles[i].isOn;

            question.respuestas.Add(answer);
        }

        return question;
    }

    public void SetQuestionData(PreguntaData question)
    {
        questionInput.text = question.pregunta;
        durationInput.text = question.duracion.ToString();

        int answerCount = question.respuestas.Count;

        answersDropdown.value = answerCount - 2;
        UpdateAnswerPanels(answersDropdown.value);

        TMP_InputField[] inputs;
        Toggle[] toggles;

        switch (answerCount)
        {
            case 2:
                inputs = answers2Input;
                toggles = answers2Correct;
                break;

            case 3:
                inputs = answers3Input;
                toggles = answers3Correct;
                break;

            default:
                inputs = answers4Input;
                toggles = answers4Correct;
                break;
        }

        for (int i = 0; i < answerCount; i++)
        {
            inputs[i].text = question.respuestas[i].texto;
            toggles[i].isOn = question.respuestas[i].correcta;
        }
    }

    public bool TryGetQuestionData(out PreguntaData question, out string error)
    {
        question = null;
        error = "";

        if (string.IsNullOrWhiteSpace(questionInput.text))
        {
            error = "Hay una pregunta sin enunciado";
            return false;
        }

        if (!int.TryParse(durationInput.text, out int duration) || duration <= 0)
        {
            error = "La duracion debe ser un número mayor que cero";
            return false;
        }

        int answerCount = answersDropdown.value + 2;

        TMP_InputField[] inputs;
        Toggle[] toggles;

        switch (answerCount)
        {
            case 2:
                inputs = answers2Input;
                toggles = answers2Correct;
                break;

            case 3:
                inputs = answers3Input;
                toggles = answers3Correct;
                break;

            default:
                inputs = answers4Input;
                toggles = answers4Correct;
                break;
        }

        bool hasCorrect = false;
        bool hasIncorrect = false;

        for (int i = 0;  i < inputs.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(inputs[i].text))
            {
                error = "Hay una respuesta vacía en una pregunta.";
                return false;
            }

            if (toggles[i].isOn)
            {
                hasCorrect = true;
            }
            else
            {
                hasIncorrect = true;
            }
        }

        if (!hasCorrect || !hasIncorrect)
        {
            error = "Cada pregunta necesita respuestas correctas e incorrectas";
            return false;
        }

        question = GetQuestionData();
        return true;
    }

}
