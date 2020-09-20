using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    private AudioSource lampAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        lampAudioSource = GetComponent<AudioSource>();
        PlayFireTorch();
    }

    private void PlayFireTorch() {
        lampAudioSource.loop = true;
        lampAudioSource.clip = AudioManager.Instance.fireTorch;
        lampAudioSource.Play();
    }
}
