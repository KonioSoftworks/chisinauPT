using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {

	private float velocity = 30f;

	// Use this for initialization
	void Start () {
		velocity = Random.Range(15f,25f);
		setVelocity(velocity);
	}

	void setVelocity(float value) {
		velocity = value;
		if(transform.position.x < 0)
			velocity *= -1;
		rigidbody.velocity = new Vector3(0,0,velocity);
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Car"){
			if((transform.position.z < other.gameObject.transform.position.z && transform.position.x > 0) || 
			   		(transform.position.z > other.gameObject.transform.position.z && transform.position.x < 0)){
				setVelocity(Mathf.Abs(other.gameObject.rigidbody.velocity.z) - Random.Range(1f,3f));				
				CarController second = other.gameObject.GetComponent<CarController>();
				second.setVelocity(Mathf.Abs(other.gameObject.rigidbody.velocity.z) + Random.Range(1f,3f));
			}
		}
	}

}
