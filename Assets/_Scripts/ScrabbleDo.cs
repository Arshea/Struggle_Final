using UnityEngine;
using System.Collections;

public class ScrabbleDo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponentInParent<InteractionManager>().trigger_cooldown_time = 4.0f;
	}

	// Update is called once per frame
	void Update () {

	}

	void TriggerInteraction() {
		StartCoroutine ("playScrabbleNarration");
	}

	IEnumerator playScrabbleNarration() {
		this.gameObject.GetComponentInParent<InteractionManager> ().narration_triggered = true;
		yield return new WaitForSeconds (0.5f);
		MusicManager musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
		musicManager.SendMessage("playNarrationOfTrigger", ObjectTriggerType.SCRABBLE,SendMessageOptions.DontRequireReceiver);
		//this.gameObject.GetComponentInParent<InteractionManager> ().narration_triggered = false;

	}
}
