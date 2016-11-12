using UnityEngine;
using System.Collections;

public class AttachControllable : MonoBehaviour {
    public GameObject Controllable;
    public Weapon DefaultGun;

	// Use this for initialization
	void Start () {
        GetComponent<PlayerController>().SetTarget(Controllable);

        Controllable.GetComponent<PlayableCharacter>().SetWeapon(DefaultGun);
	}
}
