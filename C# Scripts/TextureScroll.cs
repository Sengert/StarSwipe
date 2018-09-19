using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour {
	public float ScrollX = .5f;
	public float ScrollY = .5f;
	
	// Update is called once per frame
	void FixedUpdate () {
		float OffsetX = Time.time * ScrollX;
		float OffsetY = Time.time * ScrollY;
		GetComponent<Renderer> ().material.mainTextureOffset= new Vector2 (OffsetX, OffsetY);
	}
}
