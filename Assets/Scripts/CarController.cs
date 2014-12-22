using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {

	public float velocity = 30f;

	private float timeToChange = 0f;
	private float speedToChange = 0f;

	// Use this for initialization
	void Start () {
		velocity = Random.Range(10f,25f);
		setVelocity(velocity);
	}

	void Update() {
		if(timeToChange > 5f){
			timeToChange = 0;
			speedToChange = Random.Range(-4f,4f);
		}
		timeToChange += Time.deltaTime;
		setVelocity(Mathf.Lerp(10f,25f,Mathf.Abs(velocity) + speedToChange*Time.deltaTime));
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
