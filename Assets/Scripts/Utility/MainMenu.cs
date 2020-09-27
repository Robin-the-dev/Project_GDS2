using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public SettingsHandler settings;

    void Start() {
        GetComponent<Canvas>().enabled = true;
        AudioManager.Instance.PlayTempleMusicLoop();
    }

    public void OpenSettings(){
        settings.OpenSettings(GetComponent<Canvas>());
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
    }
}
