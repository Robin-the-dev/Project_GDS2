using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton

    enum State {Idle, Dead}; // Put additional state you want

    State state;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite damagedHeartSprite;
    private int playerLife;

    private void Awake() {
        instance = this; // Singleton
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        playerLife = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerLife == 0) {
            state = State.Dead; // Player dies when life is zero
        }

        if(state == State.Dead) {
            // If player is dead, do something here
        }
    }

    public void Damage() {
        if(playerLife != 0) {
            playerLife--;
        }
        hearts[playerLife].sprite = damagedHeartSprite;
    }
}
