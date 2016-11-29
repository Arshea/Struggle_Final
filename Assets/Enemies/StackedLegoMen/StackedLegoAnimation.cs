using UnityEngine;
using System.Collections;

public class StackedLegoAnimation : MonoBehaviour {

	public GameObject player;
	public GameObject enemyRoot;
	public float running_speed = 8.0f;
	// Use this for initialization
	private float closest_distance_to_player;
	private int current_state;
	public float territory_radius = 14.0f;
	private Vector3 initial_position;
	private Vector3 current_position;
	public int enemy_health;
	//private float run_speed;

	// Children
	public GameObject animatedFreggo;
	public GameObject deadFreggo;

	//Audio
	public AudioSource audio_source;
	public AudioClip[] stunned_sound;

	/***Triggers for narration. Do not remove***/
	public static bool narration_trigger = false;
	//private bool has_already_triggered = false;
	private bool has_already_triggered_stun_2 = false;
	private bool has_already_triggered_stun_1 = false;
	private bool has_already_triggered_stun_0 = false;
	public static Vector3 current_enemy_position;
	//public static string enemy_name;
	/*******************************************/

	private enum states :int
	{
		BEFOREEMERGE = -1,
		IDLE_3,
		RUN_3,
		STUN_2,
		RETURN_3,
		EMERGE_3, // To run back to initial position
		RUN_2,
		RETURN_2,
		IDLE_2,
		STUN_1,
		RUN_1,
		IDLE_1,
		RETURN_1,
		STUN_0

	}

	private enum transitions : int
	{
		IDLE_3_RUN_3 = states.STUN_0+1,
		RUN_3_STUN_2,
		RETURN_3_IDLE_3,
		RETURN_3_RUN_3,
		RUN_3_RETURN_3,
		RUN_3_IDLE_3,
		//RUNSTAGGER,
		//STAGGERRUN,
		//STAGGERSTUN,
		EMERGE_3_RUN_3,
		IDLE_3_STUN_2,
		RETURN_3_STUN_2,


		STUN_2_RUN_2,
		IDLE_2_RUN_2,
		RUN_2_STUN_1,
		RETURN_2_IDLE_2,
		RETURN_2_RUN_2,
		RUN_2_RETURN_2,
		IDLE_2_STUN_1,
		RETURN_2_STUN_1,
		STUN_2_STUN_1,
		//RUN_2_IDLE_2,

		STUN_1_RUN_1,
		IDLE_1_RUN_1,
		RUN_1_STUN_0,
		RETURN_1_IDLE_1,
		RETURN_1_RUN_1,
		RUN_1_RETURN_1,
		//RUN_1_IDLE_1,
		IDLE_1_STUN_0,
		RETURN_1_STUN_0,
		STUN_1_STUN_0

	}
	void Start () {
		if(player == null) player = GameObject.Find ("Player");


		current_state = (int)states.BEFOREEMERGE;
		initial_position = transform.position;
		current_position = initial_position;
		//enemy_health = 2;
		//run_speed = 2;
		closest_distance_to_player = 5;

		deadFreggo.SetActive (false);
	}

	//Returns true if the player is in territory
	bool playerInTerritory() {

		if (Vector3.Distance (player.transform.position, initial_position) < territory_radius) {

			return true;
		}

		return false;
	}

	bool atBoundary(Vector3 current_position) {
		if (Vector3.Distance (initial_position, current_position) < territory_radius)
			return false;
		return true;
	}


	// Update is called once per frame
	void Update () {
		switch (current_state) {

		case (int)states.BEFOREEMERGE: 
			playerCollider.hit_by_enemy = false;
			if (playerInTerritory ()) {
				//GameManager.encounteredEnemy ();
				animatedFreggo.GetComponent<Animator> ().SetTrigger ("EmergeTrigger");
				current_state = (int)states.EMERGE_3;
			}
			//Debug.Log ("BEFOREEMERGE");
			break;
		case (int)states.EMERGE_3: 
			playerCollider.hit_by_enemy = false;
			current_state = (int)transitions.EMERGE_3_RUN_3;
			//Debug.Log ("EMERGE");
			break;
		case (int)transitions.EMERGE_3_RUN_3:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Emerge_3_Run_3");
			current_state = (int)states.RUN_3;
			break;

		case (int)states.IDLE_3: 
			playerCollider.hit_by_enemy = false;
			if (playerInTerritory ()) {

				transform.LookAt (new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z));
				current_state = (int)transitions.IDLE_3_RUN_3;
			}
			if (Input.GetButtonDown ("Interact")) {
				//Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					current_position = transform.position;
					if (Vector3.Distance (player.transform.position, current_position) < LanternManager.lanternRange) {
						if (enemy_health == 0)
							current_state = (int)transitions.IDLE_3_STUN_2;
						//else
						//current_state = (int)transitions.RUNSTAGGER;

					}
				}
			}
			//Debug.Log ("IDLE");
			break;

