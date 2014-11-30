using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Loader : MonoBehaviour {
	
	public List<Transport> buses;

	private PlayerSave saveController;

	private GameObject bus;
	private PlayerController pc;

	public void loadAndExecute() {
		saveController = new PlayerSave();
		saveController.data.bus = 1;
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
		bus = (GameObject)Instantiate(buses[saveController.data.bus].car,position,Quaternion.Euler(0,0,0));
		pc = bus.GetComponent<PlayerController>();
	}

	void OnLevelWasLoaded(int level) {
		if (level == 1){
			GameObject mainController = GameObject.FindGameObjectWithTag("MainController");
			if(!mainController)
				Debug.Log("Cannot find maincontroller");
			Loader loader = mainController.GetComponent<Loader>();
			loader.loadAndExecute();
		}
	}

	void sendDataToServer(int score){
		ServerScript server = new ServerScript();
		server.save(saveController.data.name,score);
	}

	public void Died(int score){
		saveController.data.money += score;
		saveController.Save();
		sendDataToServer(score);
		Debug.Log("Saved and sent to server");
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
