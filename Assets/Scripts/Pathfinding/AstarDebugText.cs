using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AstarDebugText : MonoBehaviour {
    public RectTransform arrow;
    public TextMeshProUGUI scoreG, scoreH, scoreF;

    public void SetScores(int scoreG, int scoreH, int scoreF) {
        this.scoreG.text = scoreG + "";
        this.scoreH.text = scoreH + "";
        this.scoreF.text = scoreF + "";
    }
}
