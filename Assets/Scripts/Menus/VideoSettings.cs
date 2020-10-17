using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoSettings : MonoBehaviour {

    private Canvas canvas;
    public Slider sliderUIScale;
    public TextMeshProUGUI sliderValue;
    public TMP_Dropdown dropdownMenu;
    public Toggle toggleFullScreen;
    private Resolution[] resolutions;
    // Start is called before the first frame update
    void Start() {
        resolutions = Screen.resolutions;
        toggleFullScreen.isOn = Screen.fullScreen;
        InitDropdown();

        canvas = GetComponent<Canvas>();
        sliderValue = sliderUIScale.GetComponentInChildren<TextMeshProUGUI>();
        sliderUIScale.value = PlayerPrefs.GetFloat("UIScalePosition", 3);
        sliderValue.text = PlayerPrefs.GetFloat("UIScale") + "x";
        UpdateSlider();
    }

    private string ResToString(Resolution res) {
        return res.width + " x " + res.height + " (" + res.refreshRate + ")";
    }

    private void InitDropdown() {
        int i;
        for (i = 0; i < resolutions.Length; i++) {
            dropdownMenu.options.Add(new TMP_Dropdown.OptionData(ResToString(resolutions[i])));
        }
        dropdownMenu.value = PlayerPrefs.GetInt("ResolutionValue", i);
    }

    public void UpdateResolution() {
        Screen.SetResolution(resolutions[dropdownMenu.value].width, resolutions[dropdownMenu.value].height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionValue", dropdownMenu.value);
    }

    public void UpdateFullScreen() {
         Screen.fullScreen = toggleFullScreen.isOn;
    }

    public void UpdateSlider() {
        if (!canvas.enabled) return;
        float scale = 1;
        switch(sliderUIScale.value) {
            case 1:
                scale = 0.5f;
            break;
            case 2:
                scale = 0.75f;
            break;
            case 3:
                scale = 1;
            break;
            case 4:
                scale = 1.5f;
            break;
            case 5:
                scale = 2;
            break;
        }
        sliderValue.text = scale + "x";
        PlayerPrefs.SetFloat("UIScale", scale);
        PlayerPrefs.SetFloat("UIScalePosition", sliderUIScale.value);
        UIScaler[] list = FindObjectsOfType<UIScaler>();
        foreach (UIScaler s in list) {
            s.UpdateUIScale();
        }
    }
}
