using System;

[Serializable]
public class SettingsData
{
    public float musicVolume = 0.8f;
    public float effectsVolume = 0.8f;

    public bool fullscreen = true;

    public int resolutionWidth = 1920;
    public int resolutionHeight = 1080;
}