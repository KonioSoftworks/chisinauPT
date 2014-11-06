using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float carDistance = 12;

	// Update is called once per frame
	void Update () {
		var car = GameObject.FindGameObjectWithTag("Player");
		Vector3 carV = car.transform.position;
		Vector3 camV = transform.position;
		transform.position = new Vector3(carV.x - carDistance,camV.y,camV.z);
	}
}
