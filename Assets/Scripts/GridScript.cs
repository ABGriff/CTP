using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour {

    public Transform gridPrefab;
    public Vector3 Size;
    public static bool lemon;

	// Use this for initialization
	void Start () {
        CreateGrid();
        lemon = false;
	}
	
    //Creates the grid of points to snap things onto. Chucks all of the points under the "Grid" Game Object.
	void CreateGrid()
    {
        GameObject go = GameObject.Find("Grid");
        for (int x = 0; x < Size.x; x++) {
            for (int y = 0; y < Size.y; y++) {
                for (int z = 0; z < Size.z; z++) {
                    //Divide our Vector 3 by 5 to place a point every .20 Metres IRL.
                    Transform instance = Instantiate(gridPrefab, (new Vector3(x-5, y-3, z+17)/5), Quaternion.identity);
                    instance.transform.parent = go.transform;
                }
            }
        }
    }
}
