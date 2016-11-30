using UnityEngine;
using System.Collections;

public class InteractionManager : MonoBehaviour {

	public GameObject player;

	bool interaction_triggered;
	public float range_factor = 1.0f;
	public bool is_retriggerable;
	public float trigger_cooldown_time = 2.0f;	//This should be 2.0f for only those objects that are not retriggerable.
    public Material []indicator_materials; //0 is the dim one, 1 is the bright one
	public bool narration_triggered;
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
		narration_triggered = false;
        /*halo = (Behaviour)interaction_indicator.GetComponent("Halo");
		halo.enabled = false;*/
        interaction_indicator.GetComponent<Renderer>().material = indicator_materials[0];
	}


	// Update is called once per frame
	void Update () {
		
	}

	//This function is called if player clicks near an object. See GameManager
	void InteractWithObject(float distance_to_player) {
		if (!interaction_triggered && !narration_triggered) {
			if (interaction_indicator.activeSelf == false)
				interaction_indicator.SetActive (true);
			distance_to_player = Vector3.Distance (player.transform.position, interactive_object.transform.position);
			if (distance_to_player < LanternManager.lanternRange * range_factor) { //Accurate distance from player to object 
					interaction_triggered = true;
					StartCoroutine ("triggerCountdown");
                    //halo.enabled = true;
					interaction_indicator.GetComponent<Animator> ().SetTrigger ("IdleRotate");
                    interaction_indicator.GetComponent<Renderer>().material = indicator_materials[1];
                    interactive_object.SendMessage ("TriggerInteraction", SendMessageOptions.DontRequireReceiver);

			}
		}
	}

	IEnumerator triggerCountdown() {
		yield return new WaitForSeconds (trigger_cooldown_time);
		if (is_retriggerable) {
			interaction_indicator.GetComponent<Animator> ().SetTrigger ("RotateIdle");
			interaction_indicator.GetComponent<Renderer> ().material = indicator_materials [0];
			interaction_triggered = false;
		} 
		else {
			interaction_indicator.SetActive (false);
		
		}
	}

}
