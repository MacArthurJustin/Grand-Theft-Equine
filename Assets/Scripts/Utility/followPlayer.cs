using UnityEngine;
using System.Collections;

public class followPlayer : MonoBehaviour {
    Transform tra;
    Transform ply_tra;

	// Use this for initialization
	void Start () {
        tra = this.GetComponent<Transform>();
        ply_tra = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (ply_tra != null)
        {
            Vector3 move = ply_tra.position;
            move.z = -10;
            tra.position = move;
        }
	}
}
