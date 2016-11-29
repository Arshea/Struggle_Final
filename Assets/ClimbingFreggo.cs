using UnityEngine;
using System.Collections;

public class ClimbingFreggo : MonoBehaviour {

	public GameObject player;

	bool interaction_triggered = false;
	public float range_factor = 1.0f;
	public bool is_retriggerable;
	public float trigger_cooldown_time = 2.0f;	//This should be 2.0f for only those objects that are not retriggerable.
	public Material []indicator_materials; //0 is the dim one, 1 is the bright one
	//public bool narration_triggered;
	//CHILDREN
	//Object
	public GameObject[] freggos;
	public GameObject[] freggoFinalPositions;
	//Glowy interaction indicator
	private Behaviour halo;
	public GameObject interaction_indicator;
	// Use this for initialization


	void Start () {
		if(player == null) 
			player = GameObject.Find ("Player");
		interaction_triggered = false;
		/*halo = (Behaviour)interaction_indicator.GetComponent("Halo");
		halo.enabled = false;*/
		interaction_indicator.GetComponent<Renderer>().material = indicator_materials[0];
		interaction_indicator.SetActive (false);
	}


	// Update is called once per frame
	void Update () {
		if (!interaction_triggered && (freggos [0].activeSelf && freggos [1].activeSelf && freggos [2].activeSelf))
			interaction_indicator.SetActive (true);
	}

	//This function is called if player clicks near an object. See GameManager
	void InteractWithObject(float distance_to_player) {
		//Debug.Log ("ClimbingFreggo:: Entered InteractWithObject fn");
		if (interaction_indicator.activeSelf) {
			//Debug.Log ("ClimbingFreggo:: Passed first if");
			if (!interaction_triggered) {
				//Debug.Log ("ClimbingFreggo:: Passed second if");
				distance_to_player = Vector3.Distance (player.transform.position, interaction_indicator.transform.position);
				//Debug.Log ("ClimbingFreggo:: Distance to player is " + distance_to_player);
				if (distance_to_player < LanternManager.lanternRange * range_factor) { //Accurate distance from player to object 
					//Debug.Log ("ClimbingFreggo:: Passed third if");
					interaction_triggered = true;
					StartCoroutine ("triggerCountdown");
					//halo.enabled = true;
					interaction_indicator.GetComponent<Renderer> ().material = indicator_materials [1];


					StartCoroutine("MoveTheFreggos");

				}
			}
		}
	}

	IEnumerator triggerCountdown() {
		yield return new WaitForSeconds (trigger_cooldown_time);

		interaction_indicator.SetActive (false);
		Debug.Log ("triggerCountdown in ClimbingFreggo!");

	}

	IEnumerator MoveTheFreggos() {
		float animationTime = 2.5f;
		float complete = 0.0f;
		float startTime = Time.time;
		Vector3[] freggoStartPos = {
			freggos [0].transform.position,
			freggos [1].transform.position,
			freggos [2].transform.position
		};
		Quaternion[] freggoStartRot = {
			freggos [0].transform.rotation,
			freggos [1].transform.rotation,
			freggos [2].transform.rotation
		};

		while (Time.time - startTime < animationTime) {
			for (int i = 0; i < 3; i++) {
				complete = (Time.time - startTime) / animationTime;
				freggos [i].transform.position = Vector3.Lerp (freggoStartPos [i], freggoFinalPositions [i].transform.position, complete);
				freggos [i].transform.rotation = Quaternion.Lerp (freggoStartRot [i], freggoFinalPositions [i].transform.rotation, complete);
			
			}

			yield return null;
		}

		yield return new WaitForSeconds (0.5f);

		for (int i = 0; i < 3; i++) {
			freggos [i].GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		}

		yield return null;
	}

}
