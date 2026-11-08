using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class KahootCard : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    private Action onClickCallback;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    public void SetData(KahootData kahoot, Sprite portada, Action callback)
    {
        titleText.text = kahoot.titulo;
        descriptionText.text = kahoot.descripcion;
        image.sprite = portada;

        onClickCallback = callback;
    }

    public void OnClick()
    {
        onClickCallback?.Invoke();
    }
}

