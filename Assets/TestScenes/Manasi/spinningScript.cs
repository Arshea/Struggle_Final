using UnityEngine;
using System.Collections;

public class spinningScript : MonoBehaviour {

	public GameObject player;

	//Audio
	public AudioSource spinningTopSfxLoop;
	public AudioSource spinningTopSfxHit;

    public float yRotSpeed = 100;
    public bool isSpinning = true;

    private Vector3 origin;
 	//private float zRotationMin = -35;
	//private float zRotationMax = 0;
	//private float t = 0f;

	// Use this for initialization
	void Start () {
		if(player == null) player = GameObject.Find ("Player");

        origin = this.gameObject.GetComponent<Transform>().position;
		this.gameObject.GetComponentInParent<InteractionManager>().trigger_cooldown_time = 2.0f;
        //t = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isSpinning)
            Spinning();
	}
	void TriggerInteraction() {
		if(isSpinning) StopSpinning ();
		Vector3 forceDir = transform.position - player.transform.position;
		forceDir.y = 0.0f;
		forceDir.Normalize ();
		GetComponent<Rigidbody> ().AddForce (forceDir * 1000.0f);
		spinningTopSfxHit.Play ();
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
		spinningTopSfxLoop.Stop ();
    }
   
}
