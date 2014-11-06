using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public List<WheelCollider> forwardWheels;

	public List<WheelCollider> backWheels;

	public float motorTorque = 10f;

	public float steerAngle = 20f;

	public float brakeTorque = 50f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		setSteerAngle(steerAngle * x);
		if(y > 0){
			//setBrake(0);
			setTorque(motorTorque*y);
		} else if(y < 0){
			//setBrake(brakeTorque*y);
		}

	}

	void setTorque(float value){
		for(int i=0;i < forwardWheels.Count;i++){
			forwardWheels[i].motorTorque = motorTorque;
		}
	}

	void setSteerAngle(float value){
		for(int i=0;i < forwardWheels.Count;i++){
			forwardWheels[i].steerAngle = value;
		}
	}

	void setBrake(float value){
		for(int i=0;i < forwardWheels.Count;i++){
			forwardWheels[i].brakeTorque = value;
		}
		for(int i=0;i < backWheels.Count;i++){
			backWheels[i].brakeTorque = value;
		}
	}	
}
