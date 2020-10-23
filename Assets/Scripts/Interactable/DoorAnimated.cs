using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimated : LinkedObject {
    public bool isOpen;
    private Animator anim;
    private AudioSource source;
    private SpriteRenderer sprite;
    private BoxCollider2D collider;

    public bool isBig = false;

    private AudioClip scrape;
    private AudioClip bang;
    private AudioClip open;

    bool playSound = false;
    float wait = 3;

    // Start is called before the first frame update
    void Awake() {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        source = GetComponent<AudioSource>();
        scrape = Resources.Load<AudioClip>("Sounds/SoundEffects/Scrape Stone");
        bang = Resources.Load<AudioClip>("Sounds/SoundEffects/Rumble");
        open = Resources.Load<AudioClip>("Sounds/SoundEffects/Cave Level Opening");
        if (isOpen) openDoor();
    }

    public void Update(){
        if (wait > 0) {
            wait-= Time.deltaTime;
        } else {
            playSound = true;
        }
    }

    public override void Activate(string msg) {
        openDoor();
    }

    public override void Deactivate(string msg) {
        closeDoor();
    }

    public void DoorOpened() {
        if (isBig) {
            source.volume = PlayerPrefs.GetFloat("SoundEffectVolume");
            if (playSound) source.PlayOneShot(open);
        }
    }

    public void DoorClosed() {
        if (isBig) {
            source.volume = PlayerPrefs.GetFloat("SoundEffectVolume");
            if (playSound) source.PlayOneShot(bang);
        }
    }

    private void openDoor() {
        if (isOpen) return;
        isOpen = true;
        anim.Play("Open");
        source.volume = PlayerPrefs.GetFloat("SoundEffectVolume");
        if (playSound) source.PlayOneShot(scrape);
    }

    private void closeDoor() {
        if (!isOpen) return;
        isOpen = false;
        anim.Play("Close");
        source.volume = PlayerPrefs.GetFloat("SoundEffectVolume");
        if (playSound) source.PlayOneShot(scrape);
    }
}