		case (int)transitions.IDLE_3_RUN_3:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Idle_3_Run_3");
			current_state = (int)states.RUN_2;
			//Debug.Log ("IDLE_3_RUN_3");
			break;
		case (int)transitions.IDLE_3_STUN_2:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Idle_3_Stun_2");
			current_state = (int)states.STUN_2;
			//Debug.Log ("IDLE_3_STUN_2");
			break;

		case (int)states.RUN_3:
			//float z_value = 3 * Time.deltaTime;
			current_position = transform.position;
			if (!atBoundary (current_position)) {
				transform.LookAt (new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z));
				if (closest_distance_to_player >= Vector3.Distance (player.transform.position, this.transform.position)) {
					current_enemy_position = transform.position;
					player.SendMessage ("enemyHit", current_enemy_position,SendMessageOptions.DontRequireReceiver);
					//Debug.Log ("Hit by enemy" + playerCollider.hit_by_enemy);
				}

				transform.Translate (new Vector3 (0, 0, running_speed * Time.deltaTime));

			} else
				current_state = (int)transitions.RUN_3_RETURN_3;

			if (Input.GetButtonDown ("Interact")) {
				//Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					if (Vector3.Distance (player.transform.position, current_position) < LanternManager.lanternRange) {
						if (enemy_health == 0)
							current_state = (int)transitions.RUN_3_STUN_2;
						//else
						//current_state = (int)transitions.RUNSTAGGER;

					}
				}
			}
			//Debug.Log ("RUN");
			break;
		case (int)transitions.RUN_3_STUN_2:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Run_3_Stun_2");
			current_state = (int)states.STUN_2;
			//Debug.Log ("RUN_3_STUN_2");
			break;
		case (int)transitions.RUN_3_RETURN_3:
			playerCollider.hit_by_enemy = false;
			current_state = (int)states.RETURN_3;
			//Debug.Log ("RUN_3_RETURN_3");
			break;

		case (int)states.RETURN_3:
			playerCollider.hit_by_enemy = false;
			if (playerInTerritory()) {
				//transform.LookAt (new Vector3 (player.transform.position.x, 0, player.transform.position.z));
				current_state = (int)transitions.RETURN_3_RUN_3;
			}

			else if (Vector3.SqrMagnitude (initial_position - transform.position) > 0.1f) {
				transform.LookAt (new Vector3 (initial_position.x, transform.position.y, initial_position.z));
				transform.Translate (new Vector3 (0, 0, running_speed * Time.deltaTime));
			} 

			else {
				//initial_position = transform.position;
				current_state = (int)transitions.RETURN_3_IDLE_3;
			}
			if (Input.GetButtonDown ("Interact")) {
				Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					if (Vector3.Distance (player.transform.position, current_position) < LanternManager.lanternRange) {
						if (enemy_health == 0)
							current_state = (int)transitions.RETURN_3_STUN_2;
						//else
						//current_state = (int)transitions.RUNSTAGGER;

					}
				}
			}
			//			Debug.Log ("RETURN");
			break;
		case (int)transitions.RETURN_3_IDLE_3:
			playerCollider.hit_by_enemy = false;
			//GetComponent<Animator> ().ResetTrigger ("IdleRun");
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Run_3_Idle_3");
			current_state = (int)states.IDLE_3;
			//Debug.Log ("RETURN_3_IDLE_3");
			break;
		case (int)transitions.RETURN_3_RUN_3:
			playerCollider.hit_by_enemy = false;
			current_state = (int)states.RUN_3;
			//Debug.Log ("RETURN_3_RUN_3");
			break;
		case (int)transitions.RETURN_3_STUN_2:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Run_3_Stun_2");
			current_state = (int)states.STUN_2;
			//Debug.Log ("RETURN_3_STUN_2");
			break;
		case (int)states.STUN_2:
			playerCollider.hit_by_enemy = false;
			/*For narration, do not remove*/
			if (!has_already_triggered_stun_2) {
				has_already_triggered_stun_2 = true;
				//StartCoroutine ("deathTheFreggo");
				current_state = (int)transitions.STUN_2_RUN_2;
				audio_source.clip = stunned_sound [Random.Range (0, (stunned_sound.Length - 1))];
				audio_source.Play();
			} else {

			}
			/*if (Input.GetButtonDown ("Interact")) {
				//Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					if (Vector3.Distance (player.transform.position, current_position) < LanternManager.lanternRange) {
						if (enemy_health == 0)
							current_state = (int)transitions.STUN_2_STUN_1;
						//else
						//current_state = (int)transitions.RUNSTAGGER;

					}
				}
			}*/
			/*****************************/
			// Debug.Log ("STUN");
			break;
		case (int)transitions.STUN_2_RUN_2:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Stun_2_Run_2");
			current_state = (int)states.RUN_2;
			break;
		/*case (int)transitions.STUN_2_STUN_1:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Stun_2_Stun_1");
			current_state = (int)states.STUN_1;
			//Debug.Log ("RETURN_3_STUN_2");
			break;*/
		case (int)states.RUN_2:
			current_position = transform.position;
			if (!atBoundary (current_position)) {
				transform.LookAt (new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z));
				if (closest_distance_to_player >= Vector3.Distance (player.transform.position, this.transform.position)) {
					current_enemy_position = transform.position;
					player.SendMessage ("enemyHit", current_enemy_position,SendMessageOptions.DontRequireReceiver);
					//Debug.Log ("Hit by enemy" + playerCollider.hit_by_enemy);
				}

				transform.Translate (new Vector3 (0, 0, running_speed * Time.deltaTime));

			} else
				current_state = (int)transitions.RUN_2_RETURN_2;

			if (Input.GetButtonDown ("Interact")) {
				//Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					if (Vector3.Distance (player.transform.position, current_position) < LanternManager.lanternRange) {
						if (enemy_health == 0)
							current_state = (int)transitions.RUN_2_STUN_1;
						//else
						//current_state = (int)transitions.RUNSTAGGER;

					}
				}
			}
			//Debug.Log ("RUN");
			break;

		case (int)transitions.RUN_2_RETURN_2:
			playerCollider.hit_by_enemy = false;
			current_state = (int)states.RETURN_2;
			//Debug.Log ("RUN_3_RETURN_3");
			break;
		case (int)transitions.RUN_2_STUN_1:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Run_2_Stun_1");
			current_state = (int)states.STUN_1;
			//Debug.Log ("RUN_3_STUN_2");
			break;
		case (int)states.RETURN_2:
			playerCollider.hit_by_enemy = false;
			if (playerInTerritory ()) {
				//transform.LookAt (new Vector3 (player.transform.position.x, 0, player.transform.position.z));
				current_state = (int)transitions.RETURN_2_RUN_2;
			} else if (Vector3.SqrMagnitude (initial_position - transform.position) > 0.1f) {
				transform.LookAt (new Vector3 (initial_position.x, transform.position.y, initial_position.z));
				transform.Translate (new Vector3 (0, 0, running_speed * Time.deltaTime));
			} else {
				//initial_position = transform.position;
				current_state = (int)transitions.RETURN_2_IDLE_2;
			}
			if (Input.GetButtonDown ("Interact")) {
				Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					if (Vector3.Distance (player.transform.position, current_position) < LanternManager.lanternRange) {
						if (enemy_health == 0)
							current_state = (int)transitions.RETURN_2_STUN_1;
						//else
						//current_state = (int)transitions.RUNSTAGGER;

					}
				}
			}
			break;
		case (int)transitions.RETURN_2_IDLE_2:
			playerCollider.hit_by_enemy = false;
			//GetComponent<Animator> ().ResetTrigger ("IdleRun");
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Run_2_Idle_2");
			current_state = (int)states.IDLE_2;
			//Debug.Log ("RETURN_3_IDLE_3");
			break;
		case (int)transitions.RETURN_2_RUN_2:
			playerCollider.hit_by_enemy = false;
			current_state = (int)states.RUN_2;
			//Debug.Log ("RETURN_3_RUN_3");
			break;
		case (int)transitions.RETURN_2_STUN_1:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Run_2_Stun_1");
			current_state = (int)states.STUN_2;
			//Debug.Log ("RETURN_3_STUN_2");
			break;
		case (int)states.IDLE_2: 
			playerCollider.hit_by_enemy = false;
			if (playerInTerritory ()) {

				transform.LookAt (new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z));
				current_state = (int)transitions.IDLE_2_RUN_2;
			}
			if (Input.GetButtonDown ("Interact")) {
				//Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					current_position = transform.position;
					if (Vector3.Distance (player.transform.position, current_position) < LanternManager.lanternRange) {
						if (enemy_health == 0)
							current_state = (int)transitions.IDLE_2_STUN_1;
						//else
						//current_state = (int)transitions.RUNSTAGGER;

					}
				}
			}
			//Debug.Log ("IDLE");
			break;
		case (int)transitions.IDLE_2_STUN_1:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Idle_2_Stun_1");
			current_state = (int)states.STUN_1;
			//Debug.Log ("IDLE_3_STUN_2");
			break;
		case (int)transitions.IDLE_2_RUN_2:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Idle_2_Run_2");
			current_state = (int)states.RUN_2;
			//Debug.Log ("IDLE_3_RUN_3");
			break;

		case (int)states.STUN_1:
			playerCollider.hit_by_enemy = false;
			/*For narration, do not remove*/
			if (!has_already_triggered_stun_1) {
				has_already_triggered_stun_1 = true;
				//StartCoroutine ("deathTheFreggo");
				current_state = (int)transitions.STUN_1_RUN_1;
				audio_source.clip = stunned_sound [Random.Range (0, (stunned_sound.Length - 1))];
				audio_source.Play();
			} else {

			}
			/*if (Input.GetButtonDown ("Interact")) {
				//Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					if (Vector3.Distance (player.transform.position, current_position) < LanternManager.lanternRange) {
						if (enemy_health == 0)
							current_state = (int)transitions.STUN_1_STUN_0;
						//else
						//current_state = (int)transitions.RUNSTAGGER;

					}
				}
			}*/
			/*****************************/
			// Debug.Log ("STUN");
			break;
		case (int)transitions.STUN_1_RUN_1:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Stun_1_Run_1");
			current_state = (int)states.RUN_1;
			break;
		/*case (int)transitions.STUN_1_STUN_0:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Stun_1_Stun_0");
			current_state = (int)states.STUN_0;
			//Debug.Log ("RETURN_2_STUN_1");
			break;*/
		case (int)states.RUN_1:
			current_position = transform.position;
			if (!atBoundary (current_position)) {
				transform.LookAt (new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z));
				if (closest_distance_to_player >= Vector3.Distance (player.transform.position, this.transform.position)) {
					current_enemy_position = transform.position;
					player.SendMessage ("enemyHit", current_enemy_position,SendMessageOptions.DontRequireReceiver);
					//Debug.Log ("Hit by enemy" + playerCollider.hit_by_enemy);
				}

				transform.Translate (new Vector3 (0, 0, running_speed * Time.deltaTime));

			} else
				current_state = (int)transitions.RUN_1_RETURN_1;

			if (Input.GetButtonDown ("Interact")) {
				//Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					if (Vector3.Distance (player.transform.position, current_position) < LanternManager.lanternRange) {
						if (enemy_health == 0)
							current_state = (int)transitions.RUN_1_STUN_0;
						//else
						//current_state = (int)transitions.RUNSTAGGER;

					}
				}
			}
			//Debug.Log ("RUN");
			break;

		case (int)transitions.RUN_1_RETURN_1:
			playerCollider.hit_by_enemy = false;
			current_state = (int)states.RETURN_1;
			//Debug.Log ("RUN_2_RETURN_2");
			break;
		case (int)transitions.RUN_1_STUN_0:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Run_1_Stun_0");
			current_state = (int)states.STUN_0;
			//Debug.Log ("RUN_2_STUN_1");
			break;
		case (int)states.RETURN_1:
			playerCollider.hit_by_enemy = false;
			if (playerInTerritory ()) {
				//transform.LookAt (new Vector3 (player.transform.position.x, 0, player.transform.position.z));
				current_state = (int)transitions.RETURN_1_RUN_1;
			} else if (Vector3.SqrMagnitude (initial_position - transform.position) > 0.1f) {
				transform.LookAt (new Vector3 (initial_position.x, transform.position.y, initial_position.z));
				transform.Translate (new Vector3 (0, 0, running_speed * Time.deltaTime));
			} else {
				//initial_position = transform.position;
				current_state = (int)transitions.RETURN_1_IDLE_1;
			}
			if (Input.GetButtonDown ("Interact")) {
				Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					if (Vector3.Distance (player.transform.position, current_position) < LanternManager.lanternRange) {
						if (enemy_health == 0)
							current_state = (int)transitions.RETURN_1_STUN_0;
						//else
						//current_state = (int)transitions.RUNSTAGGER;

					}
				}
			}
			break;
		case (int)transitions.RETURN_1_IDLE_1:
			playerCollider.hit_by_enemy = false;
			//GetComponent<Animator> ().ResetTrigger ("IdleRun");
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Run_1_Idle_1");
			current_state = (int)states.IDLE_1;
			//Debug.Log ("RETURN_2_IDLE_2");
			break;
		case (int)transitions.RETURN_1_RUN_1:
			playerCollider.hit_by_enemy = false;
			current_state = (int)states.RUN_1;
			//Debug.Log ("RETURN_2_RUN_2");
			break;
		case (int)transitions.RETURN_1_STUN_0:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Run_1_Stun_0");
			current_state = (int)states.STUN_0;
			//Debug.Log ("RETURN_2_STUN_1");
			break;
		case (int)states.IDLE_1: 
			playerCollider.hit_by_enemy = false;
			if (playerInTerritory ()) {

				transform.LookAt (new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z));
				current_state = (int)transitions.IDLE_1_RUN_1;
			}
			if (Input.GetButtonDown ("Interact")) {
				//Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					current_position = transform.position;
					if (Vector3.Distance (player.transform.position, current_position) < LanternManager.lanternRange) {
						if (enemy_health == 0)
							current_state = (int)transitions.IDLE_1_STUN_0;
						//else
						//current_state = (int)transitions.RUNSTAGGER;

					}
				}
			}
			//Debug.Log ("IDLE");
			break;
		case (int)transitions.IDLE_1_STUN_0:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Idle_1_Stun_0");
			current_state = (int)states.STUN_0;
			//Debug.Log ("IDLE_2_STUN_1");
			break;
		case (int)transitions.IDLE_1_RUN_1:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("Idle_1_Run_1");
			current_state = (int)states.RUN_1;
			//Debug.Log ("IDLE_2_RUN_2");
			break;

		case (int)states.STUN_0:
			playerCollider.hit_by_enemy = false;
			/*For narration, do not remove*/
			if (!has_already_triggered_stun_0) {
				has_already_triggered_stun_0 = true;
				StartCoroutine ("deathTheFreggo");
				audio_source.clip = stunned_sound [Random.Range (0, (stunned_sound.Length - 1))];
				audio_source.Play();
			} else {

			}
			/*****************************/
			// Debug.Log ("STUN");
			break;
		default:
			//Debug.Log ("ERROR");
			break;

		}


		/*if (Input.GetButtonDown ("Interact")) {
			Debug.Log ("Ammunition: " + LanternManager.ammunition);
			if (LanternManager.ammunition > 0) {
				if (Vector3.Distance (player.transform.position, current_position) < 12) {
					if(current_state == (int)states.STUN) {
						// Kill animated freggo
						Vector3 pos = enemyRoot.transform.position;
						Quaternion rot = enemyRoot.transform.rotation;
						animatedFreggo.SetActive (false);
						// Birth static freggo in its place
						deadFreggo.SetActive (true);
						deadFreggo.transform.position = pos;
						deadFreggo.transform.rotation = rot;

					//else
					//current_state = (int)transitions.RUNSTAGGER;
					}
				}
			}
		}*/
	}

	IEnumerator deathTheFreggo() {
		yield return new WaitForSeconds (1.25f);
		// Kill animated freggo
		Vector3 pos = new Vector3(0.0f,0.0f,0.0f);
		float rotY = enemyRoot.transform.eulerAngles.y;
		Vector3 rot = new Vector3 (0.0f, rotY, 0.0f);
		Vector3 sca = enemyRoot.transform.localScale;
		animatedFreggo.SetActive (false);
		// Birth static freggo in its place
		deadFreggo.SetActive (true);
		//sca *= 100.0f; // Scales are 100 factor different, probably metre-cm conversion
		//pos.y -= 1.45f;	// Move to floor
		//pos.x = 5.0f;
		deadFreggo.transform.localPosition = pos;
		deadFreggo.transform.eulerAngles = rot;
		deadFreggo.transform.localScale = sca;


		Debug.Log ("killed a freggo child");
		yield return null;
	}


}
