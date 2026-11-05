
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KahootCard : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button selectButton;

    private string jsonPath;

    public event Action<string> OnKahootSelected;

    private void Awake()
    {
        selectButton.onClick.AddListener(SelectKahoot);
    }

    public void SetKahoot(string title, string description,
                          Sprite icon, string path)
    {
        titleText.text = title;
        descriptionText.text = description;
        iconImage.sprite = icon;
        jsonPath = path;
    }

    private void SelectKahoot()
    {
        OnKahootSelected?.Invoke(jsonPath);
    }

    private void OnDestroy()
    {
        selectButton.onClick.RemoveListener(SelectKahoot);
    }
}
