using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    private void Awake() {
        if(_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /*
    [Header("Audio Sources")]
    // List audio sources here

    [Header("SFX Clips")]
    // List sound effect clips here

    [Header("Ambience sound Clips")]
    // List ambience sound clips here

    [Header("BGM Clips")]
    // List Background music clips here
    */
}
