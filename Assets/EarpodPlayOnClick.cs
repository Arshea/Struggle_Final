using UnityEngine;
using System.Collections;

public class EarpodPlayOnClick : MonoBehaviour {

	//public GameObject player;
//	private bool triggered = false;
	private float startTime = 0.0f; // Time of start of music fadeout;

	// Use this for initialization
	void Start () {
		//if(player == null) player = GameObject.Find ("Player");
		this.gameObject.GetComponentInParent<InteractionManager>().trigger_cooldown_time = 4.0f;
		GetComponent<AudioSource> ().mute = true;
	}

	// Update is called once per frame
	void Update () {
		//if (!triggered/*move this to later if want to reset timer on second click*/ && Input.GetButtonDown ("Interact") && LanternManager.ammunition > 0) {
			/*if (Vector3.Distance (player.transform.position, transform.position) < LanternManager.lanternRange) {

				StartCoroutine ("playMusic");
				triggered = true;

				// Change to cooler animation and maybe music note particle effects??
				this.GetComponent<Rigidbody>().AddForce(1800 * Vector3.up);
				this.GetComponent<Rigidbody>().AddTorque(1800 * Vector3.up);

			}
		}*/
	}
	void TriggerInteraction() {
		StartCoroutine ("playMusic");
		StartCoroutine ("playEarpodNarration");
		//triggered = true;
		// Change to cooler animation and maybe music note particle effects??
		this.GetComponent<Rigidbody>().AddForce(1800 * Vector3.up);
		this.GetComponent<Rigidbody>().AddTorque(1800 * Vector3.up);
	}

	IEnumerator playEarpodNarration() {
		this.gameObject.GetComponentInParent<InteractionManager> ().narration_triggered = true;
		yield return new WaitForSeconds (1.0f);
		MusicManager musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
		musicManager.SendMessage("playNarrationOfTrigger", ObjectTriggerType.EARPHONES,SendMessageOptions.DontRequireReceiver);
		this.gameObject.GetComponentInParent<InteractionManager> ().narration_triggered = false;

	}
	IEnumerator playMusic() {
		GetComponent<AudioSource> ().mute = false;
		//yield return new WaitForSeconds (4);

		startTime = Time.time;
		float endTime = this.gameObject.GetComponentInParent<InteractionManager> ().trigger_cooldown_time;
		float startVol = GetComponent<AudioSource> ().volume;
		while (Time.time - startTime < endTime) {
			float complete = (Time.time - startTime) / endTime; // Between 0 and 1
			GetComponent<AudioSource> ().volume = Mathf.Lerp (startVol, 0.0f, complete);
			yield return null;
		}

		// Reset
		GetComponent<AudioSource> ().mute = true;
		GetComponent<AudioSource> ().volume = startVol;
		//triggered = false;

		yield return null;
	}
}
