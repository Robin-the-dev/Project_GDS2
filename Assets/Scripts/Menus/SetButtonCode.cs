using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetButtonCode : MonoBehaviour {
    public ButtonControl setting;
    public ButtonControl settingAlt;
    public SelectionController controller;
    public string code;
    // Start is called before the first frame update
    private void Awake() {
        setting.selectionCode = code;
        settingAlt.selectionCode = code + "Alt";
        setting.controller = controller;
        settingAlt.controller = controller;
    }
}
