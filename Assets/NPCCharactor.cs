using UnityEngine;
using System.Collections;

public class NPCCharactor : Character, IInteractable
{

  private float repdiff = 0;
 public bool CanInteract { get { return Mathf.Abs (repdiff) < 10; } }
 public void Interact(PlayableCharacter PC) {
  //  repdiff = 
  }
  

  // Use this for initialization
  void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
