using UnityEngine;
using System.Collections;

public class DeadFreggoMove : MonoBehaviour {

	public GameObject player;
	private float moveStrengthFactor = 1000.0f;

	// Use this for initialization
	void Start () {
		if(player == null) player = GameObject.Find ("Player");

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Interact") && LanternManager.ammunition > 0) {
			float distToPlayer = Vector3.Distance (player.transform.position, this.transform.position);
			if (distToPlayer < LanternManager.lanternRange) {

				float moveStrength = (LanternManager.lanternRange - distToPlayer) / LanternManager.lanternRange; // Between 0 and 1 depending on proximity
				Vector3 moveDirection = this.transform.position - player.transform.position;
				moveDirection.Normalize ();
				moveDirection.y = 0.3f; // Nawwww
				this.GetComponent<Rigidbody> ().AddForce (moveDirection * moveStrength * moveStrengthFactor);
			}
		}
	}
}
