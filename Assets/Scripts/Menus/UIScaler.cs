using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScaler : MonoBehaviour {
    CanvasScaler canvasScaler;
    // Start is called before the first frame update
    void Start() {
        canvasScaler = GetComponent<CanvasScaler>();
        UpdateUIScale();
    }

    // Update is called once per frame
    public void UpdateUIScale() {
        canvasScaler.scaleFactor = PlayerPrefs.GetFloat("UIScale", 1) / 2;
    }
}
