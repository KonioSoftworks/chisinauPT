using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public void ChangeScene(){
		Application.LoadLevel("scene");
	}

	public void GoStore(){
		Application.LoadLevel("shop");
	}

	public void goFacebook(){
		Application.OpenURL("https://www.facebook.com/koniosoftworks");
	}

	public void Exit(){
		Application.Quit();
	}

}
