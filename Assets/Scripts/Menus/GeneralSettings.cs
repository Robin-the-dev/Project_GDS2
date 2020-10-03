using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneralSettings : MonoBehaviour {

    public TMP_Dropdown dropdown;

    void Awake() {
        dropdown.value = PlayerPrefs.GetInt("TextSpeedDropdown", 1);
    }

    public void UpdateTextSpeed() {
        PlayerPrefs.SetInt("TextSpeedDropdown", dropdown.value);
        AudioManager.Instance.PlayClickButton();
        switch (dropdown.value) {
            case 0:
                PlayerPrefs.SetFloat("TextSpeed", 2f);
                break;
            case 1:
                PlayerPrefs.SetFloat("TextSpeed", 1f);
                break;
            case 2:
                PlayerPrefs.SetFloat("TextSpeed", 0.5f);
                break;
            case 3:
                PlayerPrefs.SetFloat("TextSpeed", 0.25f);
                break;
            case 4:
                PlayerPrefs.SetFloat("TextSpeed", 0f);
                break;
        }
    }
}
