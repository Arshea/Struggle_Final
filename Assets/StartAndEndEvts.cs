using UnityEngine;
using System.Collections;


// This is triggered from PickUpLight when GameManager returns the correct progress state
public class StartAndEndEvts : MonoBehaviour {

	public GameObject bookHouseTrigger;
	public GameObject lanternLightSource;

	private Transform[] contents;
	private MusicManager musicManager;
	private GameObject[] ambientLights;
	private UnityStandardAssets.Characters.FirstPerson.FirstPersonController FPController;


	// Use this for initialization
	void Start () {
		bookHouseTrigger.SetActive (false); // Enable trigger collider when 4th light is collected

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

		float startIntensity = 2.5f;
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
		bookHouseTrigger.SetActive (true);
	}

	public void Goodbye() {
		Debug.Log ("From StartAndEndEvts:: Bye!!!! <3 \\o/");
		StartCoroutine ("playLastNarration");
		MusicManager musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
		musicManager.SendMessage("playNarrationOfTrigger", ObjectTriggerType.STORY_END,SendMessageOptions.DontRequireReceiver);
		// Do the ending coroutine
	}

	IEnumerator playLastNarration() {
		Debug.Log ("Waving goodbye now.");

		MusicManager musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
	//	yield return new WaitForSeconds (musicManager.narration_clips [5].length);

		float endOverlayIntensity = 2.5f;
		float startAmbientIntensity = ambientLights [0].GetComponent<Light> ().intensity;
		float startLanternIntensity = lanternLightSource.GetComponent<Light> ().intensity;

		float startTime = Time.time;
		float endTime = 5.0f; // Added 1 second because it feels a bit better
		float progress = 0.0f;



		while (Time.time - startTime < endTime) {

			progress = (Time.time - startTime) / endTime;
			//Debug.Log ("Completion: " + progress + " currently at " + (Time.time-startTime) + " ending at " + endTime);
			// Overlay
			UnityStandardAssets.ImageEffects.ScreenOverlay.intensity = Mathf.Lerp (0.0f, endOverlayIntensity, progress);

			// Lighting (complements overlay pattern - otherwise fully transparent in places)
			foreach (GameObject al in ambientLights) {
				al.GetComponent<Light> ().intensity = Mathf.Lerp (startAmbientIntensity, 0.0f, progress);
			}

			lanternLightSource.GetComponent<Light> ().intensity = Mathf.Lerp (startLanternIntensity, 0.0f, progress);


			yield return null;
		}

		FPController.movementEnabled = false;

		while (Time.time - startTime < musicManager.narration_clips [5].length + 2.0f) {
			yield return null;
		}

		Application.LoadLevel("Main_Menu");


		yield return null;

	}
}
