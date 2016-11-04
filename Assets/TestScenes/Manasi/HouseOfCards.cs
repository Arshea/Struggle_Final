using UnityEngine;
using System.Collections;

public class HouseOfCards : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
