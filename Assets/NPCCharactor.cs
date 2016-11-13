using UnityEngine;
using UnityEngine.UI;
  using System.Collections;

public class NPCCharactor : Character, IInteractable
{
  public string speech_text;
  public Text text;
  public float timer;
 
 public bool CanInteract { get { return true; } }
  public void Interact(PlayableCharacter PC)
  {
    if (CanInteract)
    {
      text.text = speech_text;
      timer = 3.0f;
      return;
    }
    return;
  }

  // Use this for initialization
  void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
    if (timer > 0) {
      timer -= Time.deltaTime; }
   else
    {
      text.text = "";
    }
	}
}
