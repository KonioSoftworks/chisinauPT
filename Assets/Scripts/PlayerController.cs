using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

public class PlayerController : MonoBehaviour {

	//transmision
	public List<float> GearRatios;

	public float axleRatio;

	public float minRpm;

	public float maxRpm;

	public float wheelRadius;
	//public GUIText score_text;
	//public GUIText speed;

	public float hp;

	public float converterRatio = 3f;

	private float rpm;
	private int gear;

	// Player Data -> To Save

	public int money = 0;

	//position on road

	private int band = 3;
	private int previousBand = 0;

	private bool pressed = false;
	private bool isPaused = false;
	private bool gameOver = false;

	private float[] positions = new float[]{-5f,-1.8f,1.8f,5f};

	private bool isMoving = false;
	private bool saved = false;
	
	// GUI

	CanvasGroup canvas;
	CanvasGroup canvas2;
	List<CanvasGroup> canvasGroups;

	//main controller
	GameObject mainController;
	Loader loader;

	void Start() {
		mainController = GameObject.FindGameObjectWithTag("MainController");
		loader = mainController.GetComponent<Loader>();
		Time.timeScale  = 1;
		rpm = minRpm;
		gear = 0;
		money = 0;
		canvas = new CanvasGroup();
		canvas = GameObject.FindObjectOfType<CanvasGroup>();
		canvas.alpha = 0f;
		canvas2 = new CanvasGroup();
		canvas2 = GameObject.FindObjectOfType<CanvasGroup>();
		canvas2.alpha = 0f;
		canvasGroups = new List<CanvasGroup>();
		canvasGroups = GameObject.FindObjectsOfType<CanvasGroup>().ToList();
		canvasGroups[0].alpha = 0f;
		canvasGroups[1].alpha = 0f;
	}

	public void Save(){
		if(!saved){
			loader.Died(money);
			money = 0;
			saved = true;
		}
	}

	public void Resume(){
		isPaused = false;
		Time.timeScale = 1;
		canvasGroups[1].alpha = 0f;
	}

	public void Exit(){
		Application.LoadLevel("menu");
		Save();
	}
	
	void OnGUI(){
		if(isPaused){
			isPaused = false;
			Time.timeScale = 0;
			//canvas.alpha = 1f;
			canvasGroups[1].alpha = 1f;
		}
		// Game Over
		if(gameOver){
			Time.timeScale = 0;
			isPaused = false;
			//canvas2.alpha = 1f;
			canvasGroups[0].alpha = 1f;
			Save();
		}

	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(Time.timeScale != 0 ){
				isPaused = true;
				Time.timeScale = 0;
				//canvas.alpha = 0f;
				canvasGroups[1].alpha = 1f;
				//Save ();
			}else{ 
				Time.timeScale = 1;
				isPaused = false;
				//canvas.alpha = 0f;
				canvasGroups[1].alpha = 0f;
			}
		}

		if (isMoving && !isPaused)
			smoothMove();
		int x = 0;

		float gas = 1;
		float brake = -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);

		if(brake != 0){
			rpm -= Time.deltaTime * 2300f;
			rpm = Mathf.Max (minRpm,rpm);
		}

		if(gas == 1 && brake == 0){
			rpm += Time.deltaTime * (10*hp - gear*20) * gas;
			rpm = Mathf.Min (maxRpm,rpm);
		}
		if(rpm == maxRpm && gear < GearRatios.Count-1){
			gear++;
			rpm = getRpmByVelocity() + 0.01f;
			audio.Stop();
			audio.PlayDelayed(0.1f);
		}
		if(rpm == minRpm && gear > 0){
			gear--;
			rpm = getRpmByVelocity() - 0.01f;
			audio.Stop();
			audio.PlayDelayed(0.1f);
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
		audio.pitch = 1 + (rpm/maxRpm);
	}

	public void move(int x) {
		if(gameOver)
			return ;
		if ((band < 3 && x > 0) || (band > 0 && x < 0)) {
			if(!isMoving)
				previousBand = band;
			band = band + x;	
		}
		isMoving = true;
	}

	public void smoothMove(){
		float[] newPositions = new float[]{0f,3.2f,6.8f,10f};

		float x = transform.position.x;
		float coefficient = 0;
		if(getVelocity() < 10)
			coefficient = (getVelocity()/20f);		
		else
			coefficient = (getVelocity()/40f);

		float units = 0.13f * coefficient * 60 * Time.deltaTime;
		float angle = 20f * coefficient;
		float angle2 = 20f * coefficient;
		float k = (band > previousBand) ? 1 : -1;

		if (band == previousBand){
			transform.rotation = Quaternion.Euler(0,0,0);
		} else {
			float radius = angle / (Mathf.Abs(newPositions[band] - newPositions[previousBand])/units);
			float radius2 = angle2 / (Mathf.Abs(newPositions[band] - newPositions[previousBand])/units);
			if (k > 0) {
				float mediumX = (newPositions[band] + newPositions[previousBand])/2f;
				if (x - positions[0] < mediumX){
					transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y + radius,transform.rotation.eulerAngles.z + radius2);
				}else{	
					transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y - radius,transform.rotation.eulerAngles.z - radius2);
				}
			} else {
				float mediumX = (newPositions[previousBand] + newPositions[band])/2f;	
				if (x - positions[0] < mediumX){				
					transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y + radius,transform.rotation.eulerAngles.z + radius2);
				}else{			
					transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y - radius,transform.rotation.eulerAngles.z - radius2);
				}
			}
		}
		x += ((x < positions[band]) ? 1f  : -1f) * units;
		Vector3 newPos = new Vector3 (x, transform.position.y, transform.position.z);
		transform.position = newPos;
		if (Mathf.Abs (x - positions [band]) < 0.2f) {
			Vector3 Pos = new Vector3 (positions[band], transform.position.y, transform.position.z);
			transform.position = Pos;
			transform.rotation = new Quaternion(0,0,0,0);
			isMoving = false;
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Coins"){
			money += 3;
			other.gameObject.SetActive(false);
		}
		if(other.gameObject.tag == "Car"){
				gameOver = true;
		}
	}

	public float getVelocity() {
		return (0.104f * wheelRadius * rpm)/(axleRatio*GearRatios[gear]);
	}

	public float getRpmByVelocity(){
		return rigidbody.velocity.z * axleRatio * GearRatios[gear] / (0.104f * wheelRadius); 
	}
	
}

