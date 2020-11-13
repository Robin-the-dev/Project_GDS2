using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHook : MonoBehaviour {

    public GameObject grapplePrefab;
    public Vector3 grapplePos;
    public GameObject grapple;

    void OnEnable() {
        grapple = Instantiate(grapplePrefab, grapplePos, Quaternion.identity);
        grapple.layer = 0;
    }
}
