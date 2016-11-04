using UnityEngine;
using System.Collections;

public class ParticleDirector : MonoBehaviour {
	// Attracts particles to player pos

	public ParticleSystem p;
	public float affectDistance;
	public GameObject player;
	public float speed = 3.0f;

	private ParticleSystem.Particle[] particles;
	private float sqrDistance;
	private Transform trans;


	// Use this for initialization
	void Start () {
		trans = player.transform;
		sqrDistance = affectDistance * affectDistance;
		if (particles == null || particles.Length < p.maxParticles)
			particles = new ParticleSystem.Particle[p.maxParticles]; 
	}
	
	void LateUpdate () {
		float currDist;
		int numParticles = p.GetParticles (particles);
		for(int i = 0; i < numParticles; i++) {
			currDist = Vector3.SqrMagnitude(
				trans.position - particles[i].position);
			if(currDist < sqrDistance) {
				particles[i].position = Vector3.Lerp(particles[i].position, 
					trans.position, speed * Time.deltaTime);
			}
			//Debug.Log("Doing a thing");
		}
		p.SetParticles (particles, numParticles);
	}
}
