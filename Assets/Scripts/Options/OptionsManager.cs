
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class OptionsManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TMP_Text musicPercentageText;
    [SerializeField] private TMP_Text sfxPercentageText;

    [Header("Screen")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Panels")]
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject screenPanel;

    [Header("Tab Buttons")]
    [SerializeField] private Button audioButton;
    [SerializeField] private Button screenButton;
    [SerializeField] private Image audioButtonImage;
    [SerializeField] private Image screenButtonImage;

    private List<Resolution> resolutions = new List<Resolution>();

    private void Start()
    {
        LoadResolutions();

        // Cargar valores del singleton
        musicSlider.SetValueWithoutNotify(
            SettingsManager.Instance.MusicVolume);

        sfxSlider.SetValueWithoutNotify(
            SettingsManager.Instance.SFXVolume);

        fullscreenToggle.SetIsOnWithoutNotify(
            SettingsManager.Instance.Fullscreen);

        UpdateAudioPercentages();

        // Listeners
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);

        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        fullscreenToggle.onValueChanged.AddListener(ChangeFullscreen);

        audioButton.onClick.AddListener(ShowAudioPanel);
        screenButton.onClick.AddListener(ShowScreenPanel);

        ShowAudioPanel();
    }

    private void ChangeMusicVolume(float value)
    {
        SettingsManager.Instance.SetMusicVolume(value);
        musicPercentageText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    private void ChangeSFXVolume(float value)
    {
        SettingsManager.Instance.SetSFXVolume(value);
        sfxPercentageText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    private void UpdateAudioPercentages()
    {
        musicPercentageText.text =
            Mathf.RoundToInt(SettingsManager.Instance.MusicVolume * 100) + "%";

        sfxPercentageText.text =
            Mathf.RoundToInt(SettingsManager.Instance.SFXVolume * 100) + "%";
    }

    private void LoadResolutions()
    {
        Resolution[] availableResolutions = Screen.resolutions;

        resolutions.Clear();
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        foreach (Resolution resolution in availableResolutions)
        {
            bool alreadyExists = false;

            foreach (Resolution savedResolution in resolutions)
            {
                if (savedResolution.width == resolution.width &&
                    savedResolution.height == resolution.height)
                {
                    alreadyExists = true;
                    break;
                }
            }

            if (!alreadyExists)
            {
                resolutions.Add(resolution);
                options.Add(resolution.width + " x " + resolution.height);
            }
        }

        resolutionDropdown.AddOptions(options);

        int selectedIndex = 0;

        for (int i = 0; i < resolutions.Count; i++)
        {
            if (resolutions[i].width == SettingsManager.Instance.ResolutionWidth &&
                resolutions[i].height == SettingsManager.Instance.ResolutionHeight)
            {
                selectedIndex = i;
                break;
            }
        }

        resolutionDropdown.SetValueWithoutNotify(selectedIndex);
        resolutionDropdown.RefreshShownValue();
    }

    private void ChangeResolution(int index)
    {
        Resolution resolution = resolutions[index];

        SettingsManager.Instance.SetResolution(
            resolution.width,
            resolution.height);
    }

    private void ChangeFullscreen(bool value)
    {
        SettingsManager.Instance.SetFullscreen(value);
    }

    public void ShowAudioPanel()
    {
        audioPanel.SetActive(true);
        screenPanel.SetActive(false);

        SetButtonAlpha(audioButtonImage, true);
        SetButtonAlpha(screenButtonImage, false);
    }

    public void ShowScreenPanel()
    {
        audioPanel.SetActive(false);
        screenPanel.SetActive(true);

        SetButtonAlpha(audioButtonImage, false);
        SetButtonAlpha(screenButtonImage, true);
    }

    private void SetButtonAlpha(Image buttonImage, bool selected)
    {
        Color color = buttonImage.color;
        color.a = selected ? 70f / 255f : 0f;

        buttonImage.color = color;
    }

    public void GoBack()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);
        ChangeSceneManager.Instance.GoToMainMenu();
    }
}
