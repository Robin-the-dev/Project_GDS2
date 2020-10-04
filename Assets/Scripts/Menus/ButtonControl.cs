using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonControl : MonoBehaviour {
    // Start is called before the first frame update
    public string selectionCode;
    public SelectionController controller;
    public Button button;
    public TextMeshProUGUI text;

    public void SelectKey() {
        controller.Select(selectionCode);
    }
}
