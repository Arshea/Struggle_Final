using UnityEngine;
using System.Collections;

public class AnimateEnemy: MonoBehaviour {

	public GameObject player;
	public GameObject enemyRoot;
	public float running_speed = 8.0f;
	// Use this for initialization
	private float closest_distance_to_player;
	private int current_state;
	public float territory_radius = 12.0f;
	private Vector3 initial_position;
	private Vector3 current_position;
	public int enemy_health;
	private float run_speed;

	// Children
	public GameObject animatedFreggo;
	public GameObject deadFreggo;

	/***Triggers for narration. Do not remove***/
	public static bool narration_trigger = false;
	private bool has_already_triggered = false;
	private bool has_already_triggered_stun = false;
	public static Vector3 current_enemy_position;
	//public static string enemy_name;
	/*******************************************/

	private enum states :int
	{
		BEFOREEMERGE = -1,
		IDLE,
		RUN,
		STUN,
		RETURN,
		STAGGER,
		EMERGE// To run back to initial position
	}

	private enum transitions : int
	{
		IDLERUN = 6,
		RUNSTUN,
		STUNIDLE,
		RETURNIDLE,
		RETURNRUN,
		RUNRETURN,
		RUNSTAGGER,
		STAGGERRUN,
		STAGGERSTUN,
		EMERGERUN,
		IDLESTUN,
		RETURNSTUN
	}
	void Start () {
		current_state = (int)states.BEFOREEMERGE;
		initial_position = transform.position;
		current_position = initial_position;
		//enemy_health = 2;
		run_speed = 2;
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

	/*void resetAllAnimations() {
		GetComponent<Animator> ().ResetTrigger ("IdleRun");
		GetComponent<Animator> ().ResetTrigger ("RunStun");
		GetComponent<Animator> ().ResetTrigger ("RunIdle");
	}*/
	// Update is called once per frame
	void Update () {
		//resetAllAnimations ();
		switch (current_state) {
		case (int)states.BEFOREEMERGE: 
			playerCollider.hit_by_enemy = false;
			if (playerInTerritory ()) {
				animatedFreggo.GetComponent<Animator> ().SetTrigger ("EmergeTrigger");
				current_state = (int)states.EMERGE;
			}
						//Debug.Log ("BEFOREEMERGE");
			break;
		case (int)states.EMERGE: 
			playerCollider.hit_by_enemy = false;
			current_state = (int)transitions.EMERGERUN;
						//Debug.Log ("EMERGE");
			break;
		case (int)transitions.EMERGERUN:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("EmergeRun");
			current_state = (int)states.RUN;
			break;
		case (int)states.IDLE: 
			playerCollider.hit_by_enemy = false;
			if (playerInTerritory ()) {
				/*Triggers narration when in enemy territory*/
				if (!has_already_triggered) {
					narration_trigger = true;
					has_already_triggered = true;
					MusicManager.enemy_name = transform.name;
					//Debug.Log (MusicManager.enemy_name);
				}
				/*******************************************/
				transform.LookAt (new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z));
				current_state = (int)transitions.IDLERUN;
			}
			if (Input.GetButtonDown ("Interact")) {
				//Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					current_position = transform.position;
					if (Vector3.Distance (player.transform.position, current_position) < LanternManager.lanternRange) {
						if (enemy_health == 0)
							current_state = (int)transitions.IDLESTUN;
						//else
							//current_state = (int)transitions.RUNSTAGGER;
					
					}
				}
			}
			//Debug.Log ("IDLE");
			break;

		case (int)transitions.IDLERUN:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("IdleRun");
			current_state = (int)states.RUN;
		//Debug.Log ("IDLERUN");
			break;
		case (int)transitions.IDLESTUN:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("IdleStun");
			current_state = (int)states.STUN;
			//Debug.Log ("IDLESTUN");
			break;

		case (int)states.RUN:
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
				current_state = (int)transitions.RUNRETURN;

			if (Input.GetButtonDown ("Interact")) {
				//Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					if (Vector3.Distance (player.transform.position, current_position) < LanternManager.lanternRange) {
						if (enemy_health == 0)
							current_state = (int)transitions.RUNSTUN;
						else
							current_state = (int)transitions.RUNSTAGGER;
					
					}
				}
			}
			//Debug.Log ("RUN");
			break;
		case (int)transitions.RUNSTUN:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("RunStun");
			current_state = (int)states.STUN;
			//Debug.Log ("RUNSTUN");
			break;
		case (int)transitions.RUNSTAGGER:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("RunStagger");
			current_state = (int)states.STAGGER;
			// Debug.Log ("RUNSTAGGER");
			break;
		case (int)transitions.RUNRETURN:
			playerCollider.hit_by_enemy = false;
			current_state = (int)states.RETURN;
			//Debug.Log ("RUNRETURN");
			break;
		case (int)states.STUN:
			playerCollider.hit_by_enemy = false;
			/*For narration, do not remove*/
			if (!has_already_triggered_stun) {
				has_already_triggered_stun = true;
				if (MusicManager.enemy_name != null)
					MusicManager.enemy_name += "Stun";
				StartCoroutine ("deathTheFreggo");
			} else {

			}
			/*****************************/
			// Debug.Log ("STUN");
			break;
		case (int)states.STAGGER:
			//Debug.Log ("STAGGER");
			playerCollider.hit_by_enemy = false;
			enemy_health -= 1;
			if (Input.GetButtonDown("Interact") && LanternManager.ammunition > 0)
			{
				if (Vector3.Distance(player.transform.position, transform.position) < LanternManager.lanternRange)
                    {
                        if (enemy_health == 0)
                            current_state = (int)transitions.STAGGERSTUN;
                        else
                        {
                            current_state = (int)transitions.STAGGERRUN;
							animatedFreggo.GetComponent<Animator>().SetFloat("RunSpeed", (run_speed * 0.3f));
                        }
                    }
                }
			break;
		case (int)transitions.STAGGERSTUN:
				playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator>().SetTrigger("StaggerStun");
                current_state = (int)states.STUN;
                break;
		case (int)transitions.STAGGERRUN:
			playerCollider.hit_by_enemy = false;
