using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private float score;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        score = PlayerPrefs.GetFloat("Score");
    }

    // Update is called once per frame
    void Update()
    {
        score = PlayerPrefs.GetFloat("Score");
        scoreText.SetText("Score: {0:0}", score);
    }
}
