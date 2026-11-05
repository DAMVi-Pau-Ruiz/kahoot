using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI answerText;

    [Header("Result Colors")]
    [SerializeField] private float darkMultiplier = 0.4f;

    private int answerIndex;
    private bool isCorrect;

    private Color originalColor;

    public event Action<int, bool> OnAnswerSelected;

    private void Awake()
    {
        originalColor = button.image.color;

        button.onClick.AddListener(OnClick);
    }

    public void SetAnswer(string text, int index, bool correct)
    {
        answerText.text = text;
        answerIndex = index;
        isCorrect = correct;

        // Recuperar el color original
        button.image.color = originalColor;
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }

    public void ShowResult(int selectedIndex)
    {
        button.interactable = false;

        if (!isCorrect)
        {
            Color darkColor = originalColor * darkMultiplier;
            darkColor.a = originalColor.a;

            button.image.color = darkColor;
        }
        else
        {
            button.image.color = originalColor;
        }
    }

    public void OnClick()
    {
        OnAnswerSelected?.Invoke(answerIndex, isCorrect);
    }
}