
using System;
using UnityEngine;
using TMPro;

public class QuestionUI : MonoBehaviour
{
    [Header("Question Templates")]
    [SerializeField] private GameObject questionWithImage;
    [SerializeField] private GameObject questionWithoutImage;

    [Header("Question Text")]
    [SerializeField] private TextMeshProUGUI questionTextImage;
    [SerializeField] private TextMeshProUGUI questionTextOnly;

    [Header("Answer Templates")]
    [SerializeField] private GameObject answers2;
    [SerializeField] private GameObject answers3;
    [SerializeField] private GameObject answers4;

    [Header("Answer Buttons")]
    [SerializeField] private AnswerButton[] buttons2;
    [SerializeField] private AnswerButton[] buttons3;
    [SerializeField] private AnswerButton[] buttons4;

    public event Action<int, bool> OnAnswerSelected;

    private void Awake()
    {
        SubscribeButtons(buttons2);
        SubscribeButtons(buttons3);
        SubscribeButtons(buttons4);
    }

    private void SubscribeButtons(AnswerButton[] buttons)
    {
        foreach (AnswerButton button in buttons)
        {
            button.OnAnswerSelected += HandleAnswerSelected;
        }
    }

    private void HandleAnswerSelected(int index, bool correct)
    {
        OnAnswerSelected?.Invoke(index, correct);
    }

    public void ShowQuestion(PreguntaData question)
    {
        bool hasImage = !string.IsNullOrEmpty(question.imagen);

        questionWithImage.SetActive(hasImage);
        questionWithoutImage.SetActive(!hasImage);

        if (hasImage)
            questionTextImage.text = question.pregunta;
        else
            questionTextOnly.text = question.pregunta;

        answers2.SetActive(question.respuestas.Count == 2);
        answers3.SetActive(question.respuestas.Count == 3);
        answers4.SetActive(question.respuestas.Count == 4);

        AnswerButton[] activeButtons = GetActiveButtons(question.respuestas.Count);

        for (int i = 0; i < activeButtons.Length; i++)
        {
            RespuestaData answer = question.respuestas[i];

            activeButtons[i].SetAnswer(
                answer.texto,
                i,
                answer.correcta
            );

            activeButtons[i].SetInteractable(true);
        }
    }

    private AnswerButton[] GetActiveButtons(int answerCount)
    {
        switch (answerCount)
        {
            case 2:
                return buttons2;

            case 3:
                return buttons3;

            case 4:
                return buttons4;

            default:
                return null;
        }
    }

    public void DisableAnswers()
    {
        DisableButtons(buttons2);
        DisableButtons(buttons3);
        DisableButtons(buttons4);
    }

    private void DisableButtons(AnswerButton[] buttons)
    {
        foreach (AnswerButton button in buttons)
        {
            button.SetInteractable(false);
        }
    }

    public void ShowResults(int selectedIndex)
    {
        ShowButtonResults(buttons2, selectedIndex);
        ShowButtonResults(buttons3, selectedIndex);
        ShowButtonResults(buttons4, selectedIndex);
    }

    private void ShowButtonResults(AnswerButton[] buttons, int selectedIndex)
    {
        foreach (AnswerButton button in buttons)
        {
            button.ShowResult(selectedIndex);
        }
    }
}
