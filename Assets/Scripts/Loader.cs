using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;

public class Loader : MonoBehaviour {
	
	public List<Transport> buses;
		
	public PlayerSave saveController;

	private GameObject bus;
	private PlayerController pc;

	public Transport transport;

	private int score = 0;

	public GameObject deadText;

	public void loadAndExecute() {
		saveController = new PlayerSave();
		if(!buses[saveController.data.bus])
			saveController.data.bus = 0;
		spawnBus();
	}

	private void spawnBus(){
		Vector3 position = new Vector3(5f,0f,10f);
		GameObject bus1 = GameObject.FindGameObjectWithTag("Player");
		if(bus1){			
			Destroy(bus1);
		}
		transport = buses [saveController.data.bus];
		GameObject selBus = transport.car;
		bus = (GameObject)Instantiate(selBus,position,selBus.transform.rotation);
		pc = bus.GetComponent<PlayerController>();
		pc.coinValue = transport.coinValue;
	}

	public void Start(){
		Time.timeScale = 1;
		if(Application.loadedLevel == 1)
			loadAndExecute();
	}

	void sendDataToServer(int scoreValue){
		score = scoreValue;
		Thread t = new Thread(threadSendData);
		t.Start();
	}

	void threadSendData(){
		ServerScript server = new ServerScript();
		server.save(saveController.data.name,score);
	}

	public void Died(int score){
		saveController.data.money += score;
		saveController.Save();
		pc.audio.Stop();
		if(deadText){
			deadText.GetComponent<Text>().text = "Whoops...\nYour earned "+score+" lei";
		}
		sendDataToServer(score);
	}

	public void resumeButton(){
		pc.Resume();
	}

	public void exitButton(){
		pc.Exit();
	}

	public void retryButton(){
		pc.Save();
		Application.LoadLevel("scene");
	}
}
