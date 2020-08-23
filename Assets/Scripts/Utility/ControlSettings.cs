using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ControlSettings : MonoBehaviour{

    public static ControlSettings Instance { get; private set; }

    public KeyCode inputOpenMenu = KeyCode.Escape;

    private Dictionary<string, KeyCode> defaultKeys = new Dictionary<string, KeyCode>();
    private Dictionary<KeyCode, string> keyOverride = new Dictionary<KeyCode, string>();
    private string selected;
    private bool selecting;

    void Start() {
        // Movement
        defaultKeys.Add("MoveUp", KeyCode.W);
        defaultKeys.Add("MoveUpAlt", KeyCode.UpArrow);
        defaultKeys.Add("MoveDown", KeyCode.S);
        defaultKeys.Add("MoveDownAlt", KeyCode.DownArrow);
        defaultKeys.Add("MoveLeft", KeyCode.A);
        defaultKeys.Add("MoveLeftAlt", KeyCode.LeftArrow);
        defaultKeys.Add("MoveRight", KeyCode.D);
        defaultKeys.Add("MoveRightAlt", KeyCode.RightArrow);
        defaultKeys.Add("Jump", KeyCode.Space);
        defaultKeys.Add("JumpAlt", KeyCode.None);
        defaultKeys.Add("Sprint", KeyCode.LeftShift);
        defaultKeys.Add("SprintAlt", KeyCode.RightShift);
        // Game Controls
        defaultKeys.Add("Interact", KeyCode.E);
        defaultKeys.Add("InteractAlt", KeyCode.I);
        // Combat
        defaultKeys.Add("Attack", KeyCode.Mouse0);
        defaultKeys.Add("AttackAlt", KeyCode.None);
        defaultKeys.Add("Ability1", KeyCode.Mouse1);
        defaultKeys.Add("Ability1Alt", KeyCode.None);
        defaultKeys.Add("Ability2", KeyCode.Q);
        defaultKeys.Add("Ability2Alt", KeyCode.None);
        // Camera
        defaultKeys.Add("CameraReset", KeyCode.LeftControl);
        defaultKeys.Add("CameraResetAlt", KeyCode.RightControl);

        InitKeys(false);
        AddOverrides();
    }

    public void InitKeys(bool isOverwriting) {
        foreach (KeyValuePair<string, KeyCode> k in defaultKeys) {
            string prefKey = "KeyBind_" + k.Key;
            if (isOverwriting || !PlayerPrefs.HasKey(prefKey)) {
                PlayerPrefs.SetString(prefKey, k.Value.ToString());
            }
        }
    }

    public KeyCode get(string selectionCode) {
        return (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("KeyBind_" + selectionCode, "None"));
    }

    public string getString(string selectionCode) {
        return getString(get(selectionCode));
    }

    public string getString(KeyCode code) {
        if (keyOverride.ContainsKey(code)) {
            return keyOverride[code];
        }else {
            return code.ToString();
        }
    }

    public void SetKey(string selectionCode, KeyCode code) {
        PlayerPrefs.SetString("KeyBind_" + selectionCode, code.ToString());
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void AddOverrides() {
        // home row
        keyOverride.Add(KeyCode.Alpha1, "1");
        keyOverride.Add(KeyCode.Alpha2, "2");
        keyOverride.Add(KeyCode.Alpha3, "3");
        keyOverride.Add(KeyCode.Alpha4, "4");
        keyOverride.Add(KeyCode.Alpha5, "5");
        keyOverride.Add(KeyCode.Alpha6, "6");
        keyOverride.Add(KeyCode.Alpha7, "7");
        keyOverride.Add(KeyCode.Alpha8, "8");
        keyOverride.Add(KeyCode.Alpha9, "9");
        keyOverride.Add(KeyCode.Alpha0, "0");
        keyOverride.Add(KeyCode.Minus, "-");
        keyOverride.Add(KeyCode.Equals, "=");

        //special defaultKeys
        keyOverride.Add(KeyCode.Period, ".");
        keyOverride.Add(KeyCode.Comma, ",");
        keyOverride.Add(KeyCode.BackQuote, "`");
        keyOverride.Add(KeyCode.Backslash, "\\");
        keyOverride.Add(KeyCode.Slash, "/");
        keyOverride.Add(KeyCode.LeftBracket, "[");
        keyOverride.Add(KeyCode.RightBracket, "]");

        //arrow defaultKeys
        keyOverride.Add(KeyCode.UpArrow, "Up");
        keyOverride.Add(KeyCode.DownArrow, "Down");
        keyOverride.Add(KeyCode.LeftArrow, "Left");
        keyOverride.Add(KeyCode.RightArrow, "Right");

        // keypad
        keyOverride.Add(KeyCode.Keypad1, "NumPad 1");
        keyOverride.Add(KeyCode.Keypad2, "NumPad 2");
        keyOverride.Add(KeyCode.Keypad3, "NumPad 3");
        keyOverride.Add(KeyCode.Keypad4, "NumPad 4");
        keyOverride.Add(KeyCode.Keypad5, "NumPad 5");
        keyOverride.Add(KeyCode.Keypad6, "NumPad 6");
        keyOverride.Add(KeyCode.Keypad7, "NumPad 7");
        keyOverride.Add(KeyCode.Keypad8, "NumPad 8");
        keyOverride.Add(KeyCode.Keypad9, "NumPad 9");
        keyOverride.Add(KeyCode.Keypad0, "NumPad 0");
        keyOverride.Add(KeyCode.KeypadPeriod, "NumPad .");
        keyOverride.Add(KeyCode.KeypadDivide, "NumPad /");
        keyOverride.Add(KeyCode.KeypadMultiply, "NumPad *");
        keyOverride.Add(KeyCode.KeypadMinus, "NumPad -");
        keyOverride.Add(KeyCode.KeypadPlus, "NumPad +");
        keyOverride.Add(KeyCode.KeypadEnter, "NumPad Enter");
        keyOverride.Add(KeyCode.KeypadEquals, "NumPad =");
        keyOverride.Add(KeyCode.Mouse0, "Left Click");
        keyOverride.Add(KeyCode.Mouse1, "Right Click");
        keyOverride.Add(KeyCode.Mouse2, "Middle Click");
        keyOverride.Add(KeyCode.Mouse3, "Mouse 3");
        keyOverride.Add(KeyCode.Mouse4, "Mouse 4");
        keyOverride.Add(KeyCode.Mouse5, "Mouse 5");
        keyOverride.Add(KeyCode.Mouse6, "Mouse 6");
    }
}
