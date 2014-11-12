
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
	private int previousBand = 0;

	private bool pressed = false;

	private float[] positions = new float[]{-5f,-1.8f,1.8f,5f};

	private float pase;
	private float delta = 1f;

	private bool isMoving = false;

	void Start() {
		rpm = minRpm;
		gear = 0;
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
			rpm = minRpm+1f;
			gear++;
		}
		if(rpm == minRpm && gear > 0){
			rpm = maxRpm-1f;
			gear--;
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
		audio.pitch = getPitch();
		//Debug.Log("RPM = "+rpm+" - Velocity = "+getVelocity() + " gear " + gear );

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
			if (x - positions[0] < mediumX)
				transform.Rotate (Vector3.up, 1 * radius);
			else{
				transform.Rotate (Vector3.up, -1 * radius);
			}
		} 

		else {
			float mediumX = (newPositions[previousBand] + newPositions[band])/2f;	
			if (x - positions[0] < mediumX)
				transform.Rotate (Vector3.up, 1 * radius);
			else
				transform.Rotate (Vector3.up, -1 * radius);
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





	public float getVelocity() {
		return 0.06f*rpm/(axleRatio*GearRatios[gear]);
	}

	public float getPitch() {
		return 1f + 0.4f*(rpm/maxRpm);
	}
}
