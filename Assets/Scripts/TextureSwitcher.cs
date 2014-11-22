using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureSwitcher : MonoBehaviour {

	public List<Texture> textures;

	void Start () {
		if(textures.Count > 0)
			renderer.material.mainTexture = textures[Random.Range(0,textures.Count-1)];
	}

}