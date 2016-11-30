using UnityEngine;
using System.Collections;

public class FathersDayNarration : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponentInParent<InteractionManager>().trigger_cooldown_time = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void TriggerInteraction() {
		StartCoroutine ("playCardNarration");
	}

	IEnumerator playCardNarration() {
		this.gameObject.GetComponentInParent<InteractionManager> ().narration_triggered = true;
		yield return new WaitForSeconds (0.5f);
		MusicManager musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
		musicManager.SendMessage("playNarrationOfTrigger", ObjectTriggerType.FATHERS_DAY,SendMessageOptions.DontRequireReceiver);
		this.gameObject.GetComponentInParent<InteractionManager> ().narration_triggered = false;

	}
}
