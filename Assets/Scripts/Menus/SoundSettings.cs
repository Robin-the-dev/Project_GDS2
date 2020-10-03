using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour {

    public Slider music;
    public Slider voice;
    public Slider soundEffects;
    public Slider ambience;

    private AudioManager manager;
    // Start is called before the first frame update
    void Awake() {
        manager = FindObjectOfType<AudioManager>();
        music.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        voice.value = PlayerPrefs.GetFloat("VoiceVolume", 0.5f);
        soundEffects.value = PlayerPrefs.GetFloat("SoundEffectVolume", 0.5f);
        ambience.value = PlayerPrefs.GetFloat("AmbientVolume", 0.5f);
    }

    public void UpdateMusicVolume() {
        if (manager == null) return;
        PlayerPrefs.SetFloat("MusicVolume", music.value);
        manager.UpdateVolume();
    }

    public void UpdateVoiceVolume() {
        if (manager == null) return;
        PlayerPrefs.SetFloat("VoiceVolume", voice.value);
        manager.UpdateVolume();
    }

    public void UpdateSoundEffectVolume() {
        if (manager == null) return;
        PlayerPrefs.SetFloat("SoundEffectVolume", soundEffects.value);
        manager.UpdateVolume();
    }

    public void UpdateAmbientVolume() {
        if (manager == null) return;
        PlayerPrefs.SetFloat("AmbientVolume", ambience.value);
        manager.UpdateVolume();
    }
}
