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
    private bool isDived;
    private bool isLanded;
    private float bugSoundTimeLeft;
    private float dripSoundTimeLeft;
    private float windSoundTimeLeft;

    private void Awake() {
        instance = this; // Singleton
    }

    // Start is called before the first frame update
    void Start() {
        state = State.Idle;
        playerLife = 3;
        score = 0.0f;
        multiplierForBugSound = 1;
        multiplierForWaterDripSound = 1;
        isDived = false;
        isLanded = false;
        bugSoundTimeLeft = Random.Range(14, 65);
        dripSoundTimeLeft = Random.Range(0.5f, 5);
    }

    private void PlayAmbience() {
        bugSoundTimeLeft -= Time.deltaTime;
        dripSoundTimeLeft -= Time.deltaTime;

        if (bugSoundTimeLeft <= 0) {
            bugSoundTimeLeft = Random.Range(14, 65);
            AudioManager.Instance.PlaySkitteringBugs();
        }
        if (dripSoundTimeLeft <= 0) {
            dripSoundTimeLeft = Random.Range(1, 10);
            AudioManager.Instance.PlayWaterDrip();
        }
    }

    // Update is called once per frame
    void Update() {
        PlayAmbience();

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

    public void AddScore(float score) {
        this.score += score;
    }


}
