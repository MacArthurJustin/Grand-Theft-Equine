﻿using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

  GameObject player;
  GameObject saloonExteriorDoor;
  GameObject saloonInteriorDoor;
  GameObject hatShopExteriorDoor;
  GameObject hatShopInteriorDoor;
  float doorOffset = 1.0f;
	// Use this for initialization
	void Start () {
    player = GameObject.FindGameObjectWithTag("Player");
    saloonExteriorDoor = GameObject.FindGameObjectWithTag("SaloonDoorExterior");
    saloonInteriorDoor = GameObject.FindGameObjectWithTag("SaloonDoorInterior");
    hatShopExteriorDoor = GameObject.FindGameObjectWithTag("HatShopDoorExterior");
    hatShopInteriorDoor = GameObject.FindGameObjectWithTag("HatShopDoorInterior");
	}
  

  void OnTriggerEnter2D(Collider2D other)
  {
    // when the player enters the collider for a door they will be translated to the coresponding door 

    //Saloon doors
    if (other.gameObject == saloonExteriorDoor)
    {
      Vector3 reposition = saloonInteriorDoor.GetComponent<Transform>().position;
      reposition.y += doorOffset;
      reposition.z = -5;
      player.GetComponent<Transform>().position = reposition;
    }

    if (other.gameObject == saloonInteriorDoor)
    {
      Vector3 reposition = saloonExteriorDoor.GetComponent<Transform>().position;
      reposition.y -= doorOffset;
      reposition.z = -5;
      player.GetComponent<Transform>().position = reposition;
    }

    // Hat Shop Doors
    if (other.gameObject == hatShopExteriorDoor)
    {
      Vector3 reposition = hatShopInteriorDoor.GetComponent<Transform>().position;
      reposition.y += doorOffset;
      reposition.z = -5;
      player.GetComponent<Transform>().position = reposition;
    }

    if (other.gameObject == hatShopInteriorDoor)
    {
      Vector3 reposition = hatShopExteriorDoor.GetComponent<Transform>().position;
      reposition.y -= doorOffset;
      reposition.z = -5;
      player.GetComponent<Transform>().position = reposition;
    }
  }


  // Update is called once per frame
  void Update () {
	
	}
}
