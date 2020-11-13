using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayCutscene : MonoBehaviour {
    public PlayableDirector director;
    public PlayableAsset playable;
    // Start is called before the first frame update
    void Start() {
        director = FindObjectOfType<PlayableDirector>();
        director.playableAsset = playable;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            director.Play();
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
