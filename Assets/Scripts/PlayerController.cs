using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerController : MonoBehaviour {

	//transmision
	public List<float> GearRatios;

	public float axleRatio;

	public float minRpm;

	public float maxRpm;

	public float wheelRadius;
	public GUIText score_text;

	public float hp;

	public float converterRatio = 3f;

	private float rpm;
	private float fuel = 100;
	public float maxFuel = 100;
	private int gear;

	//fuel

	private int fuelCan = 20;


	// Player Data -> To Save

	public int money;
	public int car;

	//position on road

	private int band = 3;
	private int previousBand = 0;

	private bool pressed = false;
	private bool isPaused = false;
	private bool gameOver = false;

	private float[] positions = new float[]{-5f,-1.8f,1.8f,5f};

	private float pase;
	private float delta = 1f;

	private bool isMoving = false;

	// GUI
	float posx_resume = Screen.width / 2 -50;
	float posy_resume = Screen.height / 2 - 20;
	float posy_exit = Screen.height / 2 + 50;

	//FMOD

	FMOD.Studio.EventInstance Engine;
	FMOD.Studio.ParameterInstance EngineRPM;
	FMOD.Studio.ParameterInstance EngineLoad;

	void Start() {
		Time.timeScale  = 1;
		rpm = minRpm;
		gear = 0;
		money = 0;
		Load();
		//FMOD
		Engine = FMOD_StudioSystem.instance.GetEvent ("event:/v2");
		Engine.getParameter ("RPM", out EngineRPM);
		Engine.getParameter ("Load",out EngineLoad);
		Engine.start();
	}

	void Save(){
		BinaryFormatter b = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerData.dat");
		PlayerData data = new PlayerData();
		data.money = money;
		data.car = car;
		b.Serialize(file, data);
		file.Close();
	}

	 void Load(){
		if(File.Exists(Application.persistentDataPath + "/playerData.dat")){
			BinaryFormatter b = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath +"/playerData.dat", FileMode.Open);
			PlayerData data = (PlayerData)b.Deserialize(file);
			money = data.money;
			car = data.car;
			file.Close();
		}
	}


	void OnGUI(){
		GUI.color = Color.yellow;
		GUI.Box( new Rect(Screen.width - 210, 10, getBoxWidthByFuel(fuel), 20), "");

		// Game Paused
		if(isPaused){
			GUI.color = new Color(1.0f,1.0f,1.0f, 1.0f);
			GUI.Box(new Rect(0,0, Screen.width, Screen.height), "");
			if(GUI.Button(new Rect(posx_resume, posy_resume ,100,40),"Resume") ){
				isPaused = false;
				Time.timeScale = 1;
			}
			if(GUI.Button( new Rect(posx_resume, posy_exit, 100, 40),"Exit") ) {
				Save ();
				Application.LoadLevel("menu");
			}

		}
		// Game Over
		if(gameOver){
			Engine.stop (0);
			Engine.release ();
			Time.timeScale = 0;
			GUI.color = new Color(1.0f,1.0f,1.0f, 1.0f);
			GUI.Box(new Rect(0,0, Screen.width, Screen.height), "");

			if(GUI.Button(new Rect(posx_resume, posy_resume ,100,40),"Try again") ){
				isPaused = false;
				Time.timeScale = 1;
				Application.LoadLevel("scene");
			}
			if(GUI.Button( new Rect(posx_resume, posy_exit, 100, 40),"Exit") ) {
				Save ();
				Application.LoadLevel("menu");
			}
		}

	}

	int getBoxWidthByFuel(float fuel){
		return Mathf.RoundToInt((200/maxFuel) * fuel);
	}

	void Update() {
		if(fuel <= 0)
			gameOver = true;
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(Time.timeScale != 0 ){
				isPaused = true;
				Time.timeScale = 0;
				Save ();
			}else{ 
				Time.timeScale = 1;
				isPaused = false;
			}
		}

		if (isMoving && !isPaused)
			smoothMove();
		int x = 0;
		fuel = Mathf.Clamp(fuel - Time.deltaTime * 3 *(rpm/maxRpm),0,maxFuel);

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
		float units = 0.2f;
		float angle = 30f;
		if( getVelocity() < 30 ){
			angle = 15f;
			units = 0.1f;
		}
		float k = (band > previousBand) ? 1 : -1;

		if (band == previousBand){
			transform.rotation = Quaternion.Euler(0,0,0);
		} else {
			float radius = angle / (Mathf.Abs(newPositions[band] - newPositions[previousBand])/units);
			if (k > 0) {
				float mediumX = (newPositions[band] + newPositions[previousBand])/2f;
				if (x - positions[0] < mediumX){
					transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y + radius,0);
				}else{	
					transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y - radius,0);
				}
			} else {
				float mediumX = (newPositions[previousBand] + newPositions[band])/2f;	
				if (x - positions[0] < mediumX){				
					transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y + radius,0);
				}else{			
					transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y - radius,0);
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
			score_text.text = "Money : " + money.ToString() + " lei";
			other.gameObject.SetActive(false);
		}
		if(other.gameObject.tag == "Fuel"){
			fuel = Mathf.Clamp(fuel+fuelCan,0,maxFuel);
			Destroy(other.gameObject);
		}
		if(other.gameObject.tag == "Car"){
			if(getVelocity() > 20)
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

[System.Serializable]
class PlayerData{
	public int money;
	public int car;
}
