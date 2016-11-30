using UnityEngine;
using System.Collections;

// Handles lantern events. Light pickups and lantern flares.
public class LanternManager : MonoBehaviour {

	// Burst attack
	public float cooldown = 2.0f; // Number of seconds between allowed shots

	// Need access to these from pretty much everywhere for onclick functionality
	public static float lanternRange = 15.0f; // 
	public static int ammunition = 0; // Ammunition currently available - always updated
	// Accessed in enemy AI script to determine a valid lantern attack

	// Audio
	public AudioSource[] lantern_audio_source;
	public AudioSource lantern_audio_source_stun_sweetner;
	public AudioClip[] lantern_audio_clips_stun_sweetner;

	// Burst mechanic
	//private float lightBurstLifetime = 1.0f;
	private int numBurst2Particles = 1; // Num particles for lantern burst
	private int numBurst3Particles = 10; // Num particles for lantern wind effect burst

	// Lantern centres and game state
	public GameObject[] lanternContents; // In enum order - lantern colour prefabs
	private ParticleSystem[] lightBurst2; // Ring effect
	private ParticleSystem[] lightBurst3; // Wind effect
	private ParticleSystem[] distortion; // Distortion effect
	private GameObject[] burstParticleContainer; // For overall rotation
	private bool[] lanternContentsAvailable; // Which lights are currently ready for use?

	//public enum Colours {Yellow = 0, Blue, Green, Red};

	/* Used for narration, do not remove */
	private bool has_triggered_narration = false;


	// Use this for initialization
	void Start () {
		// Initialise lantern contents
		if(lanternContents == null) lanternContents = GameObject.FindGameObjectsWithTag("LanternLight");

		// Start with empty lantern
		lanternContentsAvailable = new bool[]{false, false, false, false};

		// Put light burst effects into memory
		lightBurst2 = new ParticleSystem[4];
		lightBurst3 = new ParticleSystem[4];
		distortion = new ParticleSystem[4];

		burstParticleContainer = new GameObject[4]; // For overall rotation
		for (int i = 0; i < 4; i++) {
			burstParticleContainer [i] = new GameObject();
			burstParticleContainer [i] = lanternContents [i].transform.FindChild ("animation_shell"). 
				FindChild ("burstParticles").gameObject;
			if (lightBurst2 [i] = null)
				Debug.Log ("problem with burstParticleContainer");

			GameObject burstParticlesObj = lanternContents [i].transform.FindChild ("animation_shell").
				FindChild ("burstParticles").gameObject;

			Transform[] children = burstParticlesObj.GetComponentsInChildren<Transform> ();
			foreach (Transform child in children) {
				if (child.name == "burst2") {
					lightBurst2 [i] = child.GetComponent<ParticleSystem> ();
					//Debug.Log ("Found child: " + child.name);
				} else if (child.name == "burst3") {
					lightBurst3 [i] = child.GetComponent<ParticleSystem> ();
					//Debug.Log ("Found child: " + child.name);
				} else if (child.name == "distortion") {
					distortion [i] = child.GetComponent<ParticleSystem> ();
					//Debug.Log ("Found child: " + child.name);
				} else {
					//Debug.Log ("Found unexpected child in burstParticles: " + child.name);
				}
			}
			if (distortion [i] == null)
				Debug.Log ("LanternManager:: problem with distortion");
			if (lightBurst2 [i] == null)
				Debug.Log ("LanternManager:: problem with lightBurst2");
			if (lightBurst3 [i] == null)
				Debug.Log ("LanternManager:: problem with lightBurst3");
		}

		// Set all to inactive at first
		for (int i = 0; i < 4; i++) {
			Debug.Log (lanternContents [i].name);
			lanternContents [i].SetActive (false);
		}

		// Audio

		//lantern_audio_source.clip = burstSounds [0]; // Default

	}

