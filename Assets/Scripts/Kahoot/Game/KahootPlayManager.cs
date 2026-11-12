using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class KahootPlayManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private KahootCountdown kahootCountdown;
    [SerializeField] private KahootScoreManager scoreManager;

    [Header("Question Panels")]
    [SerializeField] private GameObject quizPanel;

    [SerializeField] private GameObject questionWithImage;
    [SerializeField] private GameObject questionWithoutImage;

    [SerializeField] private GameObject answers2Panel;
    [SerializeField] private GameObject answers3Panel;
    [SerializeField] private GameObject answers4Panel;

    [Header("Question UI")]
    [SerializeField] private TMP_Text quiestionText;
    [SerializeField] private TMP_Text quiestionTextWI;
    [SerializeField] private Image questionImage;

    [Header("Answer Buttons")]
    [SerializeField] private AnswerButton[] answers2;
    [SerializeField] private AnswerButton[] answers3;
    [SerializeField] private AnswerButton[] answers4;

    [Header("Image")]
    [SerializeField] private string imageResourcesFolder = "KahootImages";

    [Header("QuestionTimer")]
    [SerializeField] private TMP_Text questionTimerText;
    [SerializeField] private GameObject continueButton;

    [Header("Question Info")]
    [SerializeField] private TMP_Text currentQuestionText;

    private KahootData currentKahoot;
    private KahootGameState currentState;

    private float remainingTime;
    private Coroutine questionTimerCoroutine;

    private int currentQuestionIndex;

    private bool scoreSaved = false;

    private void Start()
    {
        currentKahoot = AppManager.Instance.CurrentKahoot;

        currentQuestionIndex = 0;

        StartQuestion();
    }

    private void StartQuestion()
    {
        currentState = KahootGameState.COUNTDOWN;

        scoreManager.HideQuestionResult();
        quizPanel.SetActive(false);
        continueButton.SetActive(false);

        PreguntaData pregunta = currentKahoot.preguntas[currentQuestionIndex];

        kahootCountdown.StartCountdown(pregunta.pregunta, OnCountdownFinished);
    }

    private void LoadQuestion()
    {
        PreguntaData pregunta = currentKahoot.preguntas[currentQuestionIndex];

        currentQuestionText.text = (currentQuestionIndex + 1) + "/" + currentKahoot.preguntas.Count;

        quiestionTextWI.text = pregunta.pregunta;
        quiestionText.text = pregunta.pregunta;

        bool hasImage = !string.IsNullOrEmpty(pregunta.imagen);

        questionWithImage.SetActive(hasImage);
        questionWithoutImage.SetActive(!hasImage);

        if (hasImage)
        {
            questionImage.sprite = Resources.Load<Sprite>(imageResourcesFolder + "/" + pregunta.imagen);
        }

        answers2Panel.SetActive(pregunta.respuestas.Count == 2);
        answers3Panel.SetActive(pregunta.respuestas.Count == 3);
        answers4Panel.SetActive(pregunta.respuestas.Count == 4);

        switch (pregunta.respuestas.Count)
        {
            case 2:
                LoadAnswers(answers2, pregunta);
                break;

            case 3:
                LoadAnswers(answers3, pregunta);
                break;

            case 4:
                LoadAnswers(answers4, pregunta);
                break;
        }
    }

    private void LoadAnswers(AnswerButton[] buttons, PreguntaData pregunta)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetText(pregunta.respuestas[i].texto, i, OnAnswerSelected);
        }

        SetAnswersState(buttons, true, null);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetDrakened(false);
        }
    }

    private void OnAnswerSelected(int index)
    {
        if (currentState != KahootGameState.QUESTION)
        {
            return;
        }

        PreguntaData pregunta = currentKahoot.preguntas[currentQuestionIndex];

        bool isCorrect = pregunta.respuestas[index].correcta;

        if (isCorrect)
        {
            AudioManager.Instance.PlaySFX(SFXType.CORRECT_ANSWER);
        }
        else
        {
            AudioManager.Instance.PlaySFX(SFXType.WRONG_ANSWER);
        }

        float duration = currentKahoot.preguntas[currentQuestionIndex].duracion;

        scoreManager.RegisterAnswer(isCorrect, remainingTime, duration);

        if (questionTimerCoroutine != null)
        {
            StopCoroutine(questionTimerCoroutine);
            questionTimerCoroutine = null;
        }
        FinishQuestion();
    }

    private void SetAnswersState(AnswerButton[] buttons, bool interactable, PreguntaData pregunta)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetInteractable(interactable);

            if (pregunta != null)
            {
                bool isIncorrect = !pregunta.respuestas[i].correcta;
                buttons[i].SetDrakened(!interactable && isIncorrect);
            }
        }
    }

    private void StartQuestionTimer()
    {
        if (questionTimerCoroutine != null)
        {
            StopCoroutine(questionTimerCoroutine);
        }

        int duration = currentKahoot.preguntas[currentQuestionIndex].duracion;

        questionTimerCoroutine = StartCoroutine(QuestionTimer(duration));
    }

    private IEnumerator QuestionTimer(int duration)
    {
        remainingTime = duration;

        while (remainingTime > 0 && currentState == KahootGameState.QUESTION)
        {
            questionTimerText.text = Mathf.CeilToInt(remainingTime).ToString();

            yield return new WaitForSeconds(1);

            remainingTime--;
        }

        questionTimerText.text = "0";

        if (currentState == KahootGameState.QUESTION)
        {
            scoreManager.RegisterAnswer(false, 0, duration);
            FinishQuestion();
        }

        questionTimerCoroutine = null;
    }

    private void OnCountdownFinished()
    {
        currentState = KahootGameState.QUESTION;

        quizPanel.SetActive(true);

        LoadQuestion();
        StartQuestionTimer();
    }

    private void FinishQuestion()
    {
        if (currentState != KahootGameState.QUESTION)
        {
            return;
        }

        if (currentQuestionIndex == currentKahoot.preguntas.Count - 1)
        {
            currentState = KahootGameState.FINISHED;
        }
        else
        {
            currentState = KahootGameState.RESULT;
        }

        PreguntaData pregunta = currentKahoot.preguntas[currentQuestionIndex];

        switch (pregunta.respuestas.Count)
        {
            case 2:
                SetAnswersState(answers2, false, pregunta);
                break;

            case 3:
                SetAnswersState(answers3, false, pregunta);
                break;

            case 4:
                SetAnswersState(answers4, false, pregunta);
                break;
        }

        questionTimerText.text = "";
        continueButton.SetActive(true);
        scoreManager.ShowQuestionResult();
    }

    public void ContinueQuestion()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);
        if (currentState == KahootGameState.RESULT)
        {
            currentQuestionIndex++;
            StartQuestion();
        }
        else if (currentState == KahootGameState.FINISHED)
        {
            if (!scoreSaved)
            {
                LeaderboardManager.SaveScore(AppManager.Instance.CurrentKahootPath, PlayerSession.Username, scoreManager.TotalScore);

                scoreSaved = true;
            }
            AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);
            ChangeSceneManager.Instance.GoToLeaderboard();
        }
    }
}
