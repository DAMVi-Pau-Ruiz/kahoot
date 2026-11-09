using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KahootScoreManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private int totalScore;

    [Header("Streak")]
    [SerializeField] private int currentStreak;

    [Header("UI References")]
    [SerializeField] private TMP_Text totalScoreText;
    [SerializeField] private TMP_Text questionScoreText;
    [SerializeField] private TMP_Text streakText;
    [SerializeField] private Image flameImage;

    private int questionScore;

    public int TotalScore => totalScore;
    public int CurrentStreak => currentStreak;
    public int QuestionScore => questionScore;

    private void Start()
    {
        totalScoreText.gameObject.SetActive(true);
        
        questionScoreText.gameObject.SetActive(false);
        streakText.gameObject.SetActive(false);
        flameImage.gameObject.SetActive(false);

        UpdateTotalScore();
    }

    public void RegisterAnswer(bool isCorrect, float remainingTime, float duration)
    {
        questionScore = 0;

        if (!isCorrect)
        {
            currentStreak = 0;
            questionScore = 0;

            UpdateStreak();
            UpdateQuestionScore();
            return;
        }

        currentStreak++;

        int basePoints = 100;

        int timeBonus = Mathf.RoundToInt(100 * (remainingTime / duration));

        int streakBonus = (currentStreak - 1) * 25;

        questionScore = basePoints + timeBonus + streakBonus;

        totalScore += questionScore;

        UpdateTotalScore();
        UpdateQuestionScore();
        UpdateStreak();
    }

    public void ShowQuestionResult()
    {
        questionScoreText.gameObject.SetActive(true);
    }

    public void HideQuestionResult()
    {
        questionScoreText.gameObject.SetActive(false);
    }

    private void UpdateTotalScore()
    {
        totalScoreText.text = totalScore.ToString();
    }

    private void UpdateQuestionScore()
    {
        questionScoreText.text = "+" + questionScore;
    }

    private void UpdateStreak()
    {
        bool showStreak = currentStreak >= 2;

        streakText.gameObject.SetActive(showStreak);
        flameImage.gameObject.SetActive(showStreak);

        if (showStreak)
        {
            streakText.text = currentStreak.ToString();
        }
    }
}
