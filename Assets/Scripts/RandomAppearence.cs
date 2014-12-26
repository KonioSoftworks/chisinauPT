using UnityEngine;
using System.Collections;

public class RandomAppearence : MonoBehaviour {

	public int intervalLimit = 1;

	// Use this for initialization
	void Start () {
		int i = Random.Range(0,intervalLimit);
		if(i != 1){
			gameObject.SetActive(false);
		}
	}
}
