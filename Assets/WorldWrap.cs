using UnityEngine;
using System.Collections;

public class WorldWrap : MonoBehaviour {


  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject == GameObject.FindGameObjectWithTag("Player"))
    {
      other.transform.position = Vector3.zero;
    }
  }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
