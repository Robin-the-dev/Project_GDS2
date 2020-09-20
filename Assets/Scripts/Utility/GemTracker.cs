using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GemTracker : MonoBehaviour {
    // Start is called before the first frame update
    Gem[] gems;
    void Start() {
        gems = FindObjectsOfType<Gem>();
        int id = 0;
        string sceneName = SceneManager.GetActiveScene().name;
        foreach (Gem g in gems) {
            g.id = sceneName + "Gem" + id + "-" + g.score;
            id++;
        }
        string[] keys = PlayerPrefs.GetString("GemTracker").Split(',');
        foreach (string k in keys) {
            foreach (Gem g in gems) {
                if (g.id == k) {
                    Destroy(g.gameObject);
                }
            }
        }
    }

    public void TrackGem(string id) {
        List<string> keys = new List<string>(PlayerPrefs.GetString("GemTracker").Split(','));
        keys.Add(id);
        PlayerPrefs.SetString("GemTracker", String.Join(",", keys));
    }
}
