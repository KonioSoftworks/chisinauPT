using UnityEngine;
using System.Collections;

public class Rotable : MonoBehaviour {

	public float speed = 90f;

	// Update is called once per frame
	void Update () {
		transform.Rotate (0f,speed*Time.deltaTime,0f);
	}
}
