using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDriver : MonoBehaviour {
	public float flickForce;
	public Transform startFlick;
	public Transform endFlick;
	public float flickTimer;
	private bool flickBool;
	private bool startMove;
	public float maxDistanceGrab;
	public GameDriver gameDriver;
	public string color;
	private Vector3 startV3;
	private Vector3 direction;
	public static int bankShot;
    public AudioSource starSwipeSound;
    public GameObject soundEffectManager;
    public UIDriver uiDriver;

    public static bool bankShotted;
    public static bool isPaused;

	// Use this for initialization
	void Start () {
        isPaused = false;
		flickBool = false;
		bankShot = 1;

		if(gameDriver == null){
			Debug.LogError ("StarDriver is missing a reference to GameDriver, please drag a reference in.");
		}

	}
	// Update is called once per frame
	void Update () {

        if (!isPaused) {

            if (!startMove) {
                startFlick.position = new Vector3(transform.position.x, 3, transform.position.z);
            }

            if (Input.GetButtonDown("Fire1")) {
                Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (Vector3.Distance(v3, startFlick.position) < maxDistanceGrab) {
                    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    CancelInvoke("cancelBankShot");

                    if (!GameDriver.lerping) {
                        bankShot = 1;
                        bankShotted = false;
                    }

                    startMove = true;
                    flickBool = true;
                    startFlick.position = v3;
                    startV3 = startFlick.position;
                }
                else {
                    flickBool = false;
                    startMove = false;
                }
            }

            if (Input.GetButtonUp("Fire1") && startMove) {

                Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                endFlick.transform.position = v3;
                startFlick.LookAt(endFlick);
                ForceAdd();
            }

            if (flickBool) {
                flickTimer += Time.deltaTime;
            }
        }	
	}

	private void ForceAdd(){

		direction = (startV3 - endFlick.position).normalized;
		startFlick.position = new Vector3 (transform.position.x, 3, transform.position.z);
		float dist = Vector3.Distance (startV3, endFlick.position);

        if (flickTimer < .02) {
            flickTimer = .02f;
        }
			
        if (dist > 10) {
            dist = 10;
        }

        float forceShot = ((flickForce / (flickTimer * 80)) * dist * Time.deltaTime);

        if (forceShot > 40){
            forceShot = 40;
        }

        gameObject.GetComponent<Rigidbody>().AddForce(-direction * forceShot, ForceMode.Impulse);

        if (soundEffectManager.activeInHierarchy){
            float vol = (flickForce / (flickTimer * 80)*dist)/900;
            starSwipeSound.volume = vol;
            starSwipeSound.Play();

        }
        
		flickBool = false;
		flickTimer = 0;
		startMove = false;
	}

	void OnTriggerEnter(Collider other){

		if (other.tag == "Asteroid") {
			gameDriver.AsteroidCollision (this.gameObject, other.gameObject);
		}
		else if(other.tag == "Boundary"){
			BankShot ();
		}
		else {
			return;
		}
	}

	public void BankShot(){
		bankShot = 2;
        bankShotted = true;
		CancelInvoke ("cancelBankShot");
		Invoke ("cancelBankShot", 1.4f);
	}

	public void cancelBankShot(){

        if (GameDriver.lerping == false){
            bankShot = 1;
            gameDriver.GetComponent<UIDriver>().UpdateMultiplierText(StarDriver.bankShot);
        }
        else{
            bankShot = 2;
        }

        bankShotted = false;
    }
}
