using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeStone : MonoBehaviour {
    // Start is called before the first frame update
    public int val;
    public Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
        val = Random.Range(1, 11);
        anim.Play("Glyph_" + val);
    }

    void NextGlyph() {
        val++;
        if (val > 10) val = 1;
        anim.Play("Glyph_" + val);
    }
}
