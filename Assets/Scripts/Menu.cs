using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public void ChangeScene(){
		Application.LoadLevel("scene");
	}

	public void Exit(){
		Application.Quit();
	}

}
