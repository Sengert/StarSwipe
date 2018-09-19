using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteScroll : MonoBehaviour {
	public float ScrollX = .5f;
	public float ScrollY = .5f;

	public float OffsetX;
	public float OffsetY;

	// Update is called once per frame
	void Update () {
     
        OffsetX = Time.unscaledTime * ScrollX;
        OffsetY = Time.unscaledTime * ScrollY;
		GetComponent<RawImage>().uvRect= new Rect (OffsetX, OffsetY,1,1);
	}
}
