using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public List<WheelCollider> wheels;

	public float motorTorque = 50f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

	}

	void addTorque(float value){
		for(WheelCollider wheel in wheels){

		}
	}
}