	// Update is called once per frame
	void Update () {
		if (Time.timeScale > 0.0f) {
			if (Input.GetButtonDown ("Interact")) {
				int index = availableLight ();
				if (index >= 0) {
					int random = Random.Range (1, lantern_audio_source.Length);
					lantern_audio_source [random].loop = false;
					lantern_audio_source [random].Play ();
					lantern_audio_source_stun_sweetner.clip = lantern_audio_clips_stun_sweetner [Random.Range (1, lantern_audio_clips_stun_sweetner.Length)];
					lantern_audio_source_stun_sweetner.Play ();

					Debug.Log ("Found available light in slot " + index);
					burstHandle (index);

					Debug.Log ("Disabling light " + index);
					lanternContentsAvailable [index] = false; // Disable burst ability
					StartCoroutine (lightBurstCooldown (index)); // Enable after cooldown
				} else {
					// Play empty
					lantern_audio_source [0].loop = false;
					lantern_audio_source [0].Play ();
				}
			}
		}
	}

	int availableLight() {
		for(int i = 0; i < GameManager.progressState; i++) {
			if(lanternContentsAvailable[i])
				return i;
		}
		return -1; // None available
	}

	void burstHandle(int index) {
		resetParticleRotation (burstParticleContainer[index]);
		burst2 (lightBurst2[index]); // Centre circle
		burst3 (lightBurst3[index]); // Wind
		burstDistort(distortion[index]); // Distortion effect
	}

	void resetParticleRotation(GameObject burstParticleContainer) {
		burstParticleContainer.transform.eulerAngles = new Vector3 (90.0f, 0.0f, 0.0f);
	}

	// Again this can be done with 1 function and an array once I know all the parameters
	void burst2(ParticleSystem lightBurst) {
		lightBurst.Emit (numBurst2Particles);
	}

	void burst3(ParticleSystem lightBurst) {
		lightBurst.Emit (numBurst3Particles);
	}

	void burstDistort(ParticleSystem lightBurst) {
		lightBurst.Emit (2);
	}

	public void addLight() {
		Debug.Log ("Enabling light (from void addLight) ");

		Debug.Log ("LanternManager: Pick Up Signal");

		lanternContentsAvailable [GameManager.progressState] = true;
		ammunition++;
		lanternContents [GameManager.progressState].SetActive (true); // Change to animation
	}



	IEnumerator lightBurstCooldown(int index) { //Add cooldown timer to lantern light burst
		yield return new WaitForEndOfFrame();
		ammunition--;


		// Disable emission
		Transform emissiveSphere = lanternContents [index].transform.FindChild ("animation_shell").
			FindChild ("centre");
		emissiveSphere.localScale /= 2.0f;
		Behaviour halo = (Behaviour)emissiveSphere.gameObject.GetComponent ("Halo");
		halo.enabled = false;

		Debug.Log ("Cooldown started");

		// Wait
		yield return new WaitForSeconds (cooldown);

		emissiveSphere.localScale *= 2.0f;
		halo.enabled = true;

		Debug.Log ("Cooldown ended");

		Debug.Log ("Enabling light " + index);
		lanternContentsAvailable[index] = true;
		ammunition++;
		Debug.Log ("Remaining ammunition: " + ammunition);
	}

	/*
	void updateLantern(GameObject otherObject)
	{
		Color toAdd = otherObject.GetComponent<ParticleSystem> ().startColor;
		for (int i = 0; i < 3; i++)
		{
			//Debug.Log("Colour: " + i + ": " + toAdd[i]);
			if (toAdd[i] < 0.9) toAdd[i] = 0; // Remove secondary colours
		}
		Color toDecrease = new Color(0.0f, 0.0f, 0.0f);
		//lanternCentre.startColor -= toDecrease;
		//lanternCentre.startColor += toAdd / 3;
		//lanternLight.range *= 1.1f;
		//lanternLight.intensity *= 1.1f;
	}*/
}
