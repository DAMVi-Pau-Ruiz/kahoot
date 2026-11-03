using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CarouselCard : MonoBehaviour
{
    [Header("Card References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button cardButton;

    public void SetData(ModeData data)
    {
        titleText.text = data.modeName;
        descriptionText.text = data.description;
        iconImage.sprite = data.icon;
        backgroundImage.sprite = data.backgorund;
    }

    public void SetButtonAction(UnityAction action)
    {
        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(action);
    }

}
