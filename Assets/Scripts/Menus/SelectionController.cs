using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour {
    public ButtonControl[] buttons;
    public Button stop;
    private bool awake = false;
    public bool selecting = false;
    private string selected = null;
    public Canvas controls;
    public GameObject clickArea;
    private RectTransform region;
    private KeyCode[] mouseKeys = new KeyCode[] { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2, KeyCode.Mouse3, KeyCode.Mouse4, KeyCode.Mouse5, KeyCode.Mouse6 };
    private KeyCode[] extraKeys = new KeyCode[] { KeyCode.LeftShift, KeyCode.RightShift };

    void Start() {
        awake = true;
        buttons = gameObject.GetComponentsInChildren<ButtonControl>();
        region = clickArea.GetComponent<RectTransform>();
        UpdateAll();
    }

    void OnGUI() {
        if (controls.enabled) {
            if (selected == null) return;
            Event e = Event.current;
            if (e != null && e.isKey) {
                Debug.Log(e.keyCode);
                if (e.keyCode.Equals(KeyCode.Escape)) {
                    ControlSettings.Instance.SetKey(selected, KeyCode.None);
                    EnableAll();
                } else if (selecting) {
                    ControlSettings.Instance.SetKey(selected, e.keyCode);
                    EnableAll();
                }
            }
        }
        UpdateAll();
    }

    public void ResetKeys() {
        Debug.Log("Resetting Keys");
        ControlSettings.Instance.InitKeys(true);
        UpdateAll();
    }

    private void Update() {
        if (!awake) return;
        stop.interactable = selecting;
        if (!controls.enabled) return;
        if (selecting && Input.anyKeyDown) {
            if (MouseContained()) {
                foreach (KeyCode key in mouseKeys) {
                    if (Input.GetKeyDown(key)) {
                        ControlSettings.Instance.SetKey(selected, key);
                        EnableAll();
                        UpdateAll();
                        return;
                    }
                }
            }
            foreach (KeyCode key in extraKeys) {
                if (Input.GetKeyDown(key)) {
                    ControlSettings.Instance.SetKey(selected, key);
                    EnableAll();
                }
            }
            UpdateAll();
        }
    }

    private bool MouseContained() {
        Vector3[] corners = new Vector3[4];
        region.GetWorldCorners(corners);
        Rect newRect = new Rect(corners[0], corners[2] - corners[0]);
        return newRect.Contains(Input.mousePosition);
    }

    public void Select(string selectionCode) {
        DisableAll();

        selected = selectionCode;
        selecting = true;
    }

    public void DisableAll() {
        foreach (ButtonControl c in buttons) {
            c.button.interactable = false;
        }
    }

    public void UpdateAll() {
        foreach (ButtonControl c in buttons) {
            c.text.text = ControlSettings.Instance.getString(c.selectionCode);
        }
    }

    public void EnableAll() {
        foreach (ButtonControl c in buttons) {
            c.button.interactable = true;
        }
        selecting = false;
        selected = null;
    }
}
