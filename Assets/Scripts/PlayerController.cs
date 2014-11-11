
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public List<WheelCollider> forwardWheels;

	public List<WheelCollider> backWheels;

	//new phisics

	public List<float> GearRatios;

	public float axleRatio;

	public float minRpm;

	public float maxRpm;

	public float wheelRadius;

	public float hp;

	private float rpm;

	private int gear;

	private float converterRatio = 5f;

	public float steerAngle = 10f;

	public float brakeTorque = 50f;

	void Start() {
		rpm = minRpm;
		gear = 0;
	}

	void FixedUpdate() {
		float steer = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
		float gas = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
		float brake = -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);

		if(gas == 0){
			rpm -= Time.deltaTime * hp;
			rpm = Mathf.Max (minRpm,rpm);
		} else {
			rpm += Time.deltaTime * hp * gas * 4;
			rpm = Mathf.Min (maxRpm,rpm);
		}
		if(rpm == maxRpm && gear < GearRatios.Count-1){
			rpm = minRpm;
			gear++;
		}
		if(rpm == minRpm && gear > 0){
			rpm = maxRpm;
			gear--;
		}
		setTorque(getTorque());
		setSteerAngle(steer*steerAngle);
		setBrake(brake*brakeTorque);
		//Debug.Log (Time.deltaTime);
		Debug.Log("RPM = "+rpm+" - Torque = "+getTorque() + " speed " + rigidbody.velocity.sqrMagnitude + " gear = " + gear );
	}

	public float getTorque() {
		return rpm/(axleRatio*GearRatios[gear]*converterRatio);
	}

	/*

	public float motorTorque = 30f;

	public float steerAngle = 10f;

	public float brakeTorque = 50f;

	public float maxSpeed = 50f;

	public float maxSqrMagnitude = 1600f;

	// Update is called once per frame
	void FixedUpdate () {

		float steer = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
		float motor = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
		float brake = -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);

		setSteerAngle(steer * steerAngle);
		setBrake(brake * brakeTorque);
		Debug.Log(rigidbody.velocity.sqrMagnitude);
		if(rigidbody.velocity.sqrMagnitude < maxSqrMagnitude)
			setTorque(motorTorque * motor);
		audio.pitch = 1 + (forwardWheels[forwardWheels.Count-1].motorTorque) / motorTorque;
	}

	*/

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
