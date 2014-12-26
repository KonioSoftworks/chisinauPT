using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Store : MonoBehaviour {

	//info data
	public GameObject cash;
	public GameObject cost;
	public GameObject infobox;

	//controllers
	public GameObject selectButton;
	public GameObject buyButton;

	public Loader mainController;

	public GameObject scene;

	private int currentCar = 0;

	// Use this for initialization
	void Awake () {
		mainController.saveController = new PlayerSave();
		updateCash();
		spawnCar(currentCar);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void goBack(){
		Application.LoadLevel("menu");
	}

	private void updateCash(){
		cash.GetComponent<Text>().text = "Cash : " + mainController.saveController.data.money + " lei";
	}

	private void spawnCar(int id){
		GameObject car = GameObject.FindGameObjectWithTag("Player");
		if(car)
			Destroy(car);
		GameObject carGO = mainController.buses[id].car;
		if(!carGO){
			Debug.Log("Bus not found ;((((");
			return;
		}
		car = (GameObject)Instantiate(carGO,Vector3.zero,carGO.transform.rotation);
		car.GetComponent<PlayerController>().enabled = false;
		car.transform.parent = scene.transform;

		//updating data

		cost.GetComponent<Text>().text = "Cost : " + mainController.buses[id].price + " lei";
		infobox.GetComponent<Text>().text = "";
		foreach(string text in mainController.buses[id].info){
			infobox.GetComponent<Text>().text += (char)183 + " " + text + (char)10;
		}
		if(haveBus(currentCar)){
			buyButton.SetActive(false);
			selectButton.SetActive(true);
		} else {
			buyButton.SetActive(true);
			selectButton.SetActive(false);
		}
		if(mainController.saveController.data.bus == currentCar)
			selectButton.SetActive(false);
	}

	private bool haveBus(int id){
		bool a = false;
		for(int i=0;i<mainController.saveController.data.ownBuses.Count;i++){
			if(mainController.saveController.data.ownBuses[i] == id)
				a = true;
		}
		return a;
	}

	public void nextCar(){
		currentCar++;
		if(currentCar >= mainController.buses.Count)
			currentCar = 0;
		spawnCar(currentCar);
	}

	public void previousCar(){
		currentCar--;
		if(currentCar < 0)
			currentCar = mainController.buses.Count-1;
		spawnCar(currentCar);		
	}

	public void buy(){
		if(!haveBus(currentCar)){
			if(mainController.saveController.data.money >= mainController.buses[currentCar].price){
				mainController.saveController.data.ownBuses.Add(currentCar);
				mainController.saveController.data.money -= mainController.buses[currentCar].price;
				updateCash();
				mainController.saveController.Save();
				spawnCar(currentCar);
			}
		}
	}

	public void select(){
		if(haveBus(currentCar)){
			mainController.saveController.data.bus = currentCar;
			mainController.saveController.Save();
			selectButton.SetActive(false);
		}
	}
}
