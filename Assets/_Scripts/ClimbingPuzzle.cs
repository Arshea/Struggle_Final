using UnityEngine;
using System.Collections;

public class ClimbingPuzzle : MonoBehaviour {

	private GameObject player;
	private GameObject jenga;
	private MusicManager musicManager;
	private bool narration_message_sent;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		jenga = GameObject.FindGameObjectWithTag ("Jenga");
		musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
		narration_message_sent = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!narration_message_sent) {
			if (player.transform.position.y > 60) {
				Vector3 direction = jenga.transform.position - player.transform.position;
				direction.Normalize ();
				if ((Vector3.Dot (player.transform.forward, direction) > 0.5) && (Vector3.Distance (jenga.transform.position, player.transform.position) < 55))
					musicManager.SendMessage ("playInstructionNarration", ObjectTriggerType.CLIMBING_START, SendMessageOptions.DontRequireReceiver);
					Debug.Log ("LOOKING AT JENGA WHEEE");
					narration_message_sent = true;
			}

		}
	}
}
