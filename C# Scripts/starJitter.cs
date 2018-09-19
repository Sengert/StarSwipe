using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starJitter : MonoBehaviour {
	public float MaxJitter;
	public float timer;
	public float timeAmount;
	public float finalTime;

    public AudioSource starExplosion;
	public int curColor;
	public int tarColor;
	public StarDriver starDriver;
	public List<Material> StarColors = new List<Material>();

	public List<GameObject> Trails = new List<GameObject> ();
	public List<ParticleSystem> BuildUps = new List<ParticleSystem> ();
	public List<ParticleSystem> ExplodeShells = new List<ParticleSystem> ();
	public List<ParticleSystem> ExplodeOut = new List<ParticleSystem> ();
	// Use this for initialization
	void Start () {
		curColor = Random.Range (0, 4);
		this.GetComponent<Renderer> ().material = StarColors [curColor];
		starDriver.color = StarColors [curColor].name;
		Trails [curColor].SetActive (true);
		tarColor = Random.Range (0, 4);
		if (tarColor == curColor) {
			while (tarColor == curColor) {
				tarColor = Random.Range (0, 4);
			}
		}

		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer += Time.deltaTime;
		if (timer > timeAmount	) {
			transform.localPosition = new Vector3 (Random.Range (-MaxJitter, MaxJitter)*(timer-timeAmount), 0, Random.Range (-MaxJitter, MaxJitter)*(timer-timeAmount));
            if (!starExplosion.isPlaying)
            {
                starExplosion.Play();
            }
			if (finalTime - timer > 1.4f)
				BuildUps[tarColor].Play ();
			if (timer > finalTime) {
				timer = 0;

				ExplodeShells[tarColor].Play ();
				ExplodeOut[tarColor].Play ();
				transform.localPosition = new Vector3 (0,0,0);
				Trails [curColor].SetActive (false);
				curColor = tarColor;
				Trails [curColor].SetActive (true);
				this.GetComponent<Renderer> ().material = StarColors [curColor];
				starDriver.color = StarColors [curColor].name;
				if (tarColor == curColor) {
					while (tarColor == curColor) {
						tarColor = Random.Range (0, 4);
					}
				}
			}
		}

	}
}
