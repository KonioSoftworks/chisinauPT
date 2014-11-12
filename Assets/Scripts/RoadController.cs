using UnityEngine;
using System.Collections;

public class RoadController : MonoBehaviour {

	public float distanceInc = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*
	void OnBecameInvisible() {
		var objects = GameObject.FindGameObjectsWithTag(tag);
		var max = objects[0].transform.position.z;
		for(int i=1;i<objects.Length;i++){
			//Debug.Log(objects[i].transform.position.z);
			if(max < objects[i].transform.position.z)
				max = objects[i].transform.position.z;
		}
		Vector3 cPos = transform.position;
		cPos.z = max+distanceInc;
		transform.position = cPos;
		//Debug.Log(cPos);
	}
	*/
}
