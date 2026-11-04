using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI answerText;

    private int answerIndex;

    public void SetAnswer(string text, int index)
    {
        answerText.text = text;
        answerIndex = index;
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }

    public void OnClick()
    {
        // Más adelante conectaremos esto con KahootGameManager.
    }
}