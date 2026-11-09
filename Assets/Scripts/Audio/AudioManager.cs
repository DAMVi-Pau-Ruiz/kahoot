using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Audio;
using System;

[Serializable]
public class SFXData
{
    public SFXType type;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private int currentMusicIndex = 0;

    [Header("Sound Effects")]
    [SerializeField] private SFXData[] soundEffects;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource.loop = true;

        PlayMusic(currentMusicIndex);
    }

    public void PlayMusic(int index)
    {
        if (musicClips.Length == 0)
        {
            return;
        }

        if (index < 0 || index >= musicClips.Length)
        {
            return;
        }

        currentMusicIndex = index;

        musicSource.clip = musicClips[currentMusicIndex];
        musicSource.Play();
    }

    public void NextMusic()
    {
        if (musicClips.Length == 0)
        {
            return;
        }

        int nextIndex = (currentMusicIndex + 1) % musicClips.Length;

        PlayMusic(nextIndex);
    }

    public void PreviousMusic()
    {
        if (musicClips.Length == 0)
        {
            return;
        }

        int previousIndex = (currentMusicIndex - 1 + musicClips.Length) % musicClips.Length;

        PlayMusic(previousIndex);
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    public void PlaySFX(SFXType type)
    {
        foreach (SFXData sound in soundEffects)
        {
            if (sound.type == type)
            {
                sfxSource.PlayOneShot(sound.clip);
                return;
            }
        }
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);

        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);

        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20f);
    }

}
