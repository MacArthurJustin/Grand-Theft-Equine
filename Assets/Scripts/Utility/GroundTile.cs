using UnityEngine;
using System.Collections;

public class GroundTile : MonoBehaviour {

  public GameObject groundTile;
	// Use this for initialization
	void Start () {
    int tilesHigh = 32;
    int tilesAcross = 32;
    int unitPerTile = 16;
    for (int i = 0; i < tilesAcross; i++)
    {
      for (int j = 0; j < tilesHigh; j++)
      {
      
        float x = i * unitPerTile - tilesAcross * unitPerTile / 2;
        float y = j * unitPerTile - tilesHigh * unitPerTile / 2;
        Vector3 pos = new Vector3(x, y, 10.0f);
        GameObject obj = (GameObject)Instantiate(groundTile, pos, Quaternion.identity);
        obj.transform.SetParent(this.transform);
        
      }
    }
   
  }
	
	// Update is called once per frame
	void Update () {
	
	}
}
