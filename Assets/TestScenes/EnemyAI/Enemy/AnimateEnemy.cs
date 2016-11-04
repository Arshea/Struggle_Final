using UnityEngine;
using System.Collections;

public class AnimateEnemy: MonoBehaviour {

	public GameObject player;
	public GameObject enemyRoot;
	// Use this for initialization
	private float disBtwn;
	private int current_state;
	private float territory_radius;
	private Vector3 initial_position;
	public int enemy_health;
	private float run_speed;

	/***Triggers for narration. Do not remove***/
	public static bool narration_trigger = false;
	private bool has_already_triggered = false;
	private bool has_already_triggered_stun = false;
	//public static string enemy_name;
	/*******************************************/

	private enum states :int
	{
		IDLE = 0,
		RUN,
		STUN,
		RETURN,
		STAGGER// To run back to initial position
	}

	private enum transitions : int
	{
		IDLERUN = 5,
		RUNSTUN,
		STUNIDLE,
		RETURNIDLE,
		RETURNRUN,
		RUNRETURN,
		RUNSTAGGER,
		STAGGERRUN,
		STAGGERSTUN
	}
	void Start () {
		current_state = (int)states.IDLE;
		initial_position = transform.position;
		territory_radius = 20;
		//enemy_health = 2;
		run_speed = 2;
		disBtwn = 2;
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
		case (int)states.IDLE: 
			
			if (playerInTerritory ()) {
				/*Triggers narration when in enemy territory*/
				if (!has_already_triggered) {
					narration_trigger = true;
					has_already_triggered = true;
					MusicManager.enemy_name = transform.name;
					Debug.Log (MusicManager.enemy_name);
				}
				/*******************************************/
				transform.LookAt (new Vector3 (player.transform.position.x, 0, player.transform.position.z));
				current_state = (int)transitions.IDLERUN;
			}
//			Debug.Log ("IDLE");
			break;

		case (int)transitions.IDLERUN:
			GetComponent<Animator> ().SetTrigger ("IdleRun");
			current_state = (int)states.RUN;
//			Debug.Log ("IDLERUN");
			break;

		case (int)states.RUN:
			//float z_value = 3 * Time.deltaTime;
			Vector3 current_position = transform.position;
			if (!atBoundary (current_position)) {
				transform.LookAt (new Vector3 (player.transform.position.x, 0, player.transform.position.z));
				if(disBtwn < Vector3.Distance(player.transform.position, this.transform.position))
					transform.Translate (new Vector3 (0, 0, 3 * Time.deltaTime));
			} else
				current_state = (int)transitions.RUNRETURN;

			if (Input.GetButtonDown ("Interact")) {
				Debug.Log ("Ammunition: " + LanternManager.ammunition);
				if (LanternManager.ammunition > 0) {
					if (Vector3.Distance (player.transform.position, current_position) < 12) {
						if (enemy_health == 0)
							current_state = (int)transitions.RUNSTUN;
						else
							current_state = (int)transitions.RUNSTAGGER;
					
					}
				}
			}
//			Debug.Log ("RUN");
			break;
		case (int)transitions.RUNSTUN:
			GetComponent<Animator> ().SetTrigger ("RunStun");
			current_state = (int)states.STUN;
//			Debug.Log ("RUNSTUN");
			break;
		case (int)transitions.RUNSTAGGER:
			GetComponent<Animator> ().SetTrigger ("RunStagger");
			current_state = (int)states.STAGGER;
//			Debug.Log ("RUNSTUN");
			break;
		case (int)transitions.RUNRETURN:
			current_state = (int)states.RETURN;
//			Debug.Log ("RUNRETURN");
			break;
		case (int)states.STUN:
			/*For narration, do not remove*/
			if (!has_already_triggered_stun) {
				has_already_triggered_stun = true;
				if(MusicManager.enemy_name!=null)
					MusicManager.enemy_name += "Stun";
			}
			/*****************************/
//			Debug.Log ("STUN");
			break;
		case (int)states.STAGGER:
//			Debug.Log ("STAGGER");
			enemy_health -= 1;
			if (Input.GetButtonDown("Interact") && LanternManager.ammunition > 0)
			{
                    if (Vector3.Distance(player.transform.position, transform.position) < 12)
                    {
                        if (enemy_health == 0)
                            current_state = (int)transitions.STAGGERSTUN;
                        else
                        {
                            current_state = (int)transitions.STAGGERRUN;
                            GetComponent<Animator>().SetFloat("RunSpeed", (run_speed * 0.3f));
                        }
                    }
                }
			break;
		case (int)transitions.STAGGERSTUN:
                GetComponent<Animator>().SetTrigger("StaggerStun");
                current_state = (int)states.STUN;
                break;
		case (int)transitions.STAGGERRUN:
//			Debug.Log ("STAGGERRUN");
			GetComponent<Animator> ().SetTrigger ("StaggerRun");
			current_state = (int)states.RUN;
			break;
		case (int)states.RETURN:
			if (Vector3.SqrMagnitude (initial_position - transform.position) > 0.1f) {
				transform.LookAt (new Vector3 (initial_position.x, 0, initial_position.z));
				transform.Translate (new Vector3 (0, 0, 3 * Time.deltaTime));
			} 
			if (playerInTerritory()) {
				//transform.LookAt (new Vector3 (player.transform.position.x, 0, player.transform.position.z));
				current_state = (int)transitions.RETURNRUN;
			}
			else {
				initial_position = transform.position;
				current_state = (int)transitions.RETURNIDLE;
			}
//			Debug.Log ("RETURN");
			break;
		case (int)transitions.RETURNIDLE:
			//GetComponent<Animator> ().ResetTrigger ("IdleRun");
			GetComponent<Animator> ().SetTrigger ("RunIdle");
			current_state = (int)states.IDLE;
//			Debug.Log ("RETURNIDLE");
			break;
		case (int)transitions.RETURNRUN:
			current_state = (int)states.RUN;
//			Debug.Log ("RETURNRUN");
			break;
		default:
//			Debug.Log ("ERROR");
			break;
			
		}

	}


}
