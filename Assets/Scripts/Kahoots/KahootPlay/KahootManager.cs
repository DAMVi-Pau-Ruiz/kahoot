
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KahootManager : MonoBehaviour
{
    [Header("Intro UI")]
    [SerializeField] private GameObject introPanel;
    [SerializeField] private TMP_Text introText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private Slider countdownSlider;

    [Header("Game UI")]
    [SerializeField] private TMP_Text remainingTimeText;
    [SerializeField] private TMP_Text questionCountText;
    [SerializeField] private Button continueButton;

    [Header("Score UI")]
    [SerializeField] private TMP_Text totalScoreText;
    [SerializeField] private GameObject streakPanel;
    [SerializeField] private TMP_Text streakText;
    [SerializeField] private TMP_Text pointsGainedText;

    [SerializeField] private QuestionUI questionUI;

    [Header("Test Kahoot")]
    [SerializeField] private TextAsset testKahootJson;

    private int totalScore = 0;
    private int correctStreak = 0;

    private KahootData currentKahoot;
    private KahootGameState currentState;

    private int currentQuestionIndex;
    private int selectedAnswerIndex = -1;
    private bool selectedAnswerCorrect;

    private bool continuePressed;

    private Coroutine gameCoroutine;

    private void Awake()
    {
        questionUI.OnAnswerSelected += HandleAnswerSelected;
        continueButton.onClick.AddListener(ContinueGame);
    }

    private void Start()
    {
        continueButton.gameObject.SetActive(false);

        if (KahootSession.CurrentKahoot != null)
        {
            StartGame(KahootSession.CurrentKahoot);
        }
        else if (testKahootJson != null)
        {
            KahootData kahoot =
                JsonUtility.FromJson<KahootData>(testKahootJson.text);

            StartGame(kahoot);
        }
        else
        {
            Debug.LogError("No hay ningún Kahoot seleccionado ni uno de prueba.");
        }
    }

    private void OnDestroy()
    {
        questionUI.OnAnswerSelected -= HandleAnswerSelected;
        continueButton.onClick.RemoveListener(ContinueGame);
    }

    public void StartGame(KahootData kahoot)
    {
        if (kahoot == null || kahoot.preguntas == null ||
            kahoot.preguntas.Count == 0)
        {
            Debug.LogError("El Kahoot no contiene preguntas.");
            return;
        }

        currentKahoot = kahoot;
        currentQuestionIndex = 0;

        totalScore = 0;
        correctStreak = 0;

        totalScoreText.text = "0";
        streakPanel.SetActive(false);
        pointsGainedText.gameObject.SetActive(false);

        PlayerSession.SetScore(0);

        if (gameCoroutine != null)
            StopCoroutine(gameCoroutine);

        gameCoroutine = StartCoroutine(PlayGame());
    }

    private IEnumerator PlayGame()
    {
        while (currentQuestionIndex < currentKahoot.preguntas.Count)
        {
            questionCountText.text =
                (currentQuestionIndex + 1) + "/" +
                currentKahoot.preguntas.Count;

            yield return StartCoroutine(ShowIntro());

            yield return StartCoroutine(ShowQuestion());

            remainingTimeText.gameObject.SetActive(false);

            questionUI.ShowResults(selectedAnswerIndex);

            // Si es la última pregunta, no esperamos aquí.
            if (currentQuestionIndex < currentKahoot.preguntas.Count - 1)
            {
                continueButton.gameObject.SetActive(true);
                continuePressed = false;

                yield return new WaitUntil(() => continuePressed);

                continueButton.gameObject.SetActive(false);
            }

            currentQuestionIndex++;
        }

        currentState = KahootGameState.FINISHED;
        remainingTimeText.text = "FIN";

        PlayerSession.SetScore(totalScore);

        Debug.Log("Puntuación final: " + totalScore);

        // Mostramos el botón para continuar al leaderboard.
        continueButton.gameObject.SetActive(true);
        continuePressed = false;

        // Esperamos a que el jugador pulse Siguiente.
        yield return new WaitUntil(() => continuePressed);

        continueButton.gameObject.SetActive(false);

        // Guardamos el resultado.
        bool saved = RankingXMLManager.SaveResult(
            KahootSession.SelectedKahootPath,
            PlayerSession.Username,
            PlayerSession.Score
        );

        if (saved)
        {
            UIManager.Instance.GoToLeaderboard();
        }
    }

    private IEnumerator ShowQuestion()
    {
        remainingTimeText.gameObject.SetActive(true);
        pointsGainedText.gameObject.SetActive(false);
        currentState = KahootGameState.QUESTION;
        selectedAnswerIndex = -1;
        selectedAnswerCorrect = false;

        PreguntaData question =
            currentKahoot.preguntas[currentQuestionIndex];

        questionUI.ShowQuestion(question);

        float remainingTime = question.duracion;

        while (remainingTime > 0 &&
               currentState == KahootGameState.QUESTION)
        {
            remainingTimeText.text =
                Mathf.CeilToInt(remainingTime).ToString();

            yield return null;

            remainingTime -= Time.deltaTime;
        }

        remainingTimeText.text = "0";

        if (currentState == KahootGameState.QUESTION)
        {
            currentState = KahootGameState.REVEAL_ANSWER;
        }

        // Calculamos la puntuación antes de mostrar los resultados.
        CalculateScore(remainingTime, question.duracion);

        questionUI.DisableAnswers();
    }

    private void HandleAnswerSelected(int index, bool correct)
    {
        if (currentState != KahootGameState.QUESTION)
            return;

        selectedAnswerIndex = index;
        selectedAnswerCorrect = correct;

        currentState = KahootGameState.REVEAL_ANSWER;
    }

    private void CalculateScore(float remainingTime, float duration)
    {
        int questionScore = 0;

        if (selectedAnswerCorrect)
        {
            correctStreak++;

            int basePoints = 100;
            int timeBonus = 0;

            if (duration > 0)
            {
                timeBonus = Mathf.RoundToInt(
                    100f * Mathf.Clamp01(remainingTime / duration)
                );
            }

            int streakBonus = (correctStreak - 1) * 25;

            questionScore = basePoints + timeBonus + streakBonus;

            totalScore += questionScore;
        }
        else
        {
            correctStreak = 0;
        }

        // Actualizamos la puntuación permanente.
        totalScoreText.text = totalScore.ToString();

        // Actualizamos la racha.
        streakPanel.SetActive(correctStreak >= 2);

        if (correctStreak >= 2)
        {
            streakText.text = correctStreak.ToString();
        }

        // Mostramos los puntos obtenidos si ha respondido.
        if (selectedAnswerIndex != -1)
        {
            pointsGainedText.text = "+" + questionScore;
            pointsGainedText.gameObject.SetActive(true);
        }

        PlayerSession.SetScore(totalScore);

        Debug.Log("Puntuación total: " + totalScore);
    }

    private void ContinueGame()
    {
        continuePressed = true;
    }

    private IEnumerator ShowIntro()
    {
        currentState = KahootGameState.INTRO;

        introPanel.SetActive(true);
        questionUI.DisableAnswers();

        PreguntaData question =
            currentKahoot.preguntas[currentQuestionIndex];

        introText.text = question.pregunta;

        float remainingTime = 5f;

        countdownSlider.maxValue = remainingTime;
        countdownSlider.value = remainingTime;

        while (remainingTime > 0)
        {
            countdownText.text =
                Mathf.CeilToInt(remainingTime).ToString();

            countdownSlider.value = remainingTime;

            yield return null;

            remainingTime -= Time.deltaTime;
        }

        countdownText.text = "0";
        countdownSlider.value = 0;

        introPanel.SetActive(false);
    }
}
