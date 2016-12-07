using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class MainMenuUIManager : MonoBehaviour {

	public GameObject mainPanel;

	void Start () {
		Cursor.visible = true;
	}

	// Update is called once per frame
	void Update () {
		
	}
	public void QuitGame () {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif 
	}

	public void startGame () {
		SceneManager.LoadScene("_Scenes/WorldBuilding");
	}

	public void rollCredits() {
		SceneManager.LoadScene ("Credits/Credits_Test");
	}
}
