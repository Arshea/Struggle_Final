using UnityEngine;
using System.Collections;

public class Chessboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponentInParent<InteractionManager>().range_factor = 3.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void TriggerInteraction(){
		GetComponent<Animator> ().SetTrigger ("IdleMove");
		StartCoroutine ("playChessboardNarration");
	}

	IEnumerator playChessboardNarration(){
		yield return new WaitForSeconds (5.958f);
		MusicManager musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
		musicManager.SendMessage ("playNarrationOfTrigger", ObjectTriggerType.CHESSBOARD,SendMessageOptions.DontRequireReceiver);
	}
}

