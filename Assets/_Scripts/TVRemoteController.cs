using UnityEngine;
using System.Collections;

public class TVRemoteController : MonoBehaviour {

    public GameObject TVCanvas;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        Debug.Log("TVRemote: OnTriggerEnter: ");
        if (TVCanvas.activeSelf)
        {
            TVCanvas.SetActive(false);
            Debug.Log("set false");
        }
        else
        {
            TVCanvas.SetActive(true);
            Debug.Log("set true");
        }
    }
}
