using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KahootPlayManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private KahootCountdown kahootCountdown;

    [Header("Question Panels")]
    [SerializeField] private GameObject quizPanel;

    [SerializeField] private GameObject questionWithImage;
    [SerializeField] private GameObject questionWithoutImage;

    [SerializeField] private GameObject answers2Panel;
    [SerializeField] private GameObject answers3Panel;
    [SerializeField] private GameObject answers4Panel;

    private KahootData currentKahoot;
    private KahootGameState currentState;

    private int currentQuestionIndex;

    private void Start()
    {
        currentKahoot = AppManager.Instance.CurrentKahoot;

        currentQuestionIndex = 0;

        StartQuestion();
    }

    private void StartQuestion()
    {
        currentState = KahootGameState.COUNTDOWN;

        quizPanel.SetActive(false);

        PreguntaData pregunta = currentKahoot.preguntas[currentQuestionIndex];

        kahootCountdown.StartCountdown(pregunta.pregunta, OnCountdownFinished);
    }

    private void OnCountdownFinished()
    {
        currentState = KahootGameState.QUESTION;

        quizPanel.SetActive(true);

        Debug.Log("comienza la pregunta");
    }
}
