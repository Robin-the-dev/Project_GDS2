using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LinkedObject : MonoBehaviour {
    // Start is called before the first frame update
    public abstract void Activate(string msg);

    public abstract void Deactivate(string msg);

}
