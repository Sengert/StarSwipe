using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

	public string color;
	public float originalSpeed;

	private float speed;
	private bool isInitialized = false;

	Rigidbody rb;

	void Awake(){
		rb = gameObject.GetComponent<Rigidbody> ();
		gameObject.transform.position = GameObject.Find ("Pooling Position").transform.position;
		gameObject.SetActive (false);
	}

	void Update(){

		if (!isInitialized) {
			return;
		}

		gameObject.GetComponent<Rigidbody> ().velocity = gameObject.GetComponent<Rigidbody> ().velocity.normalized * originalSpeed;
		speed = rb.velocity.magnitude;
	}

	void OnTriggerEnter(Collider other){

		if(other.gameObject.name != "AsteroidEnterTrigger"){
			return;
		}

		originalSpeed = rb.velocity.magnitude;
		isInitialized = true;
		gameObject.GetComponent<SphereCollider> ().isTrigger = false;

		//DEBUG
		rb.mass = 0.0f;
	}

	public void Reset(){
		originalSpeed = 0;
		speed = 0;
		isInitialized = false;
	}
		
}
