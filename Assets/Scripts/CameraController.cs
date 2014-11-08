using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	public float carDistance = 12f;

	public GameObject tile;

	public int tileRendered = 10;

	public float distanceBetweenTiles = 10f;

	private List<GameObject> tiles;

	private float distance = 0f;

	void Start () {
		tiles = new List<GameObject>();
		GameObject Road = GameObject.FindGameObjectWithTag("Road");
		for(int i=0;i<tileRendered;i++){
			Vector3 newPosition = new Vector3(tile.transform.position.x,tile.transform.position.y,
			                                  distance);
			distance += distanceBetweenTiles;
			tiles.Add((GameObject)Instantiate(tile,newPosition,tile.transform.rotation));
			tiles[i].transform.parent = Road.transform;
		}
	}

	// Update is called once per frame
	void Update () {
		// move camera
		var car = GameObject.FindGameObjectWithTag("Player");
		Vector3 carV = car.transform.position;
		Vector3 camV = transform.position;
		transform.position = new Vector3(camV.x,camV.y,carV.z - carDistance);

		// moving tiles
		for(int i=0;i < tiles.Count;i++){
			if(!tiles[i].renderer.isVisible && tiles[i].transform.position.z < carV.z - distanceBetweenTiles){
				Vector3 newPosition = new Vector3(tiles[i].transform.position.x,tiles[i].transform.position.y,
				                                  distance);
				tiles[i].transform.position = newPosition;
				//Debug.Log (distance);
				distance += distanceBetweenTiles;
			}
		}
	}


}
