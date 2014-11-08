using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public List<WheelCollider> forwardWheels;

	public List<WheelCollider> backWheels;

	public float motorTorque = 30f;

	public float steerAngle = 10f;

	public float brakeTorque = 50f;

	public float maxSpeed = 50f;

	// Update is called once per frame
	void FixedUpdate () {

		float steer = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
		float motor = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
		float brake = -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);

		setSteerAngle(steer * steerAngle);
		setTorque(motorTorque * motor);
		setBrake(brake * brakeTorque);

		audio.pitch = 1 + (forwardWheels[forwardWheels.Count-1].motorTorque) / motorTorque;
	}

	void setTorque(float value){
		for(int i=0;i < forwardWheels.Count;i++){
			forwardWheels[i].motorTorque = value;
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
