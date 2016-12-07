using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ExitCredits : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Pause"))
			SceneManager.LoadScene ("_Scenes/Main_Menu");
		if (GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Done"))
			StartCoroutine ("ReturnToMainMenu");
	}

	IEnumerator ReturnToMainMenu() {
		yield return new WaitForSeconds (3.0f);
		SceneManager.LoadScene ("_Scenes/Main_Menu");

	}
}

