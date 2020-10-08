using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour {

    public List<Sprite> sprites = new List<Sprite>();
    private SpriteRenderer render;

    void Start() {
        render = GetComponent<SpriteRenderer>();
        render.sprite = sprites[Random.Range(0, sprites.Count)];
    }

}
