using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	static int numMiniPickUps_total;
	static int percentComplete;

	// Use this for initialization
	void Start () {
		GameObject[] miniPickups = GameObject.FindGameObjectsWithTag ("PickUpMini");
		numMiniPickUps_total = miniPickups.Length;
	}
	
	// Update is called once per frame
	void Update () {
	
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
