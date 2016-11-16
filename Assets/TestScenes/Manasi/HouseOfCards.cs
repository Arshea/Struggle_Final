using UnityEngine;
using System.Collections;

public class HouseOfCards : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		if(player == null) player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Interact") && LanternManager.ammunition > 0) {
			float distToPlayer = Vector3.Distance (player.transform.position, this.transform.position);
			if (distToPlayer < LanternManager.lanternRange) {
				FallDown ();
			}
		}
	}
	void FallDown()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("PlayingCard"))
            {
               child.GetComponent<Rigidbody>().isKinematic = false;
        	}
        }
    }



}
