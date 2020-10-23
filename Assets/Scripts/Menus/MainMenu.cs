using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public SettingsHandler settings;
    public LevelSelect levelSelect;

    void Start() {
        GetComponent<Canvas>().enabled = true;
        AudioManager.Instance.PlayMenuMusicLoop();
    }

    public void OpenSettings(){
        settings.OpenSettings(GetComponent<Canvas>());
    }

    public void OpenLevelSelect(){
        levelSelect.OpenLevelSelect(GetComponent<Canvas>());
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void StartGame() {
        AudioManager.Instance.PlayLevelSelect();
        SceneManager.LoadScene("Level_1");
    }

    public void ResetGems() {
        AudioManager.Instance.PlayStartGame();
        PlayerPrefs.SetString("GemTracker", "");
        PlayerPrefs.SetFloat("Score", 0f);
        PlayerPrefs.SetString("FoundMapLevel_1", "");
        PlayerPrefs.SetString("FoundMapLevel_2", "");
        PlayerPrefs.SetInt("AllowMagic", 0);
        PlayerPrefs.SetInt("EnableLevel2", 0);
    }
}
