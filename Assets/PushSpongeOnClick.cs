using UnityEngine;
using System.Collections;

public class PushSpongeOnClick : MonoBehaviour {

	public GameObject player;
	private float moveStrengthFactor = 800.0f;
	public GameObject target;

	private bool isTriggered = false;

	// Use this for initialization
	void Start () {
		if(player == null) player = GameObject.Find ("Player");
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Interact") && LanternManager.ammunition > 0) {
			float distToPlayer = Vector3.Distance (player.transform.position, this.transform.position);
			if (distToPlayer < LanternManager.lanternRange * 3.0f) { // Increased because big object

				//float moveStrength = (LanternManager.lanternRange - distToPlayer) / LanternManager.lanternRange; // Between 0 and 1 depending on proximity
				Vector3 moveDirection = target.transform.position - this.transform.position;
				moveDirection.y = 0.0f;
				moveDirection.Normalize ();
				moveDirection.y = 0.3f; // Nawwww
				this.GetComponent<Rigidbody> ().AddForce (moveDirection * moveStrengthFactor);
			}
		}
	}
}
