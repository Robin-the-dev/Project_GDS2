using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RandomLightSprite : MonoBehaviour {

    [System.Serializable]
    public struct LightSprite {
        public List<Sprite> sprites;
        public Color color;
    }

    public List<LightSprite> lightSprites = new List<LightSprite>();
    private SpriteRenderer render;
    private Light2D light;

    void Start() {
        render = GetComponent<SpriteRenderer>();
        light = GetComponentInChildren<Light2D>();
        int randomColor = Random.Range(0, lightSprites.Count);
        int randomSprite = Random.Range(0, lightSprites[randomColor].sprites.Count);
        render.sprite = lightSprites[randomColor].sprites[randomSprite];
        light.color = lightSprites[randomColor].color;
    }

}
