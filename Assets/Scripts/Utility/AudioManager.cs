using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
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
    [SerializeField] private AudioSource ambienceOneShotSource;
    [SerializeField] private AudioSource ambienceSource;
    [SerializeField] private AudioSource footStepSource;

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
    [SerializeField] private AudioClip waterDropSingle; // I don't think we need this clip
    [SerializeField] private AudioClip doorLock;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip droppingBox; // Not implemented yet
    [SerializeField] private AudioClip explosion; // I don't think we need this clip
    [SerializeField] private AudioClip gemPickUp;
    [SerializeField] private AudioClip latchButton;
    [SerializeField] private AudioClip loriJumpLanding;
    [SerializeField] private AudioClip underWater;
    [SerializeField] private AudioClip metalLatch;
    [SerializeField] private AudioClip metalLever;
    [SerializeField] private AudioClip switchOnAndOff;
    [SerializeField] private AudioClip waterDive;
    [SerializeField] private AudioClip waterSplash; // I don't think we need this clip, sound is simillar with water dive clip
    [SerializeField] private AudioClip woodenBoxDrop; // Not implemented yet
    [SerializeField] private AudioClip woodenDoorOpen; // Not implemented yet

    [Header("Foot step Clips")]
    [SerializeField] private AudioClip caveWalk;
    [SerializeField] private AudioClip templeRunnning;
    [SerializeField] private AudioClip templeWalk;
    [SerializeField] private AudioClip woodFootstep;

    [Header("Menu Sounds")]
    [SerializeField] private AudioClip start;
    [SerializeField] private AudioClip pause;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip levelSelect;
    [SerializeField] private AudioClip leaveGame;


    [Header("BGM Clips")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip templeMusic;

    private float SFXVolume = 1.0f;
    private float BGMVolume = 1.0f;
    private float ambienceVolume = 1.0f;
    private float footStepVolume = 1.0f;

    #region FootStep
    public void PlayCaveWalk() {
        PlayFootStep(caveWalk);
    }

    public void PlayTempleRunning() {
        PlayFootStep(templeRunnning);
    }

    public void PlayTempleWalk() {
        PlayFootStep(templeWalk);
    }

    public void PlayWoodFootStep() {
        PlayFootStep(woodFootstep);
    }

    public bool isPlayingFootStep() {
        return footStepSource.isPlaying;
    }
    #endregion

    #region BGM
    #endregion

    #region SFX
    public void PlayCaveLevelOpening() {
        PlaySFXOneShot(caveLevelOpening);
    }

    public void PlayGauntletDiscovery() {
        PlaySFXOneShot(gauntletDiscovery);
    }

    public void PlayLoriJumpLanding() {
        PlaySFXOneShot(loriJumpLanding);
    }

    public void PlayWaterDive() {
        PlaySFXOneShot(waterDive);
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

    public void PlayGemPickUp() {
        PlayAmbienceOneShot(gemPickUp);
    }

    public void PlayDoorLock() {
        PlayAmbienceOneShot(doorLock);
    }

    public void PlayDoorOpen() {
        PlayAmbienceOneShot(doorOpen);
    }

    public void PlayLatchButton() {
        PlayAmbienceOneShot(latchButton);
    }

    public void PlayMetalLatch() {
        PlayAmbienceOneShot(metalLatch);
    }

    public void PlayMetalLever() {
        PlayAmbienceOneShot(metalLever);
    }

    public void PlaySwitchOnAndOff() {
        PlayAmbienceOneShot(switchOnAndOff);
    }

    public void PlayUnderWater() {
        PlayAmbience(underWater);
    }

    public bool isPlayingAmbience() {
        return ambienceSource.isPlaying;
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

    public void PlayMenuMusicLoop() {
        PlayBGM(menuMusic);
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
        ambienceOneShotSource.PlayOneShot(ambience, volume);
    }

    private void PlayAmbience(AudioClip ambience) {
        ambienceSource.clip = ambience;
        ambienceSource.loop = true;
        ambienceSource.Play();
    }

    public void StopAmbience() {
        ambienceSource.Stop();
    }

    private void PlayFootStep(AudioClip footStep) {
        footStepSource.clip = footStep;
        footStepSource.loop = true;
        footStepSource.Play();
    }

    public void StopFootStep() {
        footStepSource.Stop();
    }
    #endregion

    public void UpdateVolume() {
        SFXSource.volume = PlayerPrefs.GetFloat("SoundEffectVolume");
        BGMSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        ambienceOneShotSource.volume = PlayerPrefs.GetFloat("AmbientVolume");
        ambienceSource.volume = PlayerPrefs.GetFloat("AmbientVolume");
        footStepSource.volume = PlayerPrefs.GetFloat("SoundEffectVolume");
    }


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
        ambienceOneShotSource.volume = ambienceVolume;
    }

    public void setFootStepVolume(float volume) {
        footStepVolume = volume;
        footStepSource.volume = footStepVolume;
    }
    #endregion
}
