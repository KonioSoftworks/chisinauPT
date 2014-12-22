using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrafficController : MonoBehaviour {


	public int minCarNum = 10;
	public int maxCarNum = 20;
	public float minRenderDistance = 100f;
	public float minDestroyDistance = 150f;

	public List<GameObject> availableCars;
	public List<GameObject> rareCars;

	private float[] positions = new float[]{-5.0f,-1.8f,1.8f,5.0f};

	private float[] ellapsedTime = new float[]{0f,1f,0f,3f};	
	private float rareTime = 0f;


	// Use this for initialization
	void Start () {
		for(int i=0;i < 4;i++){
			generateCar(i,minRenderDistance+Random.Range(0.0f,15.0f));
		}
	}

	void Update () {
		rareTime += Time.deltaTime;
		for(int i=0; i< 4;i++)
			ellapsedTime[i] += Time.deltaTime;
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		var cars = GameObject.FindGameObjectsWithTag("Car");
		for(int i=0; i < cars.Length; i++){
			if((getDistance(cars[i].transform.position,player.transform.position) > minDestroyDistance)
			   || cars[i].transform.position.z < player.transform.position.z - 20){
				Destroy(cars[i]);
			}
		}
		for(int i=0;i < 4;i++){
			if(Random.Range(0,3) == 0)
				continue;
			float randTime = Random.Range(1.5f,6f);
			if(i > 1)
				randTime += Random.Range(4.5f,9f);
			if(ellapsedTime[i] < randTime)
				continue;
			else
				ellapsedTime[i] = 0f;
			generateCar(i,player.transform.position.z + minRenderDistance);
		}
		if(rareTime > 5){
			if(rareCars.Count > 0 && Random.Range(1,8) == 1)
				generateRareCar(Random.Range(0,4),player.transform.position.z + minRenderDistance + Random.Range(-10.0f,20.0f));
			rareTime = 0;
		}
	}

	void generateCar (int band,float distance) {
		Vector3 position = new Vector3(positions[band],0,distance);
		GameObject car = availableCars[Random.Range(0,availableCars.Count)];
		Quaternion rotation = Quaternion.Euler(car.transform.rotation.eulerAngles.x,car.transform.rotation.eulerAngles.y + ((positions[band] > 0)? 0 : 180),car.transform.rotation.eulerAngles.z);
		Instantiate(car,position,rotation);
	}

	void generateRareCar (int band,float distance) {
		Vector3 position = new Vector3(positions[band],0,distance);
		GameObject car = rareCars[Random.Range(0,rareCars.Count)];
		Quaternion rotation = Quaternion.Euler(car.transform.rotation.eulerAngles.x,car.transform.rotation.eulerAngles.y + ((positions[band] > 0)? 0 : 180),car.transform.rotation.eulerAngles.z);
		Instantiate(car,position,rotation);
	}

	float getDistance(Vector3 A,Vector3 B) {
		return A.z - B.z;
	}

}
