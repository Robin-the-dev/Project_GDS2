using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton

    enum State {Idle, Walk, Jump, Swim, Dead}; // Put additional state you want

    State state;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite damagedHeartSprite;
    public PlayerCharacter playerCharacter;
    private int playerLife;

    [HideInInspector] public float score;

    // Below variables are to manage sound
    private float timeCounter;
    private int multiplierForBugSound;
    private int multiplierForWaterDripSound;
    private int multiplierForRockSound;
    private bool isDived;
    private bool isLanded;

    private void Awake() {
        instance = this; // Singleton
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        playerLife = 3;
        score = 0.0f;
        timeCounter = 0.0f;
        multiplierForBugSound = 1;
        multiplierForWaterDripSound = 1;
        multiplierForRockSound = 1;
        isDived = false;
        isLanded = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;

        if (playerLife == 0) {
            state = State.Dead; // Player dies when life is zero
        }

        if(state == State.Dead) {
            // If player is dead, do something here
        }

        if (!playerCharacter.moveLeft && !playerCharacter.moveRight) {
            state = State.Idle;
        }

        if (playerCharacter.moveLeft || playerCharacter.moveRight) {
            state = State.Walk;
        }

        if(!playerCharacter.IsGrounded() && !playerCharacter.inWater) {
            state = State.Jump;
        }

        if(playerCharacter.inWater) {
            state = State.Swim;
        }

        PlayAmbience();

        if (state == State.Idle) {
            if(!isLanded) {
                AudioManager.Instance.PlayLoriJumpLanding();
                isLanded = true;
            }
        }

        if(state == State.Walk) {
            if (!AudioManager.Instance.isPlayingFootStep()) {
                AudioManager.Instance.PlayCaveWalk();
            }
            if(!isLanded) {
                AudioManager.Instance.PlayLoriJumpLanding();
                isLanded = true;
            }
        }
        else {
            if (AudioManager.Instance.isPlayingFootStep()) {
                AudioManager.Instance.StopFootStep();
            }
        }

        if(state == State.Jump) {
            isLanded = false;
        }

        if(state == State.Swim) {
            if (!AudioManager.Instance.isPlayingAmbience()) {
                AudioManager.Instance.PlayUnderWater();
            }
            if(!isDived) {
                AudioManager.Instance.PlayWaterDive();
                isDived = true;
            }
        }
        else {
            if (AudioManager.Instance.isPlayingAmbience()) {
                AudioManager.Instance.StopAmbience();
            }

            isDived = false;
        }
    }

    public void Damage() {
        if(playerLife != 0) {
            playerLife--;
        }
        hearts[playerLife].sprite = damagedHeartSprite;
    }

    public void AddScore(float score)
    {
        this.score += score;
    }

    private void PlayAmbience() {
        if(timeCounter >= 11.0f * multiplierForBugSound) {
            AudioManager.Instance.PlaySkitteringBugs();
            multiplierForBugSound++;
        }

        if(timeCounter >= 27.0f * multiplierForRockSound) {
            AudioManager.Instance.PlayRocksFallingInWater();
            multiplierForRockSound++;
        }

        if(timeCounter >= 54.0f * multiplierForWaterDripSound) {
            AudioManager.Instance.PlayWaterDrips();
            multiplierForWaterDripSound++;
        }
    }
}
