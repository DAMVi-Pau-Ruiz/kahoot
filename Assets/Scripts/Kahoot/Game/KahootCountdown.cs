using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class KahootCountdown : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float countdownDuration = 5;

    [Header("UI References")]
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private TMP_Text preguntaText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private Slider countdownSlider;

    private Coroutine countdownCoroutine;

    public void StartCountdown(string pregunta, Action onFinished)
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        countdownCoroutine = StartCoroutine(Countdown(pregunta, onFinished));
    }

    private IEnumerator Countdown(string pregunta, Action onFinished)
    {
        countdownPanel.SetActive(true);

        preguntaText.text = pregunta;

        countdownSlider.minValue = 0;
        countdownSlider.maxValue = countdownDuration;
        countdownSlider.value = countdownDuration;

        float remainingTime = countdownDuration;

        while (remainingTime > 0)
        {
            countdownText.text = Mathf.CeilToInt(remainingTime).ToString();
            countdownSlider.value = remainingTime;

            yield return null;

            remainingTime -= Time.deltaTime;
        }

        countdownText.text = "0";
        countdownSlider.value = 0;

        countdownPanel.SetActive(false);

        countdownCoroutine = null;

        onFinished?.Invoke();
    }
}
