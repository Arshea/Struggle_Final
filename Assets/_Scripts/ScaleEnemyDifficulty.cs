using UnityEngine;
using System.Collections;

public class ScaleEnemyDifficulty : MonoBehaviour {

	public GameObject []freggo_types;

	// Use this for initialization
	void Start () {
		freggo_types [0].SetActive (true);
		freggo_types [1].SetActive (false);
		freggo_types [2].SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}

	void scaleDifficultyByOne(int progressState) {
		if ((progressState > 1)&&(progressState<4)) {
			for (int i = 0; i < 3; i++) {
				if (i == (progressState - 1))
					freggo_types [i].SetActive (true);
				else
					freggo_types [i].SetActive (false);
				
			}
		}
	
	}
}
