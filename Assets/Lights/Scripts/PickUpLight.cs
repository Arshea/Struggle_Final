using UnityEngine;
using System.Collections;

public class PickUpLight : MonoBehaviour {

	public GameObject lanternCentre;
	public ParticleSystem dustDeactivate; // Moves to player
	public ParticleSystem dustActivate; // Moves to player
	public ParticleSystem explosion; // Explosion effect

	public ParticleSystem lanternSwirl; // Dust swirls around lantern to trigger (yel, blu, gre, red - enum from lanternManager)
	private ParticleSystem.Particle[] lanternSwirlParticles; // Temp array for calculating new dust positions

	private Transform lanternCentreTrans;
	private ParticleSystem.Particle[] particles; // Temp array for calculating new dust positions

	// Particle dust to player lantern effect
	private float speedDuringAnim = 5.0f; // Speed to keep particles circling player
	private float speedEndAnim = 10.0f;	// Speed into lanten at end
	// and accurately stream to target

	private bool playingAnim = false; // Is animation playing?
	private bool emittingParticles = true; // Are there still particles emitting? For when they should be stopped

	// Animation points
	private float climaxTime = 6.0f;
	private float endingTime = 10.0f;
	private float swirlTime = 0.2f; // Time to start swirl around lantern after trigger

	private float timeAnimStart;

	// Use this for initialization
	void Start () {
		lanternCentreTrans = lanternCentre.transform;
		if(dustActivate != null)
		if (particles == null || particles.Length < dustActivate.maxParticles)
			particles = new ParticleSystem.Particle[dustActivate.maxParticles]; 

		if(lanternSwirl != null)
		if (lanternSwirlParticles == null || lanternSwirlParticles.Length < lanternSwirl.maxParticles)
			lanternSwirlParticles = new ParticleSystem.Particle[lanternSwirl.maxParticles]; 

		//lanternSwirl.SetActive (false); // Turn off dust swirl around lantern
	}

	// Update is called once per frame
	void Update () {

	}

	void pickUp() {
		//Debug.Log ("Pick up sigal received");
		pickUpLight (this.gameObject);
	}

	void pickUpLight(GameObject light) {
		//Debug.Log ("Picking up the thing\n");
		playingAnim = true; // Start dust towards player anim

		StartCoroutine("moveLightToPlayer");

		pickUpAnimation (light); // Start light burst anim

		light.transform.parent.GetComponent<AudioSource>().Play(); // Trigger sound
		LanternManager.pickingUpAudioTrigger = true; // Trigger music

		// Need to change - should probably access var in a GameManager instead
		//gameState++;
	}

	void pickUpAnimation(GameObject light) {
		// Start Unity-build animation for light source
		light.GetComponent<Animation>().Play ();

		// Turn off beacons
		Transform[] children = light.GetComponentsInChildren<Transform> ();
		foreach (Transform child in children) {
			if (child.gameObject.CompareTag ("Beacon")) {
				var temp = child.gameObject.GetComponent<ParticleSystem> ().emission;
				temp.enabled = false;
				//Debug.Log ("Found child");
			}
		}

		//updateLantern (light);
	}

	IEnumerator moveLightToPlayer() {
		// Deactivate original swirl dust
		var emit = dustDeactivate.emission;
		emit.enabled = false;
		dustActivate.Play (); // Doesn't work yet - already playing

		timeAnimStart = Time.time; // Time animation initialised
		bool triggeredAdd = false;
		bool triggeredLanternSwirl = false;

		// Triggered: make faster; decrease lifetime; spawn more?
		int numParticles = dustActivate.GetParticles (particles);
		int numLanternDustParticles = lanternSwirl.GetParticles (lanternSwirlParticles);
		for (int i = 0; i < numParticles; i++) {
			particles [i].startLifetime = 3.2f; // Do not allow to linger
		}

		while (playingAnim) {
			numParticles = dustActivate.GetParticles (particles);
			numLanternDustParticles = lanternSwirl.GetParticles (lanternSwirlParticles);

			if (Time.time - timeAnimStart < climaxTime) { // During animation
				for (int i = 0; i < numParticles; i++) {
					particles [i].velocity *= 0.0f; // remove circular motion
					particles [i].position = Vector3.Lerp (
						particles [i].position, lanternCentreTrans.position, 
						speedDuringAnim * (Time.deltaTime));
					// If too close to lantern, kill
					if (Vector3.Distance (particles[i].position, lanternCentreTrans.position) < 0.25f)
						particles [i].lifetime = 0.0f;
				}
				dustActivate.SetParticles (particles, numParticles);

			} else { // End of animation -- push quickly into lantern
				// Dust towards lantern from light source
				for (int i = 0; i < numParticles; i++) {
					particles [i].position = Vector3.Lerp (
						particles [i].position, lanternCentreTrans.position, 
						speedEndAnim * (Time.deltaTime));
					particles [i].startLifetime = 0.0f; // Doesn't work yet. Change this when it's fixed. something to do with local/global space rendering
				}
				dustActivate.SetParticles (particles, numParticles);

				// Dust swirl around lantern
				Vector3 LocalLantCentre = lanternSwirl.transform.localPosition; // These are rendered in local space - need to reverse pos for orirgin
				// This doesn't work though.
				// Keep trying next week.


				//Debug.Log ("pos: " + LocalLantCentre.x/* + ", " + LocalLantCentre.y, + ", " + LocalLantCentre.z*/);
				for (int i = 0; i < numLanternDustParticles; i++) {
					lanternSwirlParticles [i].position = Vector3.Lerp (
						lanternSwirlParticles [i].position, LocalLantCentre, 
						speedEndAnim * (Time.deltaTime));
					lanternSwirlParticles [i].startLifetime = 0.2f; // Do not allow to linger
				}
				lanternSwirl.SetParticles (lanternSwirlParticles, numLanternDustParticles);

				if(emittingParticles) {
					stopEmission(); // Stop producing particles
					explode (); // Final burst (explosion)
				}

				// Trigger addition to lantern
				if (triggeredAdd == false) {
					triggeredAdd = true;
					LanternManager.lightToAddName = this.transform.parent.gameObject.name;
				}

			}

			// Start swirl around lantern
			if (Time.time - timeAnimStart < swirlTime && triggeredLanternSwirl == false) { 
				lanternSwirl.Play ();
				triggeredLanternSwirl = true;
			}

			if (Time.time - timeAnimStart > endingTime) // Stop coroutine
				playingAnim = false;

			yield return null;

		}
		terminateAnimation ();
		yield return null;

	}

	void explode() {
		burst ();
		Invoke ("burst", 0.05f);
		Invoke ("burst", 0.1f);
	}

	void burst() {
		explosion.Emit (1);
	}

	// Stop particle emitters (allow them to fade)
	void stopEmission() {
		emittingParticles = false;

		ParticleSystem[] children = this.GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem child in children) {
			var emit = child.emission;
			emit.enabled = false;
		}
		// & parent
		var temp = this.GetComponent<ParticleSystem> ().emission;
		temp.enabled = false;

		temp = lanternSwirl.emission;
		temp.enabled = false;
	}

	// Delete gameObject
	void terminateAnimation() {
		this.transform.parent.gameObject.SetActive (false);
		lanternSwirl.transform.parent.gameObject.SetActive (false);
	}

}
