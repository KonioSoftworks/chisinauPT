using UnityEngine;
using System.Collections;

public class Migalca : MonoBehaviour {

	public float speed = 90f;
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0,speed*Time.deltaTime,0));
	}
}
