using UnityEngine;
using System.Collections;

public class testing_player_con : MonoBehaviour {

 public  float speed = 20f;
  Transform tra;
  GameObject cam;
  Transform cam_tra;
	// Use this for initialization
	void Start () {
    tra = this.GetComponent<Transform>();
    cam = GameObject.FindGameObjectWithTag("MainCamera");
    cam_tra = cam.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
    float deltaY = Input.GetAxis("1 Move Y");
    float deltaX = Input.GetAxis("1 Move X");
    Vector3 move = new Vector3(deltaX * speed * Time.deltaTime, deltaY * speed *
      Time.deltaTime, 0);
    tra.Translate(move, Space.World);
    

	}
}
