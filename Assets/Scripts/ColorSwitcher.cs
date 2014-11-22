using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorSwitcher : MonoBehaviour {

	public List<Color> colors;

	// Use this for initialization
	void Start () {
		if(colors.Count > 0)
			renderer.material.color = colors[Random.Range(0,colors.Count)];
	}
}
