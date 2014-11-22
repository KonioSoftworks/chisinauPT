using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrafficController : MonoBehaviour {


	public int minCarNum = 10;
	public int maxCarNum = 20;
	public float minRenderDistance = 100f;
	public float minDestroyDistance = 150f;

	public List<GameObject> availableCars;

	private float[] positions = new float[]{-5.0f,-1.8f,1.8f,5.0f};

	private float[] ellapsedTime = new float[]{0f,1f,0f,3f};


	// Use this for initialization
	void Start () {
		int nr = Random.Range(minCarNum,maxCarNum);
		for(int i=0;i < 4;i++){
			generateCar(i,minRenderDistance+Random.Range(0.0f,15.0f));
		}
	}

	void Update () {
		for(int i=0; i< 4;i++)
			ellapsedTime[i] += Time.deltaTime;
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		var cars = GameObject.FindGameObjectsWithTag("Car");
		for(int i=0; i < cars.Length; i++){
			if((getDistance(cars[i].transform.position,player.transform.position) > minDestroyDistance)){
				Destroy(cars[i]);
			}
		}
		for(int i=0;i < 4;i++){
			float randTime = Random.Range(1.5f,6f);
			if(i > 1)
				randTime += Random.Range(4.5f,9f);
			if(ellapsedTime[i] < randTime)
				continue;
			else
				ellapsedTime[i] = 0f;
			generateCar(i,player.transform.position.z + minRenderDistance);
		}
	}

	void generateCar (int band,float distance) {
		Quaternion rotation = new Quaternion(0,(positions[band] > 0)? 0 : 180 ,0,0);
		Vector3 position = new Vector3(positions[band],0,distance);
		GameObject car = availableCars[Random.Range(0,availableCars.Count)];
		Instantiate(car,position,rotation);
	}

	float getDistance(Vector3 A,Vector3 B) {
		return A.z - B.z;
	}

}
