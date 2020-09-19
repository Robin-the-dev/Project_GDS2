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
    
    [Header("Audio Sources")]
    // List audio sources here
    [SerializeField] private AudioSource BGMSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource ambienceSource;

    [Header("SFX Clips")]
    // List sound effect clips here
    [SerializeField] private AudioClip caveLevelOpening;
    [SerializeField] private AudioClip gauntletDiscovery;

    [Header("Ambience sound effect Clips")]
    // List ambience sound clips here
    [SerializeField] private AudioClip fireTorch;
    [SerializeField] private AudioClip rocksFallingInWater;
    [SerializeField] private AudioClip skitteringBugs;
    [SerializeField] private AudioClip waterDrips;
    [SerializeField] private AudioClip waterDropSingle;

    /*
    [Header("BGM Clips")]
    // List Background music clips here
    */

    private float SFXVolume = 1.0f;
    private float BGMVolume = 1.0f;
    private float ambienceVolume = 1.0f;

    #region BGM
    #endregion

    #region SFX
    public void PlayCaveLevelOpening() {
        PlaySFXOneShot(caveLevelOpening);
    }

    public void PlayGauntletDiscovery() {
        PlaySFXOneShot(gauntletDiscovery);
    }
    #endregion

    #region Ambience
    public void PlayFireTorch() {
        PlayAmbienceOneShot(fireTorch);
    }

    // Use below method to loop the fire torch sound
    public void PlayFireTorchLoop() {
        InvokeRepeating("PlayFireTorch", 0.0f, fireTorch.length);
    }

    public void PlayRocksFallingInWater() {
        PlayAmbienceOneShot(rocksFallingInWater);
    }

    public void PlaySkitteringBugs() {
        PlayAmbienceOneShot(skitteringBugs);
    }

    public void PlayWaterDrips() {
        PlayAmbienceOneShot(waterDrips);
    }

    public void PlayWaterDropSingle() {
        PlayAmbienceOneShot(waterDropSingle);
    }
    #endregion

    #region Play Control
    public void StopBGM() {
        BGMSource.Stop();
    }

    private void PlayBGM(AudioClip BGM) {
        BGMSource.clip = BGM;
        BGMSource.loop = true;
        BGMSource.Play();
    }

    private void PlaySFXOneShot(AudioClip SFX, float volume = 1.0f) {
        volume *= SFXVolume;
        SFXSource.PlayOneShot(SFX, volume);
    }

    private void PlayAmbienceOneShot(AudioClip ambience, float volume = 1.0f) {
        volume *= ambienceVolume;
        ambienceSource.PlayOneShot(ambience, volume);
    }
    #endregion

    #region Volume Control
    public void setSFXVolume(float volume) {
        SFXVolume = volume;
        SFXSource.volume = SFXVolume;
    }

    public void setBGMVolume(float volume) {
        BGMVolume = volume;
        BGMSource.volume = BGMVolume;
    }

    public void setAmbienceVolume(float volume) {
        ambienceVolume = volume;
        ambienceSource.volume = ambienceVolume;
    }
    #endregion
}
