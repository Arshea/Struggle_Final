using UnityEngine;
using System.Collections;

public class spinning : MonoBehaviour {

    public float yRotSpeed = 10;

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
	void Update () {
        Spinning();
	}

    void Spinning()
    {
        this.gameObject.GetComponent<Transform>().Rotate(0, yRotSpeed * Time.deltaTime, 0);
        //locked the position
        this.gameObject.GetComponent<Transform>().position = origin;
    }
}
