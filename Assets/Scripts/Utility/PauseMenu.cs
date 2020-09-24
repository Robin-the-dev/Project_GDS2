using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {


    public void Quit() {
        Application.Quit();
    }

    public void MainMenu() {
        AudioManager.Instance.PlayLeaveGame();
        SceneManager.LoadScene("Main_Menu");
    }
}
