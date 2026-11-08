using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MainMenuCard : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button optionButton;

    private Action<AppAction> onOptionSelected;
    private AppAction currentAction;

    public void SetData(MainMenuOption option, Action<AppAction> callback)
    {
        titleText.text = option.title;
        description.text = option.description;
        iconImage.sprite = option.icon;

        currentAction = option.action;
        onOptionSelected = callback;
    }

    private void Awake()
    {
        optionButton.onClick.AddListener(SelectOption);
    }

    private void SelectOption()
    {
        onOptionSelected?.Invoke(currentAction);
    }
}
