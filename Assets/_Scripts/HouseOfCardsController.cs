using UnityEngine;
using System.Collections;

public class HouseOfCardsController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: other: " + other);
        if(other.tag == "Player")
        {
            Destroy(this.GetComponent<Rigidbody>());
            foreach(Transform triangle in this.GetComponent<Transform>())
            {
                foreach(Transform card in triangle)
                {
                    if(card.tag == "Card")
                    {
                        Rigidbody rb = card.gameObject.AddComponent<Rigidbody>();
                    }
                }
            }
            Destroy(this.GetComponent<BoxCollider>());
        }
    }
}
