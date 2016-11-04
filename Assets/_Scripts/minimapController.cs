using UnityEngine;
using System.Collections;

public class minimapController : MonoBehaviour {
	
	public GameObject player;
	// private Vector3 offset;
	public Light lantern_light;
	// Use this for initialization
	private Vector3 cam_pos;

	void Start () {
		// offset = transform.position - player.transform.position;
		cam_pos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
	}

	// Update is called once per frame
	void Update () {
		cam_pos.x = player.transform.position.x;
		cam_pos.y = transform.position.y;
		cam_pos.z = player.transform.position.z;
		// transform.position = player.transform.position + offset;
		// transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
		transform.position =  cam_pos;
	}

    void OnPreCull () {
        if (lantern_light != null)
            lantern_light.enabled = false;
    }
     
    void OnPreRender() {
        if (lantern_light != null)
            lantern_light.enabled = false;
    }

    void OnPostRender() {
        if (lantern_light != null)
            lantern_light.enabled = true;
    }
}
