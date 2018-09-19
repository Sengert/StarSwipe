using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(PlayerPrefs.HasKey("Tutorial") == false){
			PlayerPrefs.SetInt ("Tutorial", 0);
		}
		//Enter main game loop
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

}
