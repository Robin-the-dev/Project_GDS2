using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Gem : MonoBehaviour {
    public float score = 10;
    private Light2D light;
    public Color color1;
    public Color color2;
    public float flickerDuration = 1f;
    [HideInInspector]
    public string id;

    void Start() {
        light = GetComponentInChildren<Light2D>();
    }

    void Update() {
        float t = Mathf.PingPong(Time.time, flickerDuration) / flickerDuration;
        light.color = Color.Lerp(color1, color2, t);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Pixie") {
            PlayerPrefs.SetFloat("Score", PlayerPrefs.GetFloat("Score") + score);
            FindObjectOfType<GemTracker>().TrackGem(id);
            AudioManager.Instance.PlayGemPickUp(); 
            Destroy(gameObject);
        }
    }
}
