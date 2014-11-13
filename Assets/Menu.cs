using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	float posx_play = Screen.width / 2 -50;
	float posy_play = Screen.height / 2 - 20;
	float posy_exit = Screen.height / 2 + 50;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

		if(GUI.Button(new Rect(posx_play, posy_play ,100,40),"Play!") ){
			Application.LoadLevel("scene");
		}
		if(GUI.Button( new Rect(posx_play, posy_exit, 100, 40),"Exit") ) {
			Application.Quit();
		}

	}

}
