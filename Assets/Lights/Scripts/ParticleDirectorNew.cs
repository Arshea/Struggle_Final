using UnityEngine;
using System.Collections;

public class ParticleDirectorNew : MonoBehaviour {
	// Attracts particles to player pos

	public ParticleSystem from;
	public float affectDistance = 15.0f;
	public GameObject to;
	public float speed = 3.0f;

	private float decel = 0.96f; // need to decelerate particles to remove their initial velocity
								// and accurately stream to target
	private ParticleSystem.Particle[] particles;
	private float sqrDistance;
	private Transform trans;


	// Use this for initialization
	void Start () {
		trans = to.transform;
		sqrDistance = affectDistance * affectDistance;
		if (particles == null || particles.Length < from.maxParticles)
			particles = new ParticleSystem.Particle[from.maxParticles]; 
	}

	void LateUpdate () {
		float currDist;
		int numParticles = from.GetParticles (particles);
		currDist = Vector3.SqrMagnitude (trans.position - from.transform.position);
		if (currDist < sqrDistance) {
			for (int i = 0; i < numParticles; i++) {
				particles [i].position = Vector3.Lerp (particles [i].position, 
					trans.position, speed * Time.deltaTime);
				particles [i].velocity = decel * particles [i].velocity;
				//Debug.Log("Doing a thing");
			}
			from.SetParticles (particles, numParticles);
		}
	}
}
