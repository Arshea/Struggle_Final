using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Manager controls
	private LanternManager lanternManager;
	private MusicManager musicManager;

	//Player position
	private GameObject player;

	// Mini pickups
	static int numMiniPickUps_total;
	static int percentComplete;

	// Large pickups
	public static int progressState = 0; // Number of lights picked up

	//Enemy containers
	private GameObject[] enemies;
	private int num_of_enemies;
	private static int enemy_encounter_count;
	private bool enemy_narration_message_sent;

	//Interactable objects
	private GameObject[] interactive_objects;
	private int num_of_interactive_objects;


	// Use this for initialization
	void Start () {
		lanternManager = (LanternManager)GameObject.Find ("Lantern_Manager").GetComponent(typeof(LanternManager));
		musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));

		player = GameObject.FindGameObjectWithTag ("Player");

		GameObject[] miniPickups = GameObject.FindGameObjectsWithTag ("PickUpMini");
		numMiniPickUps_total = miniPickups.Length;

		enemy_encounter_count = 0;
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		num_of_enemies = enemies.Length;
		enemy_narration_message_sent = false;

		interactive_objects = GameObject.FindGameObjectsWithTag ("InteractionManager");
		num_of_interactive_objects = interactive_objects.Length;


	}
	
	// Update is called once per frame
	void Update () {
		float distance_to_player;
		if (Input.GetButtonDown ("Interact") && LanternManager.ammunition > 0) {
			for (int i = 0; i < num_of_interactive_objects; i++) {
				distance_to_player = Vector3.Distance (player.transform.position, interactive_objects [i].transform.position);
				//if(distance_to_player < LanternManager.lanternRange) //Approximate distance from interactive object
				interactive_objects [i].SendMessage ("InteractWithObject", distance_to_player, SendMessageOptions.DontRequireReceiver);
			}
		}
		//Move later?
		if (!enemy_narration_message_sent) {
			if (enemy_encounter_count == 1) {
				musicManager.SendMessage ("playNarrationOfTrigger", ObjectTriggerType.ENEMY);
				enemy_narration_message_sent = true;
			}
		}
		
		
	}


	public void pickedUpLightMusic() {
		musicManager.playLightPickupMusic (progressState);

	}

	public void pickedUpLight() {
		Debug.Log ("GameManager: Pick Up Signal");

		lanternManager.addLight ();

		if(musicManager != null) musicManager.playLightPickupNarration (progressState);
		progressState++;


		for (int i = 0; i < num_of_enemies; i++) {
			enemies [i].GetComponent<ScaleEnemyDifficulty> ().SendMessage ("scaleDifficultyByOne", progressState);
		}

	}



	public static float getGameCompletion() {
		if (numMiniPickUps_total <= 0)
			return 0.0f;
		
		GameObject[] miniPickups = GameObject.FindGameObjectsWithTag ("PickUpMini");
		int numTaken = numMiniPickUps_total - miniPickups.Length;
		float percentComplete = ((float)numTaken / (float)numMiniPickUps_total) * 100.0f;

		//Debug.Log ("Game status: " + numMiniPickUps_total + " total; " + numTaken + " taken; " + percentComplete + " percent complete.");


		return percentComplete;

		//return (float)numLeft;
	}

	public static void encounteredEnemy() {
		enemy_encounter_count++;
	}
}
