using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHitbox : MonoBehaviour {
    // Start is called before the first frame update
    private PlayerCharacter player;
    void Start() {
        player = GetComponentInParent<PlayerCharacter>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Water") {
            if (!player.inWater) AudioManager.Instance.PlayWaterDive();
            player.inWater = true;
            player.canWaterJump = true;
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (col.tag == "Water") player.inWater = true;
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.tag == "Water") player.inWater = false;
    }
}
