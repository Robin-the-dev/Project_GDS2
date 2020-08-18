using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour{

    public bool SoloMode;
    public PlayerCharacter activeCharacter;
    public Vector3 mousePosition = Vector3.zero;
    private Camera mainCamera;

    [HideInInspector]
    public Image primaryAbilityIcon;
    [HideInInspector]
    public Image secondaryAbilityIcon;
    [HideInInspector]
    public Text primaryCoolDownText;
    [HideInInspector]
    public Text secondaryCoolDownText;

    public void Awake() {
        //reset keys when reload.
        mainCamera = Camera.main;
        PlayerPrefs.SetInt("keyCount", 0);
        // InitAbilityIcons();
    }

    private void Update() {
        updateMousePos();
        if (Time.timeScale > 0) {
            HandleControls();
            // HandleCoolDowns();
        }
    }

    // private void InitAbilityIcons() {
    //     if (primaryAbilityIcon != null && secondaryAbilityIcon != null) return;
    //     primaryAbilityIcon = FindObjectOfType<PrimaryAbilityIcon>().GetComponent<Image>();
    //     secondaryAbilityIcon = FindObjectOfType<SecondaryAbilityIcon>().GetComponent<Image>();
    //     primaryCoolDownText = primaryAbilityIcon.GetComponentInChildren<Text>();
    //     secondaryCoolDownText = secondaryAbilityIcon.GetComponentInChildren<Text>();
    // }

    private void HandleControls() {
        //Jump controls
        if (GetKeyDown("Jump")) {
            activeCharacter.Jump();
            activeCharacter.HoldJump(true);
        } else if (GetKeyUp("Jump")) {
            activeCharacter.HoldJump(false);
        }

        //interact
        if (GetKeyDown("Interact")) {
            activeCharacter.Interact();
        }

        //climbUp
        if (GetKeyDown("MoveUp")) {
            // activeCharacter.EnterDoor();
            activeCharacter.ClimbUp(true);
        } else if (GetKeyUp("MoveUp")) {
            activeCharacter.ClimbUp(false);
        }

        //climbDown
        if (GetKeyDown("MoveDown")) {
            activeCharacter.ClimbDown(true);
        } else if (GetKeyUp("MoveDown")) {
            activeCharacter.ClimbDown(false);
        }

        //moveLeft
        if (GetKeyDown("MoveLeft")) {
            activeCharacter.MoveLeft(true);
        } else if(GetKeyUp("MoveLeft")) {
            activeCharacter.MoveLeft(false);
        }

        //moveRight
        if (GetKeyDown("MoveRight")) {
            activeCharacter.MoveRight(true);
        } else if(GetKeyUp("MoveRight")) {
            activeCharacter.MoveRight(false);
        }

        //sprint
        if (GetKeyDown("Sprint")) {
            activeCharacter.Sprint(true);
        } else if(GetKeyUp("Sprint")) {
            activeCharacter.Sprint(false);
        }

        //primaryAbility
        if (GetKeyDown("Attack")) {
            activeCharacter.doAttackAction(true, mousePosition);
        } else if (GetKeyUp("Attack")) {
            activeCharacter.doAttackAction(false, mousePosition);
        }

        // secondaryAbility
        if (GetKeyDown("Ability1")) {
            activeCharacter.doSpecialAction(true, mousePosition);
        } else if (GetKeyUp("Ability1")) {
            activeCharacter.doSpecialAction(false, mousePosition);
        }
    }

    // public void HandleCoolDowns() {
    //     // do gun cooldown
    //     InitAbilityIcons();
    //     if (activeCharacter.primaryTimeLeft > 0) {
    //         primaryAbilityIcon.color = new Color32(255, 255, 255, 100);
    //         primaryCoolDownText.enabled = true;
    //         primaryCoolDownText.text = activeCharacter.primaryTimeLeft.ToString("F1");
    //     } else {
    //         primaryAbilityIcon.color = new Color32(255, 255, 255, 255);
    //         primaryCoolDownText.enabled = false;
    //     }
    //     // do lasoo cooldown
    //     if (activeCharacter.secondaryTimeLeft > 0) {
    //         secondaryAbilityIcon.color = new Color32(255, 255, 255, 100);
    //         secondaryCoolDownText.enabled = true;
    //         secondaryCoolDownText.text = activeCharacter.secondaryTimeLeft.ToString("F1");
    //     } else {
    //         secondaryAbilityIcon.color = new Color32(255, 255, 255, 255);
    //         secondaryCoolDownText.enabled = false;
    //     }
    // }

    private bool IsKeyCode(string key, KeyCode code) {
        return ControlSettings.Instance.get(key) == code;
    }

    private bool GetKeyUp(string code) {
        bool isActive = Input.GetKeyUp(ControlSettings.Instance.get(code)) || Input.GetKeyUp(ControlSettings.Instance.get(code + "Alt"));
        return isActive;
    }

    private bool GetKeyDown(string code) {
        bool isActive = Input.GetKeyDown(ControlSettings.Instance.get(code)) || Input.GetKeyDown(ControlSettings.Instance.get(code + "Alt"));
        return isActive;
    }

    private bool GetKey(string code) {
        return Input.GetKey(ControlSettings.Instance.get(code)) || Input.GetKey(ControlSettings.Instance.get(code + "Alt"));
    }

    private void updateMousePos() {
        if (activeCharacter == null || Camera.main == null) return;
        Vector3 tempPos = Input.mousePosition;
        tempPos.z = activeCharacter.transform.position.z - Camera.main.transform.position.z;
        tempPos = Camera.main.ScreenToWorldPoint(tempPos, Camera.MonoOrStereoscopicEye.Mono);
        mousePosition = new Vector3(tempPos.x, tempPos.y, activeCharacter.transform.position.z);
    }
}
