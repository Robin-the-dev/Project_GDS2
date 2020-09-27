using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsHandler : MonoBehaviour {

    public Slider music;
    public Slider voice;
    public Slider soundEffects;
    public Slider ambience;
    public TMP_Dropdown dropdown;

    private AudioManager manager;

    private Canvas lastCanvas;

    public void OpenSettings(Canvas lastCanvas) {
        this.lastCanvas = lastCanvas;
        GetComponent<Canvas>().enabled = true;
        lastCanvas.enabled = false;
        AudioManager.Instance.PlayClickButton();
    }

    public void CloseSettings() {
        if (lastCanvas == null) throw new System.InvalidOperationException("Settings menu must not be the first canvas loaded");
        GetComponent<Canvas>().enabled = false;
        lastCanvas.enabled = true;
        AudioManager.Instance.PlayClickButton();
    }

    void Awake() {
        GetComponent<Canvas>().enabled = false;
        manager = FindObjectOfType<AudioManager>();
        music.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        voice.value = PlayerPrefs.GetFloat("VoiceVolume", 0.5f);
        soundEffects.value = PlayerPrefs.GetFloat("SoundEffectVolume", 0.5f);
        ambience.value = PlayerPrefs.GetFloat("AmbientVolume", 0.5f);
        dropdown.value = PlayerPrefs.GetInt("TextSpeedDropdown", 1);
    }

    public void UpdateMusicVolume() {
        PlayerPrefs.SetFloat("MusicVolume", music.value);
        manager.UpdateVolume();
    }

    public void UpdateVoiceVolume() {
        PlayerPrefs.SetFloat("VoiceVolume", voice.value);
        manager.UpdateVolume();
    }

    public void UpdateSoundEffectVolume() {
        PlayerPrefs.SetFloat("SoundEffectVolume", soundEffects.value);
        manager.UpdateVolume();
    }

    public void UpdateAmbientVolume() {
        PlayerPrefs.SetFloat("AmbientVolume", ambience.value);
        manager.UpdateVolume();
    }

    public void UpdateTextSpeed() {
        PlayerPrefs.SetInt("TextSpeedDropdown", dropdown.value);
        AudioManager.Instance.PlayClickButton();
        switch (dropdown.value) {
            case 0:
                PlayerPrefs.SetFloat("TextSpeed", 2f);
                break;
            case 1:
                PlayerPrefs.SetFloat("TextSpeed", 1f);
                break;
            case 2:
                PlayerPrefs.SetFloat("TextSpeed", 0.5f);
                break;
            case 3:
                PlayerPrefs.SetFloat("TextSpeed", 0.25f);
                break;
            case 4:
                PlayerPrefs.SetFloat("TextSpeed", 0f);
                break;
        }
    }
}
