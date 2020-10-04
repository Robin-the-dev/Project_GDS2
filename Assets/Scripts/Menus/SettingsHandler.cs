using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsHandler : MonoBehaviour {

    [System.Serializable]
    public class Tab {
        public Canvas canvas;
        public Button button;
        public float tabY;
        public float backY;
        private SettingsHandler handler;

        public void Init(SettingsHandler handler) {
            this.handler = handler;
            button.onClick.AddListener(() => OpenTab());
        }

        public void OpenTab() {
            if (!handler.canvas.enabled) return;
            AudioManager.Instance.PlayClickButton();
            handler.CloseAll();
            canvas.enabled = true;
            button.interactable = false;
            handler.selected = this;
            Vector3 positionTab = handler.tabGroup.transform.localPosition;
            Vector3 positionBack = handler.backButton.transform.localPosition;
            handler.tabGroup.transform.localPosition = new Vector3(positionTab.x, tabY, positionTab.z);
            handler.backButton.transform.localPosition = new Vector3(positionBack.x, backY, positionBack.z);
        }
    }

    public List<Tab> tabs = new List<Tab>();
    public Tab selected;
    public HorizontalLayoutGroup tabGroup;
    public Button backButton;

    private Canvas canvas;
    private Canvas lastCanvas;

    void Awake() {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        selected = tabs[0];
        foreach (Tab t in tabs) {
            t.Init(this);
            t.canvas.enabled = false;
        }
    }

    public void OpenSettings(Canvas lastCanvas) {
        this.lastCanvas = lastCanvas;
        GetComponent<Canvas>().enabled = true;
        lastCanvas.enabled = false;
        AudioManager.Instance.PlayClickButton();
        selected.OpenTab();
    }

    private void CloseAll() {
        foreach (Tab t in tabs) {
            t.canvas.enabled = false;
            t.button.interactable = true;
        }
    }

    public void CloseSettings() {
        if (lastCanvas == null) throw new System.InvalidOperationException("Settings menu must not be the first canvas loaded");
        GetComponent<Canvas>().enabled = false;
        lastCanvas.enabled = true;
        CloseAll();
        AudioManager.Instance.PlayClickButton();
    }

}
