using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCodeButton : InteractableObject {

    public CodeStone stone;
    public CodeStone linkedStone;
    public Animator anim;

    public override void Start() {
        base.Start();
        anim = GetComponent<Animator>();
    }

    public override void Interact() {
        stone.NextGlyph();
        anim.Play("Activate");
        AudioManager.Instance.PlayClickButton();
        CheckState(true);
    }

    public override void CheckState(bool playSound) {
        if (stone == null || linkedStone == null) return;
        if (obj == null) return;
        if (stone.val == linkedStone.val) {
            obj.Activate(key);
        } else {
            obj.Deactivate(key);
        }
        if (playSound) AudioManager.Instance.PlaySwitchOnAndOff();
    }
}
