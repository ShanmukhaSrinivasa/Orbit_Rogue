using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("References")]
    public AudioMixer mainMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // Load saved settings or default to 1 (Max volume)
        float savedMusic = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVol", 0.75f);

        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;

        // Apply immediately
        SetMusicVolume(savedMusic);
        SetSFXVolume(savedSFX);
    }

    public void SetMusicVolume(float sliderValue)
    {
        // Math: Convert 0.001-1 linear slider to -80db-0db logarithmic volume
        // We use Mathf.Log10(sliderValue) * 20
        float dB = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20;

        mainMixer.SetFloat("MusicVol", dB);
        PlayerPrefs.SetFloat("MusicVol", sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20;

        mainMixer.SetFloat("SFXVol", dB);
        PlayerPrefs.SetFloat("SFXVol", sliderValue);
    }
}