using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System;
using System.IO;
using System.Collections.Generic;

public class OptionsManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
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

    private SettingsData settings;

    private Resolution[] availableResolutions;
    private List<Resolution> resolutions = new List<Resolution>();

    private string settingsPath;

    private void Start()
    {
        settingsPath = Path.Combine(Application.persistentDataPath, "settings.json");

        LoadSettings();
        LoadResolutions();

        musicSlider.value = settings.musicVolume;
        sfxSlider.value = settings.effectsVolume;

        musicPercentageText.text = Mathf.RoundToInt(settings.musicVolume * 100) + "%";
        sfxPercentageText.text = Mathf.RoundToInt(settings.effectsVolume * 100) + "%";

        fullscreenToggle.isOn = settings.fullscreen;

        ApplyAudioSettings();
        ApplyScreenSettings();

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
        settings.musicVolume = value;

        musicPercentageText.text = Mathf.RoundToInt(value * 100) + "%";

        ApplyAudioSettings();
    }

    private void ChangeSFXVolume(float value)
    {
        settings.effectsVolume = value;

        sfxPercentageText.text = Mathf.RoundToInt(value * 100) + "%";

        ApplyAudioSettings();
    }

    private void ApplyAudioSettings()
    {
        float musicDB = Mathf.Log10(Mathf.Max(settings.musicVolume, 0.0001f)) * 20f;
        float sfxDB = Mathf.Log10(Mathf.Max(settings.effectsVolume, 0.0001f)) * 20f;

        audioMixer.SetFloat("MusicVolume", musicDB);
        audioMixer.SetFloat("EffectsVolume", sfxDB);
    }

    private void LoadResolutions()
    {
        availableResolutions = Screen.resolutions;

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
            if (resolutions[i].width == settings.resolutionWidth &&
                resolutions[i].height == settings.resolutionHeight)
            {
                selectedIndex = i;
                break;
            }
        }

        settings.resolutionWidth = resolutions[selectedIndex].width;
        settings.resolutionHeight = resolutions[selectedIndex].height;

        resolutionDropdown.SetValueWithoutNotify(selectedIndex);
        resolutionDropdown.RefreshShownValue();
    }

    private void ChangeResolution(int index)
    {
        Resolution resolution = resolutions[index];

        settings.resolutionWidth = resolution.width;
        settings.resolutionHeight = resolution.height;

        ApplyScreenSettings();
    }

    private void ChangeFullscreen(bool value)
    {
        settings.fullscreen = value;
        ApplyScreenSettings();
    }

    private void ApplyScreenSettings()
    {
        Screen.SetResolution(
            settings.resolutionWidth,
            settings.resolutionHeight,
            settings.fullscreen
        );
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

    private void LoadSettings()
    {
        if (File.Exists(settingsPath))
        {
            try
            {
                string json = File.ReadAllText(settingsPath);
                settings = JsonUtility.FromJson<SettingsData>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("Error cargando los ajustes: " + e.Message);
            }
        }

        if (settings == null)
        {
            settings = new SettingsData();
        }
    }

    private void SaveSettings()
    {
        try
        {
            string json = JsonUtility.ToJson(settings, true);
            File.WriteAllText(settingsPath, json);

            Debug.Log("Configuracion guardada correctamente.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error guardando los ajustes: " + e.Message);
        }
    }

    public void GoBack()
    {
        SaveSettings();
        ChangeSceneManager.Instance.GoToMainMenu();
    }
}
