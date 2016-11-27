using UnityEngine;
using System.Collections;

public class HouseOfCards : MonoBehaviour {

	//public GameObject player;
	public AudioSource houseOfCardsSfx;

	// Use this for initialization
	void Start () {
		//if(player == null) player = GameObject.Find ("Player");
		this.gameObject.GetComponentInParent<InteractionManager>().trigger_cooldown_time = 4.0f;
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetButtonDown ("Interact") && LanternManager.ammunition > 0) {
			float distToPlayer = Vector3.Distance (player.transform.position, this.transform.position);
			if (distToPlayer < LanternManager.lanternRange) {
				FallDown ();
			}
		}*/
	}
	void TriggerInteraction()
    {
        foreach (Transform child in transform)
        {
			if (child.gameObject.CompareTag ("PlayingCard")) {
				child.GetComponent<Rigidbody> ().isKinematic = false;
			} 
        }
		houseOfCardsSfx.Play ();

		StartCoroutine ("playHouseOfCardsNarration");
    }

	IEnumerator playHouseOfCardsNarration() {
		this.gameObject.GetComponentInParent<InteractionManager> ().narration_triggered = true;
		yield return new WaitForSeconds (1.0f);
		MusicManager musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
		musicManager.SendMessage("playNarrationOfTrigger", ObjectTriggerType.CARDS,SendMessageOptions.DontRequireReceiver);
	}


}
