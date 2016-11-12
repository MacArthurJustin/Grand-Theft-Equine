using UnityEngine;
using System.Collections;

public class AttachControllable : MonoBehaviour {
    public  GameObject Controllable;

	// Use this for initialization
	void Start () {
        GetComponent<PlayerController>().SetTarget(Controllable);
	}
}
