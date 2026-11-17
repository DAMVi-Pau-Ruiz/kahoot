using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AnswerButton : MonoBehaviour
{
    [SerializeField] private TMP_Text answerText;
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;

    private int answerIndex;
    private Action<int> onClickCallback;

    private Color originalColor;
    private Color originalTextColor;

    private void Awake()
    {
        originalColor = buttonImage.color;
        originalTextColor = answerText.color;
        button.onClick.AddListener(OnClick);
    }

    public void SetText(string text, int index, Action<int> callback)
    {
        answerText.text = text;
        answerIndex = index;
        onClickCallback = callback;
    }

    private void OnClick()
    {
        onClickCallback?.Invoke(answerIndex);
    }

    public void SetInteractable(bool value)
    {
        button.interactable = value;
    }

    public void SetDrakened(bool value)
    {
        if (value)
        {
            buttonImage.color = new Color(
                originalColor.r * 0.44f,
                originalColor.g * 0.44f,
                originalColor.b * 0.44f,
                originalColor.a
            );

            answerText.color = new Color(
                originalTextColor.r * 0.44f,
                originalTextColor.g * 0.44f,
                originalTextColor.b * 0.44f,
                originalTextColor.a
            );
        }
        else
        {
            buttonImage.color = originalColor;
            answerText.color = originalTextColor;
        }
    }
}
