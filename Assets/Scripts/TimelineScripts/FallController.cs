using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallController : MonoBehaviour {

    public PlayerCharacter player;
    public CreateHook createHook;

    void OnEnable() {
        createHook.grapple.layer = 10;
        Debug.Log("UseHook");
        StartCoroutine(UseHook());
    }

    IEnumerator UseHook() {
        player.doSpecialAction(true, createHook.grapplePos);
        yield return new WaitForSeconds(0.4f);
        AudioManager.Instance.PlayRocksFallingInWater();
        yield return new WaitForSeconds(0.6f);
        player.doSpecialAction(false, createHook.grapplePos);
    }
}
