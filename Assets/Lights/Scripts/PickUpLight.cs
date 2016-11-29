using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class PickUpLight : MonoBehaviour {
	
	private UnityStandardAssets.Characters.FirstPerson.FirstPersonController FPController;
	private GameManager gameManager;
	private GameObject startEndEvts;

	public GameObject v_spotlight;
	public GameObject spotlight;
	private big_light_music_loop lightMusicLoopScript;

	private PlaygroundParticlesC pWithTrails;
	PlaygroundParticlesC pDust;

	// Animation points
	private float timeAnimStart = 0.0f;
	private float climaxTime = 6.0f;
	private float endingTime = 20.0f;

	// Use this for initialization
	void Start () {
		gameManager = (GameManager)GameObject.Find ("Game_Manager").GetComponent(typeof(GameManager));
		startEndEvts = GameObject.Find ("Start_And_End_Events");
		if (startEndEvts == null)
			Debug.Log ("In PickUpLight.cs:: Couldn't find \"Start_And_End_Events\"");
		FPController = (UnityStandardAssets.Characters.FirstPerson.FirstPersonController)GameObject.Find ("Player").GetComponent (typeof(UnityStandardAssets.Characters.FirstPerson.FirstPersonController));

		if (v_spotlight == null || spotlight == null) { // Must be defined if they're not in the group e.g. for the domino puzzle light
			v_spotlight = transform.FindChild ("V-Light Spot").gameObject;
			spotlight = transform.FindChild ("Spot light").gameObject;
		}

		lightMusicLoopScript = GetComponent<big_light_music_loop> ();

		PlaygroundParticlesC[] pickupParticles = this.GetComponentsInChildren<PlaygroundParticlesC> ();
		pWithTrails = new PlaygroundParticlesC();
		pDust = new PlaygroundParticlesC ();
		foreach (PlaygroundParticlesC p in pickupParticles) {
			if (p.name == "Pickup") {
				pWithTrails = p;
				p.emit = false; // Disable on startup
			} else {
				pDust = p;
			}
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void pickUp() {
		Debug.Log ("PickUpLight: Pick Up Signal");
		pickUpLight (this.gameObject);
		lightMusicLoopScript.Stop_light_loop ();
	}

	void pickUpLight(GameObject light) {
		StartCoroutine("pickUpAnimation");

	}

	/*void pickUpAnimation(GameObject light) {
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
	}*/

	IEnumerator pickUpAnimation() {


		FPController.movementEnabled = false; // Stop player movement

		Vector3 randomVelocityMin = new Vector3(-5f,1,-5f);   // The minimum random velocity
		Vector3 randomVelocityMax = new Vector3(5f,5f,5f);    // The maximum random velocity
		Color32 color = Color.white;                          // The color of particles

		//if (pWithTrails != null)
		//	pWithTrails.emit = true;

		//float timeStopEmit = 4.0f;
		//float timeStartClimaxEmit = 5.5f;

		timeAnimStart = Time.time; // Time animation initialised

		gameManager.pickedUpLightMusic ();
		transform.GetComponent<AudioSource>().Play(); // Trigger sound

		pWithTrails.Emit (3, pWithTrails.transform.position, randomVelocityMin, randomVelocityMax, color);
		while (Time.time - timeAnimStart < 1.1f) {
			yield return null;
		}
		pWithTrails.Emit (3, pWithTrails.transform.position, randomVelocityMin, randomVelocityMax, color);
		while (Time.time - timeAnimStart < 2.2f) {
			yield return null;
		}
		pWithTrails.Emit (3, pWithTrails.transform.position, randomVelocityMin, randomVelocityMax, color);
		while (Time.time - timeAnimStart < 3.3f) {
			yield return null;
		}
		pWithTrails.Emit (3, pWithTrails.transform.position, randomVelocityMin, randomVelocityMax, color);
		while (Time.time - timeAnimStart < 3.3f) {
			yield return null;
		}
		pWithTrails.Emit (3, pWithTrails.transform.position, randomVelocityMin, randomVelocityMax, color);


		//pWithTrails.Emit (3, pWithTrails.transform.position, randomVelocityMin, randomVelocityMax, color);

		//while (Time.time - timeAnimStart < 1.0f) {
		//	yield return null;
		//}

		//pWithTrails.Emit (3, pWithTrails.transform.position, randomVelocityMin, randomVelocityMax, color);


		//pWithTrails.emit = false; // Pause before climax

		pDust.emit = false; // Timing-wise here is a good place to stop the centre emission

		while (Time.time - timeAnimStart < (climaxTime)) {
			yield return null;
		}

		pWithTrails.Emit (40, pWithTrails.transform.position, randomVelocityMin, randomVelocityMax, color);
		//pWithTrails.particleCount = 100; // Release loads and loads :J
		//pWithTrails.emit = true;
		//pWithTrails.Emit(100);




		// Turn off spotlight
		v_spotlight.gameObject.SetActive(false);

		FPController.movementEnabled = true; // Reenable player movement



		yield return new WaitForSeconds (0.5f);

		// Maybe wait a tiny bit more before doing this next bit? ~
		gameManager.pickedUpLight (); // Add the light functionality to the lantern

		//pWithTrails.emit = false; // Turn off


		// Trigger ending? New vlight beacon and minipickup trail; add trigger for book house animation, etc.
		Debug.Log("From PickUpLight.cs:: Current state is: " + GameManager.progressState);
		if (GameManager.progressState == 4) {
			startEndEvts.transform.SendMessage ("triggerEnding", SendMessageOptions.DontRequireReceiver);
		}
	

		// Reduce spotlight
		float timeStartSpotlightAnim = Time.time;
		float timeTotalSpotlightAnim = 5.0f;
		float initialIntensity = spotlight.GetComponent<Light> ().intensity;
		while (Time.time - timeStartSpotlightAnim < timeTotalSpotlightAnim) {
			float complete = (Time.time - timeStartSpotlightAnim) / timeTotalSpotlightAnim; // 0 to 1 based on completion
			spotlight.GetComponent<Light>().intensity = Mathf.Lerp(initialIntensity, 0.0f, complete);
			yield return null;
		}

		yield return new WaitForSeconds(5.0f);
		pWithTrails.transform.parent.gameObject.SetActive (false); // Terminate everything (in case bugged particles remaining)

		yield return null;



		/*
		// Deactivate original swirl dust
		var emit = dustDeactivate.emission;
		emit.enabled = false;
		dustActivate.Play (); // Doesn't work yet - already playing

		bool triggeredAdd = false;
		bool triggeredLanternSwirl = false;

		// Triggered: make faster; decrease lifetime; spawn more?
		int numParticles = dustActivate.GetParticles (particles);
		//		int numLanternDustParticles = lanternSwirl.GetParticles (lanternSwirlParticles);
		for (int i = 0; i < numParticles; i++) {
			particles [i].startLifetime = 3.2f; // Do not allow to linger
		}

		while (playingAnim) {
			numParticles = dustActivate.GetParticles (particles);
			//			numLanternDustParticles = lanternSwirl.GetParticles (lanternSwirlParticles);

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
				//				Vector3 LocalLantCentre = lanternSwirl.transform.localPosition; // These are rendered in local space - need to reverse pos for orirgin
				// This doesn't work though.
				// Keep trying next week.

				if(emittingParticles) {
					stopEmission(); // Stop producing particles
					explode (); // Final burst (explosion)
				}

				// Trigger addition to lantern
				if (triggeredAdd == false) {
					triggeredAdd = true;
					gameManager.pickedUpLight (this.transform.parent.gameObject.name);
				}

			}

			// Start swirl around lantern
			if (Time.time - timeAnimStart < swirlTime && triggeredLanternSwirl == false) { 
				//				lanternSwirl.Play ();
				triggeredLanternSwirl = true;
			}

			if (Time.time - timeAnimStart > endingTime) // Stop coroutine
				playingAnim = false;

			yield return null;

		}
		terminateAnimation ();*/
	}

	/*
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

		//		temp = lanternSwirl.emission;
		//		temp.enabled = false;
	}*/


	/*
	// Delete gameObject
	void terminateAnimation() {
		this.transform.parent.gameObject.SetActive (false);
		//		lanternSwirl.transform.parent.gameObject.SetActive (false);
	}
	*/
}
