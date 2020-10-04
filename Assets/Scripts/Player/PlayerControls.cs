using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour {

    public bool SoloMode;
    public PlayerCharacter activeCharacter;
    public PixieCharacter pixieCharacter;
    public Vector3 mousePosition = Vector3.zero;
    private Camera mainCamera;
    public PauseMenu pauseMenu;

    private Transform target = null;

    [HideInInspector]
    public Image primaryAbilityIcon;
    [HideInInspector]
    public Image secondaryAbilityIcon;
    [HideInInspector]
    public Text primaryCoolDownText;
    [HideInInspector]
    public Text secondaryCoolDownText;

    private bool pixieMode = false;

    private float pixieHeld = 0;
    private bool paused = false;

    public void Awake() {
        //reset keys when reload.
        mainCamera = Camera.main;
        PlayerPrefs.SetInt("keyCount", 0);
        // InitAbilityIcons();
    }

    private void Update() {
        updateMousePos();
        pixieCharacter.UpdatePosition(mousePosition);
        // handle game pauses
        HandlePause();
        if (Time.timeScale > 0) {
            SwitchMode();
            if (pixieMode) {
                HandlePixieControls();
            } else {
                HandleControls();
            }
        }
    }

    private void HandlePause() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            paused = Time.timeScale == 0;
            if (paused) {
                pauseMenu.ClosePause();    
            } else {
                pauseMenu.OpenPause();
            }
        }
    }

    private void SwitchMode() {
        if (GetKey("Maki")) {
            pixieHeld += Time.deltaTime;
        } else if (GetKeyUp("Maki")) {
            if (pixieHeld >= 0.3) {
                pixieCharacter.WarpHome(activeCharacter);
                pixieMode = false;
            } else {
                pixieMode = !pixieMode;
            }
            pixieCharacter.UpdatePixieMode(pixieMode);
            ChangeCamera();
            if (pixieMode) {
                CancelControls();
            }
            pixieHeld = 0;
        }
    }

    private void CancelControls() {
        activeCharacter.ClimbUp(false);
        activeCharacter.ClimbDown(false);
        activeCharacter.MoveLeft(false);
        activeCharacter.MoveRight(false);
        activeCharacter.HoldJump(false);
        activeCharacter.HoldInteract(false);
        activeCharacter.Sprint(false);
        activeCharacter.doAttackAction(false, mousePosition);
        activeCharacter.doSpecialAction(false, mousePosition);
    }

    private void HandlePixieControls() {
        //interact
        if (GetKeyDown("Interact")) {
            pixieCharacter.Interact();
        }

        if (GetKeyDown("MoveUp")) {
            pixieCharacter.MoveUp(true);
        } else if (GetKeyUp("MoveUp")) {
            pixieCharacter.MoveUp(false);
        }

        //climbDown
        if (GetKeyDown("MoveDown")) {
            pixieCharacter.MoveDown(true);
        } else if (GetKeyUp("MoveDown")) {
            pixieCharacter.MoveDown(false);
        }

        //moveLeft
        if (GetKeyDown("MoveLeft")) {
            pixieCharacter.MoveLeft(true);
        } else if(GetKeyUp("MoveLeft")) {
            pixieCharacter.MoveLeft(false);
        }

        //moveRight
        if (GetKeyDown("MoveRight")) {
            pixieCharacter.MoveRight(true);
        } else if(GetKeyUp("MoveRight")) {
            pixieCharacter.MoveRight(false);
        }
    }

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
            activeCharacter.HoldInteract(true);
        } else if (GetKeyUp("Interact")) {
            activeCharacter.HoldInteract(false);
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

    private void ChangeCamera() {
        if (pixieMode) {
            target = pixieCharacter.transform;
        } else {
            target = activeCharacter.transform;
        }
        StartCoroutine(Transition());
    }

    IEnumerator Transition() {
        float t = 0.0f;
        float transitionDuration = 0.25f;
        Vector3 startingPos = mainCamera.transform.position;
        while (t < 1.0f) {
             t += Time.deltaTime * (Time.timeScale/transitionDuration);
             Vector3 newPos = new Vector3(target.position.x, target.position.y, startingPos.z);
             mainCamera.transform.position = Vector3.Lerp(startingPos, newPos, t);
             yield return 0;
        }
        mainCamera.transform.SetParent(target);
    }

    private void updateMousePos() {
        Rect screen = new Rect(0, 0, Screen.width, Screen.height);
        if (!screen.Contains(Input.mousePosition)) return;

        Vector3 tempPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(tempPos);
        mousePosition = new Vector3(worldPosition.x, worldPosition.y, 0);
    }
}
