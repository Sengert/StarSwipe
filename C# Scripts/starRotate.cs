using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starRotate : MonoBehaviour {
	public Transform star;
	public float speed;


	// Update is called once per frame
	void FixedUpdate () {
		star.Rotate (0, speed, 0);
	}
}
