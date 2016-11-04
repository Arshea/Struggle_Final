using UnityEngine;
using System.Collections;

public class FOW : MonoBehaviour {

	public float minProx = 6.0F; // Min proximity to centre to activate
	private GameObject player;
	private bool active;

	// Use this for initialization
	void Start () {
		active = true;
		player = GameObject.FindWithTag("Player");
		InvokeRepeating("FOWRun", 0, 0.5F);
	}
	
	// Update is called once per frame
	void FOWRun () {
		if(active)
		{
			Vector3 FOWSection = this.transform.position;
			Vector3 playerPos = player.transform.position;
			float x = FOWSection.x, z = FOWSection.z;

			if ((x - minProx / 2) < playerPos.x && playerPos.x < (x + minProx / 2) &&
				(z - minProx / 2) < playerPos.z && playerPos.z < (z + minProx / 2))
			{
//                Debug.Log("Moving " + this);
				this.transform.Translate(0, 20, 0);
				active = false;
			}
		}

	}
}
