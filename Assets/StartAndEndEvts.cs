using UnityEngine;
using System.Collections;


// This is triggered from PickUpLight when GameManager returns the correct progress state
public class StartAndEndEvts : MonoBehaviour {

	private Transform[] contents;
	private MusicManager musicManager;
	private GameObject[] ambientLights;
	private UnityStandardAssets.Characters.FirstPerson.FirstPersonController FPController;


	// Use this for initialization
	void Start () {
		musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
		FPController = (UnityStandardAssets.Characters.FirstPerson.FirstPersonController)GameObject.Find ("Player").GetComponent (typeof(UnityStandardAssets.Characters.FirstPerson.FirstPersonController));
		ambientLights = GameObject.FindGameObjectsWithTag ("AmbientLight");

		// Ending event !!!
		contents = transform.GetComponentsInChildren<Transform> ();
		foreach (Transform o in contents) {
			if(o.name != this.name)
				o.gameObject.SetActive (false);
		}

		// Starting event !!!
		StartCoroutine("startFadeIn");


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator startFadeIn() {
		// Freeze movement
		FPController.movementEnabled = false;

		float startIntensity = 3.5f;
		UnityStandardAssets.ImageEffects.ScreenOverlay.intensity = startIntensity;
		float endLightIntensity = ambientLights [0].GetComponent<Light> ().intensity;

		float startTime = Time.time;
		float endTime = startTime + musicManager.narration_clips [0].length + 1.0f; // Added 1 second because it feels a bit better
		float progress = 0.0f;



		while (Time.time - startTime < endTime) {
			progress = (Time.time - startTime) / endTime;

			// Overlay
			UnityStandardAssets.ImageEffects.ScreenOverlay.intensity = Mathf.Lerp (startIntensity, 0.0f, progress);

			// Lighting (complements overlay pattern - otherwise fully transparent in places)
			foreach (GameObject al in ambientLights) {
				al.GetComponent<Light> ().intensity = Mathf.Lerp (0.0f, endLightIntensity, progress);
			}

			yield return null;
		}

		FPController.movementEnabled = true;
		yield return null;
	}

	public void triggerEnding() {
		Debug.Log ("From StartAndEndEvts:: Triggering ending");
		foreach (Transform o in contents) {
			o.gameObject.SetActive (true);
		}
	}
}
