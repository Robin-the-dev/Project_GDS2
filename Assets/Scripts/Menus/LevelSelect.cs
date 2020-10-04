using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

    private Canvas lastCanvas;

    void Awake() {
        GetComponent<Canvas>().enabled = false;
    }

    public void OpenLevelSelect(Canvas lastCanvas) {
        this.lastCanvas = lastCanvas;
        GetComponent<Canvas>().enabled = true;
        lastCanvas.enabled = false;
        AudioManager.Instance.PlayClickButton();
    }

    public void CloseLevelSelect() {
        if (lastCanvas == null) throw new System.InvalidOperationException("Settings menu must not be the first canvas loaded");
        GetComponent<Canvas>().enabled = false;
        lastCanvas.enabled = true;
        AudioManager.Instance.PlayClickButton();
    }

    public void SelectLevel1(){
        AudioManager.Instance.PlayLevelSelect();
        SceneManager.LoadScene("Level_1");
    }

    public void SelectLevel2(){
        AudioManager.Instance.PlayLevelSelect();
        SceneManager.LoadScene("Level_2");
    }

}
