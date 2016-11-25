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
	public enum Colours {Yellow = 0, Blue, Green, Red};
	public static bool[] lanternContentsUnlocked; // Which lights are currently unlocked?
	public static int progressState = 0; // Number of lights picked up

	//Enemy containers
	private GameObject[] enemies;
	private int num_of_enemies;

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

		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		num_of_enemies = enemies.Length;

		lanternContentsUnlocked = new bool[]{false, false, false, false};

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
	}

	private Colours findLightIndex(string toAdd) {
		if (toAdd == "LightPickup_yellow") {
			//Debug.Log ("adding yellow");
			return GameManager.Colours.Yellow;
		} else if (toAdd == "LightPickup_blue") {
			//Debug.Log ("adding blue");
			return GameManager.Colours.Blue;
		} else if (toAdd == "LightPickup_green") {
			//Debug.Log ("adding green");
			return GameManager.Colours.Green;
		} else if (toAdd == "LightPickup_red") {
			//Debug.Log ("adding red");
			return GameManager.Colours.Red;
		} else {
			Debug.Log ("Error parsing light pickup name");
			return 0;
		}
	}

	public void pickedUpLightMusic() {
		musicManager.playLightPickupMusic ();

	}

	public void pickedUpLight(string lightName) {
		Colours colour = findLightIndex (lightName);

		lanternManager.addLight (colour);

		//musicManager.playLightPickupNarration (progressState);
		lanternContentsUnlocked [(int)colour] = true;
		progressState++;


		for (int i = 0; i < num_of_enemies; i++) {
			enemies [i].GetComponent<ScaleEnemyDifficulty> ().SendMessage ("scaleDifficultyByOne", progressState, SendMessageOptions.DontRequireReceiver);
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
}
