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

    private void Awake()
    {
        originalColor = buttonImage.color;
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
        buttonImage.color = value ? originalColor * 0.2f : originalColor;
    }
}
