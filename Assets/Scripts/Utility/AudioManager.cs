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
    public AudioClip fireTorch; // needs to be public so that Lamp object can call this audio clip to play its own audio source
    [SerializeField] private AudioClip rocksFallingInWater;
    [SerializeField] private AudioClip skitteringBugs;
    [SerializeField] private AudioClip waterDrips;
    [SerializeField] private AudioClip waterDropSingle;


    [Header("Menu Sounds")]
    [SerializeField] private AudioClip start;
    [SerializeField] private AudioClip pause;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip levelSelect;
    [SerializeField] private AudioClip leaveGame;


    [Header("BGM Clips")]
    [SerializeField] private AudioClip templeMusic;

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

    #region MenuSounds
    public void PlayStartGame() {
        PlaySFXOneShot(start);
    }

    public void PlayPauseGame() {
        PlaySFXOneShot(pause);
    }

    public void PlayClickButton() {
        PlaySFXOneShot(click);
    }

    public void PlayLevelSelect() {
        PlaySFXOneShot(levelSelect);
    }

    public void PlayLeaveGame() {
        PlaySFXOneShot(leaveGame);
    }
    #endregion

    #region Play Control
    public void StopBGM() {
        BGMSource.Stop();
    }

    #region Music
    public void PlayTempleMusicLoop() {
        PlayBGM(templeMusic);
    }
    #endregion

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
