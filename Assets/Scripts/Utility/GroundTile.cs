using UnityEngine;
using System.Collections;

public class GroundTile : MonoBehaviour {

  public GameObject groundTile;
	// Use this for initialization
	void Start () {
    int tilesHigh = 32;
    int tilesAcross = 32;
    int unitPerTile = 16;
    for (int i = 0; i > tilesAcross; i++)
    {
    for (int j = 0; j> tilesHigh; j++)
      {

      }
    }
    GameObject obj = (GameObject)Instantiate(groundTile, Vector3.zero, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
