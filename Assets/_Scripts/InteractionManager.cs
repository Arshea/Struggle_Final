﻿using UnityEngine;
using System.Collections;

public class InteractionManager : MonoBehaviour {

	public GameObject player;

	bool interaction_triggered;
	public float range_factor = 1.0f;
	public bool is_retriggerable;
	public float trigger_cooldown_time = 2.0f;	//This should be 2.0f for only those objects that are not retriggerable.

	//CHILDREN
	//Object
	public GameObject interactive_object;
	//Glowy interaction indicator
	private Behaviour halo;
	public GameObject interaction_indicator;
	// Use this for initialization


	void Start () {
		if(player == null) 
			player = GameObject.Find ("Player");
		interaction_triggered = false;
		halo = (Behaviour)interaction_indicator.GetComponent("Halo");
		halo.enabled = false;
	}


	// Update is called once per frame
	void Update () {
		
	}

	//This function is called if player clicks near an object. See GameManager
	void InteractWithObject(float distance_to_player) {
		if (!interaction_triggered) {
			distance_to_player = Vector3.Distance (player.transform.position, interactive_object.transform.position);
			if (distance_to_player < LanternManager.lanternRange * range_factor) { //Accurate distance from player to object 
					interaction_triggered = true;
					StartCoroutine ("triggerCountdown");
					halo.enabled = true;
					interactive_object.SendMessage ("TriggerInteraction", SendMessageOptions.DontRequireReceiver);

			}
		}
	}

	IEnumerator triggerCountdown() {
		yield return new WaitForSeconds (trigger_cooldown_time);
		interaction_triggered = false;
		if (is_retriggerable)
			halo.enabled = false;
		else
			interaction_indicator.SetActive (false);
	}

}
