using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	public float carDistance = 12f;

	public GameObject tile;

	public int tileRendered = 12;

	public float distanceBetweenTiles = 10f;

	private List<GameObject> tiles;

	private float distance = 0f;

	public GameObject sun;

	public List<GameObject> availableBuildings;
	private List<GameObject> buildings;

	public List<GameObject> sideWalkAvailableItems;
	private List<GameObject> sideWalkItems;



	private float leftDistance = 0f;
	private float rightDistance = 0f;

	private float leftSideWalkDistance = 0f;
	private float rightSideWalkDistance = 0f;

	private float roadDistance = 18f;

	void Start () {
		tiles = new List<GameObject>();
		buildings = new List<GameObject>();
		sideWalkItems = new List<GameObject>();
		GameObject Road = GameObject.FindGameObjectWithTag("Road");

		//render road tiles
		for(int i=0;i<tileRendered;i++){
			Vector3 newPosition = new Vector3(tile.transform.position.x,tile.transform.position.y,
			                                  distance);
			distance += distanceBetweenTiles;
			tiles.Add((GameObject)Instantiate(tile,newPosition,tile.transform.rotation));
			tiles[i].transform.parent = Road.transform;
		}

		renderBuildings();
	}

	// Update is called once per frame
	void Update () {
		// move camera and sun xD
		var car = GameObject.FindGameObjectWithTag("Player");
		Vector3 carV = car.transform.position;
		Vector3 camV = transform.position;
		transform.position = new Vector3(camV.x,camV.y,carV.z - carDistance);
		sun.transform.position = new Vector3(sun.transform.position.x,sun.transform.position.y,carV.z - carDistance);

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
		renderBuildings();
	}

	void renderBuildings() {
		const float hz = 10f;
		for(int i=0;i < buildings.Count;i++) {
			if(buildings[i].transform.position.z + hz < transform.position.z){
				Destroy(buildings[i]);
				buildings.RemoveAt(i);
			}
		}

		for(int i=0;i < sideWalkItems.Count;i++) {
			if(sideWalkItems[i].transform.position.z + hz < transform.position.z){
				Destroy(sideWalkItems[i]);
				sideWalkItems.RemoveAt(i);
			}
		}

		renderSide(ref leftDistance,-1);
		renderSide(ref rightDistance,1);
		renderSideWalk(ref leftSideWalkDistance,-1);
		renderSideWalk(ref rightSideWalkDistance,1);
	}

	void renderSide(ref float sideDistance,int pos){
		while(sideDistance <= distance){
			GameObject obj = availableBuildings[Random.Range(0,availableBuildings.Count)];
			BuildingController objController = (BuildingController) obj.GetComponent(typeof(BuildingController));
			Vector3 position = new Vector3(pos*roadDistance+(pos*objController.distanceFromRoad),objController.heightFromRoad,sideDistance);

			buildings.Add((GameObject)Instantiate(obj,position,obj.transform.rotation));
			sideDistance += objController.distanceFromOthers + Random.Range(0,10);
		}
	}

	void renderSideWalk(ref float sideDistance,int pos){
		while(sideDistance <= distance){
			GameObject obj = sideWalkAvailableItems[Random.Range(0,sideWalkAvailableItems.Count)];
			BuildingController objController = (BuildingController) obj.GetComponent(typeof(BuildingController));
			Vector3 position = new Vector3(pos*roadDistance+(pos*objController.distanceFromRoad),objController.heightFromRoad,sideDistance);

			sideWalkItems.Add((GameObject)Instantiate(obj,position,obj.transform.rotation));
			sideDistance += objController.distanceFromOthers + Random.Range(0,10);
		}
	}

}
