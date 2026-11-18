using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(-100)]
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    public float MusicVolume {  get; private set; }
    public float SFXVolume {  get; private set; }

    public int ResolutionWidth { get; private set; }
    public int ResolutionHeight { get; private set; }

    public bool Fullscreen { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        ResolutionWidth = PlayerPrefs.GetInt(
            "ResolutionWidth", Screen.currentResolution.width);

        ResolutionHeight = PlayerPrefs.GetInt(
            "ResolutionHeight", Screen.currentResolution.height);

        Fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        ApplyMusicVolume();
        ApplySFXVolume();
        ApplyResolution();
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);

        ApplyMusicVolume();
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        SFXVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);

        ApplySFXVolume();
        PlayerPrefs.Save();
    }

    public void SetResolution(int width, int height)
    {
        ResolutionWidth = width;
        ResolutionHeight = height;

        PlayerPrefs.SetInt("ResolutionWidth", width);
        PlayerPrefs.SetInt("ResolutionHeight", height);

        ApplyResolution();
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool value)
    {
        Fullscreen = value;
        PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);

        ApplyResolution();
        PlayerPrefs.Save();
    }

    private void ApplyMusicVolume()
    {
        audioMixer.SetFloat("MusicVolume", VolumeToDecibels(MusicVolume));
    }

    private void ApplySFXVolume()
    {
        audioMixer.SetFloat("SFXVolume", VolumeToDecibels(SFXVolume));
    }

    private float VolumeToDecibels(float volume)
    {
        if (volume <= 0.0001f)
            return -80f;

        return Mathf.Log10(volume) * 20f;
    }

    private void ApplyResolution()
    {
        FullScreenMode mode = Fullscreen
            ? FullScreenMode.FullScreenWindow
            : FullScreenMode.Windowed;

        Screen.SetResolution(
            ResolutionWidth,
            ResolutionHeight,
            mode
        );
    }
}
