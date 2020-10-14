using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class MapController : MonoBehaviour {
    // Start is called before the first frame update

    private static MapController _instance;
    public static MapController Instance { get { return _instance; } }

    public string mapId = "Level_2";

    private void Awake() {
        if(_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
        }
    }

    public List<Image> layers;
    private Canvas canvas;

    bool isNotBackground(Image i) {
        return i.name != "Image";
    }

    public void EnableMap() {
        canvas.enabled = true;
    }

    public void DisableMap() {
        canvas.enabled = false;
    }

    public void ToggleMap() {
        canvas.enabled = !canvas.enabled;
    }

    void Start() {
        // remove me to have map progress saved
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        PlayerPrefs.SetString("FoundMap" + mapId, "");
        layers = Array.FindAll(GetComponentsInChildren<Image>(), isNotBackground).ToList();
        string[] keys = PlayerPrefs.GetString("FoundMap" + mapId).Split(',');
        foreach (string k in keys) {
            foreach (Image i in layers) {
                if (k == i.name) {
                    i.enabled = true;
                }
            }
        }
    }

    public void EnableLayer(string layer) {
        List<string> keys = new List<string>(PlayerPrefs.GetString("FoundMap" + mapId).Split(','));
        keys.Add(layer);
        PlayerPrefs.SetString("FoundMap" + mapId, String.Join(",", keys));
        foreach (Image i in layers) {
            if (i.name == layer) i.enabled = true;
        }
    }

}
