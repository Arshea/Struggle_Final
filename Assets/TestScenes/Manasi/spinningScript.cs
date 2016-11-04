using UnityEngine;
using System.Collections;

public class spinningScript : MonoBehaviour {

    public float yRotSpeed = 100;
    public bool isSpinning = true;

    private Vector3 origin;
    private float zRotationMin = -35;
    private float zRotationMax = 0;
    private float t = 0f;

	// Use this for initialization
	void Start () {
        origin = this.gameObject.GetComponent<Transform>().position;
        t = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isSpinning)
            Spinning();
	}

    void Spinning()
    {
        this.gameObject.GetComponent<Transform>().Rotate(0, yRotSpeed * Time.deltaTime, 0, Space.World);
        //locked the position
        this.gameObject.GetComponent<Transform>().position = origin;
    }

    void StopSpinning()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        isSpinning = false;
    }
   
}
