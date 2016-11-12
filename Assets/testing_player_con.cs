using UnityEngine;
using System.Collections;

public class testing_player_con : MonoBehaviour {

  float speed = 20f;
  Transform tra;
	// Use this for initialization
	void Start () {
    tra = this.GetComponent<Transform>();
    

	}
	
	// Update is called once per frame
	void Update () {
    
    float deltaY = Input.GetAxis("Vertical");
    float deltaX = Input.GetAxis("Horizontal");
    Vector3 move = new Vector3(deltaX * speed* Time.deltaTime, deltaY * speed* Time.deltaTime, 0);
    tra.Translate(move, Space.World);
    
	}
}
