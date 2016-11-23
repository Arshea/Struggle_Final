using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public GameObject pausePanel;
	public bool isPaused;
	public GameObject first_person_camera;
    public GameObject player;

    private Vector3 prevLoc;

	// Use this for initialization
	void Start () {
		isPaused = false;
        prevLoc = player.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 vel = (player.transform.position - prevLoc);
        if (isPaused) {
			PauseGame (true);
		} else {
			PauseGame (false);
		}
		if(Input.GetButtonDown ("Pause") && vel.y == 0) {
			TogglePause ();
		}
        prevLoc = player.transform.position;
    }

	void PauseGame (bool state) {
		if (state) {
			Cursor.visible = true;
			Time.timeScale = 0.0f;
		} 
		else {
			Cursor.visible = false;
			Time.timeScale = 1.0f;
		}
		
		pausePanel.SetActive (state);
	}

	public void TogglePause () {
		isPaused = !isPaused;
	}

	public void QuitGame () {
		Application.LoadLevel("Main_Menu");
	}
}
