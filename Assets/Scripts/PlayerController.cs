
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	//new phisics

	public List<float> GearRatios;

	public float axleRatio;

	public float minRpm;

	public float maxRpm;

	public float wheelRadius;

	public float hp;

	public float converterRatio = 3f;

	private float rpm;

	private int gear;

	private int band = 3;

	private bool pressed = false;

	private float[] positions = new float[]{-5f,-1.8f,1.8f,5f};

	void Start() {
		rpm = minRpm;
		gear = 0;
	}

	void FixedUpdate() {
		int x = 0;

		float gas = 1;
		float brake = -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);

		if(brake != 0){
			rpm -= Time.deltaTime * 2300f;
			rpm = Mathf.Max (minRpm,rpm);
		}

		if(gas == 1 && brake == 0){
			rpm += Time.deltaTime * hp * gas * 6f;
			rpm = Mathf.Min (maxRpm,rpm);
		}
		if(rpm == maxRpm && gear < GearRatios.Count-1){
			rpm = minRpm+1f;
			gear++;
		}
		if(rpm == minRpm && gear > 0){
			rpm = maxRpm-1f;
			gear--;
		}

		if(Input.GetKeyDown("left")){
			x--;
			if(!pressed)				
				move (x);
			pressed = true;

		}
		if(Input.GetKeyDown("right")){
			x++;
			if(!pressed)				
				move (x);
			pressed = true;

		}
		if(Input.GetKeyUp("left") || Input.GetKeyUp("right"))
			pressed = false;
		rigidbody.velocity = new Vector3(0,0,getVelocity());
		audio.pitch = getPitch();
		//Debug.Log("RPM = "+rpm+" - Torque = "+getVelocity() + " speed " + rigidbody.velocity.sqrMagnitude + " gear = " + gear );

	}

	public void move(int x) {
		if((band < 3 && x > 0) || (band > 0 && x < 0))
			band = band + x;
		Vector3 newPos = new Vector3(positions[band],transform.position.y,transform.position.z);
		transform.position = newPos;
	}

	public float getVelocity() {
		return 0.06f*rpm/(axleRatio*GearRatios[gear]);
	}

	public float getPitch() {
		return 1f + 0.4f*(rpm/maxRpm);
	}
}
