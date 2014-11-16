﻿
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	//transmision
	public List<float> GearRatios;

	public float axleRatio;

	public float minRpm;

	public float maxRpm;

	public float wheelRadius;
	public int score = 0;
	public GUIText score_text;

	public float hp;

	public float converterRatio = 3f;

	private float rpm;

	private int gear;

	//effects

	//position on road

	private int band = 3;
	private int previousBand = 0;

	private bool pressed = false;

	private float[] positions = new float[]{-5f,-1.8f,1.8f,5f};

	private float pase;
	private float delta = 1f;

	private bool isMoving = false;

	//FMOD

	FMOD.Studio.EventInstance Engine;
	FMOD.Studio.ParameterInstance EngineRPM;
	FMOD.Studio.ParameterInstance EngineLoad;

	void Start() {
		rpm = minRpm;
		gear = 0;
		score_text.text = "Score : ";
		score = 0;
		//FMOD
		Engine = FMOD_StudioSystem.instance.GetEvent ("event:/v2");
		Engine.getParameter ("RPM", out EngineRPM);
		Engine.getParameter ("Load",out EngineLoad);
		Engine.start();
	}

	void Update() {

		if (isMoving)
			smoothMove ();
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
			gear++;
			rpm = getRpmByVelocity() + 0.01f;
		}
		if(rpm == minRpm && gear > 0){
			gear--;
			rpm = getRpmByVelocity() - 0.01f;
			//Engine.stop();
		}

		if(Input.GetKeyDown("left")){
			x--;
			move (x);
			pressed = true;

		}
		if(Input.GetKeyDown("right")){
			x++;				
			move (x);
			pressed = true;

		}
		if(Input.GetKeyUp("left") || Input.GetKeyUp("right"))
			pressed = false;
		rigidbody.velocity = new Vector3(0,0,getVelocity());
		EngineRPM.setValue(rpm);
		EngineLoad.setValue(gear * (1/GearRatios.Count));
	}

	public void move(int x) {
		if(isMoving)
			return ;
		isMoving = true;
		if ((band < 3 && x > 0) || (band > 0 && x < 0)) {
			previousBand = band;
			band = band + x;	
		}
	}

	public void smoothMove(){
		float[] newPositions = new float[]{0f,3.2f,6.8f,10f};
		float x = transform.position.x;
		float units = getVelocity()/200f; //kakaita huinea ! do not change this number
		float k = (band > previousBand) ? 1 : -1;
		float radius = getVelocity()/40f;
		if (k > 0) {
			float mediumX = (newPositions[band] + newPositions[previousBand])/2f;
			if (x - positions[0] < mediumX){
				transform.Rotate (Vector3.up, 1 * radius);
				transform.Rotate (Vector3.forward, 1 * radius);
			}else{
				transform.Rotate (Vector3.up, -1 * radius);
				transform.Rotate (Vector3.forward, -1 * radius);
			}
		} 

		else {
			float mediumX = (newPositions[previousBand] + newPositions[band])/2f;	
			if (x - positions[0] < mediumX){
				transform.Rotate (Vector3.up, 1 * radius);				
				transform.Rotate (Vector3.forward, 1 * radius);
			}else{
				transform.Rotate (Vector3.up, -1 * radius);				
				transform.Rotate (Vector3.forward, -1 * radius);
			}
		}
		x += ((x < positions[band]) ? 1f  : -1f) * units;
		Vector3 newPos = new Vector3 (x, transform.position.y, transform.position.z);
		transform.position = newPos;

		if (Mathf.Abs (x - positions [band]) < units) {
			Vector3 Pos = new Vector3 (positions[band], transform.position.y, transform.position.z);
			transform.position = Pos;
			transform.rotation = new Quaternion(0,0,0,0);
			isMoving = false;
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Coins"){
				score += 1;
				score_text.text = "Score : " + score.ToString();
				Debug.Log("DSDsad");
			}

	}



	public float getVelocity() {
		return 0.06f*rpm/(axleRatio*GearRatios[gear]);
	}

	public float getRpmByVelocity(){
		return rigidbody.velocity.z * axleRatio * GearRatios[gear] / 0.06f; 
	}

}
