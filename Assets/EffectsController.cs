using UnityEngine;
using System.Collections;

public class EffectsController : MonoBehaviour {

	public GameObject speed;
	public GameObject coins;
	public AudioClip coinAudio;

	private GameObject player;
	private PlayerController pcontroller;
	private AudioSource coinsAs;
	private int money;

	// Use this for initialization
	void Awake() {
		findPlayerIfNotExist();
		coinsAs = gameObject.AddComponent<AudioSource>();
		coinsAs.clip = coinAudio;
		money = 0;
	}
	
	// Update is called once per frame
	void Update () {
		findPlayerIfNotExist();
		if (pcontroller) {
			if(money != pcontroller.money)
				coinEffect();
			coins.guiText.text = pcontroller.money.ToString() + " lei";	
			speed.guiText.text = (Mathf.RoundToInt(pcontroller.getVelocity() * 2.6f)).ToString() + "km/h";
		}
	}

	void findPlayerIfNotExist(){
		if(player)
			return;
		player = GameObject.FindGameObjectWithTag ("Player");
		if(player){
			pcontroller = player.GetComponent<PlayerController>();
		}
	}

	public void coinEffect(){
		coinsAs.Play();
		money = pcontroller.money;
	}

}
