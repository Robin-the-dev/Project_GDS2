using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSound : MonoBehaviour {
    // Start is called before the first frame update
    public AudioSource source;

    void OnDisable() {
        source.Stop();
    }

    void OnEnable() {
        source.Play();
    }

    private void Update() {
        if (AudioManager.Instance.isPaused) {
            source.Pause();
        } else {
            source.UnPause();
        }
        float volume = PlayerPrefs.GetFloat("SoundEffectVolume");
        if (source.volume != volume) {
            source.volume = volume;
        }
    }
}
