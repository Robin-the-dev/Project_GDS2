using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatedCharacter : MonoBehaviour {

    [HideInInspector]
    public Animator anim;

    [HideInInspector]
    protected int blinkWaitTime = 90;
    [HideInInspector]
    protected int maxBlinkWaitTime = 80;
    [HideInInspector]
    protected int minBlinkWaitTime = 200;
    [HideInInspector]
    protected int blinkTime = 0;

    public virtual void Start() {
        anim = GetComponent<Animator>();
    }

    public virtual void FixedUpdate() {
        HandleBlink();
    }

    public void HandleBlink() {
        // enable blink layer
        if (blinkTime > 0) {
            anim.SetLayerWeight(anim.GetLayerIndex("Blinking"),1);
        } else {
            anim.SetLayerWeight(anim.GetLayerIndex("Blinking"),0);
        }
        // do blink cooldown
        if (blinkWaitTime <= 0 && blinkTime <= 0) {
            // get new cooldown
            blinkWaitTime = Random.Range(minBlinkWaitTime, maxBlinkWaitTime);
            blinkTime = 8;
        } else if (blinkTime > 0){
            blinkTime--;
        } else {
            blinkWaitTime--;
        }
    }

}
