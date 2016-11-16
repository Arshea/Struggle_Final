using UnityEngine;
using System.Collections;

public class EarpodPlayOnClick : MonoBehaviour {

	public GameObject player;
	private bool triggered = false;
	private float startTime = 0.0f; // Time of start of music fadeout;

	// Use this for initialization
	void Start () {
		if(player == null) player = GameObject.Find ("Player");

		GetComponent<AudioSource> ().mute = true;
	}

	// Update is called once per frame
	void Update () {
		if (!triggered && Input.GetButtonDown ("Interact") && LanternManager.ammunition > 0) {
			if (Vector3.Distance (player.transform.position, transform.position) < LanternManager.lanternRange) {

				if (!triggered) {
					StartCoroutine ("playMusic");
					triggered = true;
				}
			}
		}
	}

	IEnumerator playMusic() {
		GetComponent<AudioSource> ().mute = false;
		yield return new WaitForSeconds (4);

		startTime = Time.time;
		float endTime = 2.0f;
		float startVol = GetComponent<AudioSource> ().volume;
		while (Time.time - startTime < endTime) {
			float complete = (Time.time - startTime) / endTime; // Between 0 and 1
			GetComponent<AudioSource> ().volume = Mathf.Lerp (startVol, 0.0f, complete);
			yield return null;
		}

		// Reset
		GetComponent<AudioSource> ().mute = true;
		GetComponent<AudioSource> ().volume = startVol;
		triggered = false;

		yield return null;
	}
}
