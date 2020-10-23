using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeHandler : LinkedObject {

    private bool activated = false;
    private bool opening = true;
    private Animator anim;
    private AudioSource source;

    private AudioClip scrape;
    private AudioClip bang;

    private bool playSound = false;

    float wait = 0.5f;

    public void Start() {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        scrape = Resources.Load<AudioClip>("Sounds/SoundEffects/Scrape Stone");
        bang = Resources.Load<AudioClip>("Sounds/SoundEffects/Rumble");
    }

    public override void Activate(string msg) {
        if (activated) {
            anim.Play("Close_Bridge");
        } else {
            anim.Play("Open_Bridge");
        }
        source.volume = PlayerPrefs.GetFloat("SoundEffectVolume");
        if (playSound) source.PlayOneShot(scrape);

    }

    public override void Deactivate(string msg) {
        activated = !activated;
    }

    public void PlayBang() {
        source.volume = PlayerPrefs.GetFloat("SoundEffectVolume");
        if (playSound) source.PlayOneShot(bang);
    }

    public void Update(){
        if (wait > 0) {
            wait-= Time.deltaTime;
        } else {
            playSound = true;
        }
    }
}
