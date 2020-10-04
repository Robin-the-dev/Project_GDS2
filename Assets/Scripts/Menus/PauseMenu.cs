using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public SettingsHandler settings;
    private bool inSubMenu = false;
    public Canvas canvas;

    public void Quit() {
        Application.Quit();
    }

    public void Start() {
        canvas = GetComponent<Canvas>();
    }

    public void OpenPause() {
        Time.timeScale = 0;
        canvas.enabled = true;
        AudioManager.Instance.PlayPauseGame();
    }

    public void ClosePause() {
        if (inSubMenu) {
            settings.CloseSettings();
        }else {
            Time.timeScale = 1;
            canvas.enabled = false;
            AudioManager.Instance.PlayPauseGame();
        }
    }

    public void Update() {
        inSubMenu = Time.timeScale == 0 && !canvas.enabled;
    }

    public void MainMenu() {
        AudioManager.Instance.PlayLeaveGame();
        SceneManager.LoadScene("Main_Menu");
    }

    public void OpenSettings() {
        settings.OpenSettings(GetComponent<Canvas>());
    }
}
