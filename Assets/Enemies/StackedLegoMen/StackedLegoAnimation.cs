﻿using UnityEngine;
using System.Collections;

public class StackedLegoAnimation : MonoBehaviour {

	public GameObject player;
	private Vector3 initial_position;

	// Use this for initialization
	void Start () {
		initial_position = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Interact")) {
			Debug.Log ("Ammunition: " + LanternManager.ammunition);
			if (LanternManager.ammunition > 0) {
				if (Vector3.Distance (player.transform.position, initial_position) < 12) {
					Debug.Log ("Hit Stacked Lego");
					GetComponent<Animator> ().SetTrigger ("IdleStun");

				}
			}
		}
	}
}
