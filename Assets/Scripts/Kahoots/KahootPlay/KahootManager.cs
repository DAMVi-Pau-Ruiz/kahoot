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

    [SerializeField] private QuestionUI questionUI;

    [Header("Test Kahoot")]
    [SerializeField] private TextAsset testKahootJson;

    private KahootData currentKahoot;
    private KahootGameState currentState;

    private int currentQuestionIndex;
    private int selectedAnswerIndex = -1;

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

        // Comprobar si hay un Kahoot seleccionado
        if (KahootSession.CurrentKahoot != null)
        {
            StartGame(KahootSession.CurrentKahoot);
        }
        else if (testKahootJson != null)
        {
            // Cargar Kahoot de prueba
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

            questionUI.ShowResults(selectedAnswerIndex);

            continueButton.gameObject.SetActive(true);
            continuePressed = false;

            yield return new WaitUntil(() => continuePressed);

            continueButton.gameObject.SetActive(false);

            currentQuestionIndex++;
        }

        currentState = KahootGameState.FINISHED;
        remainingTimeText.text = "FIN";
        //Se ha acabado el juego
    }

    private IEnumerator ShowQuestion()
    {
        currentState = KahootGameState.QUESTION;
        selectedAnswerIndex = -1;

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

        questionUI.DisableAnswers();
    }

    private void HandleAnswerSelected(int index, bool correct)
    {
        if (currentState != KahootGameState.QUESTION)
            return;

        selectedAnswerIndex = index;
        currentState = KahootGameState.REVEAL_ANSWER;
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