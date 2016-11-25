using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Manager controls
	private LanternManager lanternManager;
	private MusicManager musicManager;

	// Mini pickups
	static int numMiniPickUps_total;
	static int percentComplete;

	// Large pickups
	public static int progressState = 0; // Number of lights picked up

	//Enemy containers
	private GameObject[] enemies;
	private int num_of_enemies;

	// Use this for initialization
	void Start () {
		lanternManager = (LanternManager)GameObject.Find ("Lantern_Manager").GetComponent(typeof(LanternManager));
		musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager)); // UNCOMMENT WHEN YOU WANT SOUNDS !!!!


		GameObject[] miniPickups = GameObject.FindGameObjectsWithTag ("PickUpMini");
		numMiniPickUps_total = miniPickups.Length;

		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		num_of_enemies = enemies.Length;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void pickedUpLightMusic() {
		if(musicManager != null) musicManager.playLightPickupMusic ();

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
}
