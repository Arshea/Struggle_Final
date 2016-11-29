using UnityEngine;
using System.Collections;

public class DominoFall : MonoBehaviour {

	public GameObject player;

	public GameObject v_spotlight;
	public GameObject spotlight;
	public GameObject v_spotAnimPosition; // position of vlight after the animation
	public GameObject spotAnimPosition; // position of spot after the animation


	// Use this for initialization
	void Start () {
		//if(player == null) player = GameObject.Find ("Player");
		this.gameObject.GetComponentInParent<InteractionManager>().trigger_cooldown_time = 12.458f;
		if (player == null)
			player = GameObject.Find ("Player");

	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetButtonDown ("Interact") && LanternManager.ammunition > 0) {
			float distToPlayer = Vector3.Distance (player.transform.position, dominoTrigger.transform.position);
			if (distToPlayer < LanternManager.lanternRange) {
				FallDown ();
			}
		}*/
	}

	void TriggerInteraction() {
		// Move spotlights
		v_spotlight.transform.position = v_spotAnimPosition.transform.position;
		v_spotlight.transform.rotation = v_spotAnimPosition.transform.rotation;
		spotlight.transform.position = spotAnimPosition.transform.position;
		spotlight.transform.rotation = spotAnimPosition.transform.rotation;

		//Debug.Log ("Hit a domino!");
		this.gameObject.GetComponentInParent<Animator> ().SetTrigger ("FallTrigger");
		StartCoroutine ("playDominoNarration");

	}
	IEnumerator playDominoNarration() {
		yield return new WaitForSeconds (1.0f);
		this.gameObject.GetComponentInParent<InteractionManager> ().narration_triggered = true;
		MusicManager musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
		musicManager.SendMessage("playNarrationOfTrigger", ObjectTriggerType.AFTER_DOMINO,SendMessageOptions.DontRequireReceiver);
	}
}
