using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {

	public int maxThrust = 6;//The ceiling amount of speed to apply to a newly spawned asteroid
	public List<GameObject> asteroids = new List<GameObject>();//Data container for asteroid gameobjects

	private List<GameObject> activeAsteroids;
	private GameObject curAsteroid;//The most recently spawned asteroid
	private Vector3 upperLeftCorner;
	private Vector3 upperRightCorner;
	private Vector3 bottomLeftCorner;
	private Vector3 bottomRightCorner;
	private Vector3 dirUpperLeftCorner;
	private Vector3 dirUpperRightCorner;
	private Vector3 dirBottomLeftCorner;
	private Vector3 dirBottomRightCorner;

	private Vector3 spawnPosition;

    private int side;//1 = left, 2 = right, 3 = top, 4 = bottom

	// Use this for initialization
	void Start () {
		upperLeftCorner = GameObject.Find("/SpawnBoundaries/UpperLeftCorner").transform.position;
		upperRightCorner = GameObject.Find("/SpawnBoundaries/UpperRightCorner").transform.position;
		bottomLeftCorner = GameObject.Find("/SpawnBoundaries/BottomLeftCorner").transform.position;
		bottomRightCorner = GameObject.Find("/SpawnBoundaries/BottomRightCorner").transform.position;

		dirUpperLeftCorner = GameObject.Find("/SpawnBoundaries/DirectionalUpperLeftCorner").transform.position;
		dirUpperRightCorner = GameObject.Find("/SpawnBoundaries/DirectionalUpperRightCorner").transform.position;
		dirBottomLeftCorner = GameObject.Find("/SpawnBoundaries/DirectionalBottomLeftCorner").transform.position;
		dirBottomRightCorner = GameObject.Find("/SpawnBoundaries/DirectionalBottomRightCorner").transform.position;
		Debug.Log ("Initialized upperLeftCorner at : " + GameObject.Find("/SpawnBoundaries/UpperLeftCorner").transform.position);

		activeAsteroids = gameObject.GetComponent<GameDriver> ().activeAsteroids;//Set a reference to the list of spawned asteroids to push asteroids into

		if(asteroids.Count == 0){
			Debug.LogError ("The list 'asteroids' contained in 'AsteroidSpawner.cs' attached to '" + gameObject.name + "' is empty. Please add asteroid gameobjects to the list.");
		}
	}


    public void SpawnAsteroid() {

		if(asteroids.Count == 0){
			Debug.Log ("The max amount of asteroids are currently spawned.");
			return;
		}

		//Compute which side to spawn an asteroid on
        side = Random.Range(1, 5);//Max bound for ints is exclusive so this will pick either 1, 2, 3, or 4.

		//Compute an origin spawn point on the given side
        switch (side) {

		    case 1://left side
			spawnPosition = new Vector3 (upperLeftCorner.x, upperLeftCorner.y, Random.Range (bottomLeftCorner.z, upperLeftCorner.z));
                break;

            case 2://right side
			spawnPosition = new Vector3(upperRightCorner.x, upperRightCorner.y, Random.Range(bottomRightCorner.z, upperRightCorner.z));
                break;

            case 3://top side
			spawnPosition = new Vector3(Random.Range(upperLeftCorner.x, upperRightCorner.x), upperLeftCorner.y, upperLeftCorner.z);
                break;

            case 4://bottom side
			spawnPosition = new Vector3(Random.Range(bottomLeftCorner.x, bottomRightCorner.x), bottomLeftCorner.y, bottomLeftCorner.z);
                break;

            default:
                Debug.LogError("Could not compute the correct side to spawn an asteroid on.");
                break;
        }

		int asteroidsIndex = Random.Range (0, asteroids.Count - 1);

		curAsteroid = asteroids.ElementAt (asteroidsIndex);


		//TODO: Reset back to old pooled position when the star catches the asteroid.
		curAsteroid.transform.position = spawnPosition;
		curAsteroid.SetActive (true);
		asteroids.RemoveAt (asteroidsIndex);
		activeAsteroids.Add (curAsteroid);

		curAsteroid.GetComponent<SphereCollider> ().radius = Random.Range (.0120f, .0149f);
		ApplyForce(curAsteroid);
    }

	//Ramps up starting speed of an asteroid based on how high your score is.
	//The force is calculated as a percentage of the absolute maximum force
	//based on score. i.e.(your score is 1000 so the asteroid can spawn with a speed at most 10% of the maximum allowed force.)
	private void ApplyForce(GameObject asteroid){
		Vector3 dir = new Vector3(Random.Range(dirUpperLeftCorner.x, dirUpperRightCorner.x), dirUpperLeftCorner.y, Random.Range(dirUpperLeftCorner.z, dirBottomLeftCorner.z));
		dir = dir - asteroid.transform.position;

		//20 was the original spawn value of all asteroids, this will allow the starting speed to ramp up over time
		float force = Random.Range(20, maxThrust);

		asteroid.GetComponent<Rigidbody> ().AddForce (dir * force);
		maxThrust++;
	}
		
}
