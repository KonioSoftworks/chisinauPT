using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	float posx_play = Screen.width / 2 -50;
	float posy_play = Screen.height / 2 - 20;
	float posy_exit = Screen.height / 2 + 50;


	public void ChangeScene(){
		Application.LoadLevel("scene");
	}

	public void Exit(){
		Application.Quit();
	}

}
