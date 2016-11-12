using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

  GameObject player;
  GameObject exteriorDoor;
  GameObject interiorDoor;
	// Use this for initialization
	void Start () {
    player = GameObject.FindGameObjectWithTag("Player");
    exteriorDoor = GameObject.FindGameObjectWithTag("SaloonDoorExterior");
    interiorDoor = GameObject.FindGameObjectWithTag("SaloonDoorInterior");
	}
  

  void OnTriggerEnter2D(Collider2D collision)
  {
    Debug.Log(collision);
    Vector3 reposition = interiorDoor.GetComponent<Transform>().position;
    reposition.z += 2;
    if (collision.gameObject == player)
    {
      player.GetComponent<Transform>().position = reposition;
    }
  }


  // Update is called once per frame
  void Update () {
	
	}
}
