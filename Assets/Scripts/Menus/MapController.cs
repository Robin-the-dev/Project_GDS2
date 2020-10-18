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

    [System.Serializable]
    public class Zone {
        public string key;
        public Image layer;
        public Vector2 position;
        public MapTrigger trigger;

        public void Init() {
            key = layer.name;
        }

        public void enableZone() {
            if (trigger != null) trigger.mapEnabled = true;
            layer.enabled = true;
        }
    }

    public List<Zone> zones;
    public Zone activeZone = null;
    private Canvas canvas;
    public Image currentPosition;

    void Start() {
        // remove me to have map progress saved
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        string[] keys = PlayerPrefs.GetString("FoundMap" + mapId).Split(',');
        MapTrigger[] triggers = FindObjectsOfType<MapTrigger>();
        foreach (Zone z in zones) {
            z.Init();
            foreach (MapTrigger t in triggers) {
                if (t.key == z.key) {
                    z.trigger = t;
                }
            }
        }
        foreach (string key in keys) {
            foreach (Zone z in zones) {
                if (key == z.key) z.enableZone();
            }
        }
    }

    private void FixedUpdate() {
        if (activeZone != null) {
            currentPosition.enabled = true;
            currentPosition.transform.localPosition = activeZone.position;
        } else {
            currentPosition.enabled = false;
        }
    }

    private bool isNotBackground(Image i) {
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

    public Zone getZone(string key) {
        foreach (Zone z in zones) {
            if (z.key == key) return z;
        }
        return null;
    }

    public void UpdateMapPosition(string key) {
        activeZone = getZone(key);
    }

    public void EnableLayer(string layer) {
        List<string> keys = new List<string>(PlayerPrefs.GetString("FoundMap" + mapId).Split(','));
        keys.Add(layer);
        PlayerPrefs.SetString("FoundMap" + mapId, String.Join(",", keys));
        Zone zone = getZone(layer);
        activeZone = zone;
        zone.enableZone();
    }

}