//			//Debug.Log ("STAGGERRUN");
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("StaggerRun");
			current_state = (int)states.RUN;
			break;
		case (int)states.RETURN:
			playerCollider.hit_by_enemy = false;
			if (playerInTerritory()) {
				//transform.LookAt (new Vector3 (player.transform.position.x, 0, player.transform.position.z));
				current_state = (int)transitions.RETURNRUN;
			}

			else if (Vector3.SqrMagnitude (initial_position - transform.position) > 0.1f) {
				transform.LookAt (new Vector3 (initial_position.x, transform.position.y, initial_position.z));
				transform.Translate (new Vector3 (0, 0, running_speed * Time.deltaTime));
			} 

			else {
				//initial_position = transform.position;
				current_state = (int)transitions.RETURNIDLE;
			}
			if (Input.GetButtonDown ("Interact")) {
				Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					if (Vector3.Distance (player.transform.position, current_position) < LanternManager.lanternRange) {
						if (enemy_health == 0)
							current_state = (int)transitions.RETURNSTUN;
						//else
							//current_state = (int)transitions.RUNSTAGGER;
					
					}
				}
			}
//			Debug.Log ("RETURN");
			break;
		case (int)transitions.RETURNIDLE:
			playerCollider.hit_by_enemy = false;
			//GetComponent<Animator> ().ResetTrigger ("IdleRun");
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("RunIdle");
			current_state = (int)states.IDLE;
			//Debug.Log ("RETURNIDLE");
			break;
		case (int)transitions.RETURNRUN:
			playerCollider.hit_by_enemy = false;
			current_state = (int)states.RUN;
			//Debug.Log ("RETURNRUN");
			break;
		case (int)transitions.RETURNSTUN:
			playerCollider.hit_by_enemy = false;
			animatedFreggo.GetComponent<Animator> ().SetTrigger ("RunStun");
			current_state = (int)states.STUN;
			//Debug.Log ("RETURNSTUN");
			break;
		default:
			//Debug.Log ("ERROR");
			break;
			
		}

		/*
		if (Input.GetButtonDown ("Interact")) {
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
		Vector3 pos = enemyRoot.transform.position;
		float rotY = enemyRoot.transform.eulerAngles.y;
		Vector3 rot = new Vector3 (0.0f, rotY, 0.0f);
		Vector3 sca = enemyRoot.transform.localScale;
		animatedFreggo.SetActive (false);
		// Birth static freggo in its place
		deadFreggo.SetActive (true);
		sca *= 100.0f; // Scales are 100 factor different, probably metre-cm conversion
		pos.y -= 1.45f;	// Move to floor
		//pos.x = 5.0f;
		deadFreggo.transform.position = pos;
		deadFreggo.transform.eulerAngles = rot;
		deadFreggo.transform.localScale = sca;


		// Debug.Log ("killed a freggo child");
		yield return null;
	}


}