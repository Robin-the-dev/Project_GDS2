using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeHandler : LinkedObject {

    private float startPosX;
    public float endPosX;
    public float lerpSpeed = 0.5f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool activated = false;
    private bool opening = true;

    public override void Activate(string msg) {
        activated = true;
        StartCoroutine(Transition());
    }

    public override void Deactivate(string msg) {
        activated = false;
        opening = !opening;
    }

    void Start() {
        startPosX = transform.position.x;
        startPosition = new Vector3(startPosX, transform.position.y, transform.position.z);
        endPosition = new Vector3(endPosX, transform.position.y, transform.position.z);
    }

    IEnumerator Transition() {
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        while (t < 1.0f) {
            t += Time.deltaTime * (Time.timeScale/lerpSpeed);
            if (opening) {
                transform.position = Vector3.Lerp(startingPos, endPosition, t);
            } else {
                transform.position = Vector3.Lerp(startingPos, startPosition, t);
            }
            yield return 0;
        }
    }
}
