using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour {
    [HideInInspector] public AudioSource lampAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        lampAudioSource = GetComponent<AudioSource>();
        PlayFireTorch();
    }

    private void Update() {
        float volume = PlayerPrefs.GetFloat("AmbientVolume");
        if (lampAudioSource.volume != volume) {
            lampAudioSource.volume = volume;
        }
        if (AudioManager.Instance.isPaused) {
            lampAudioSource.Pause();
        } else {
            lampAudioSource.UnPause();
        }
    }

    private void PlayFireTorch() {
        lampAudioSource.loop = true;
        lampAudioSource.clip = AudioManager.Instance.fireTorch;
        lampAudioSource.Play();
    }
}
