using UnityEngine;
using System.Collections;

public class Sailing : MonoBehaviour {

	public Collider North;
	public Collider South;
	public Collider East;
	public Collider West;

	public GameObject start;
    public GameObject end;
    public float speed = 1.0F;

	private Transform startMarker;
    private Transform endMarker;
    private Transform playerTransform;

    private float startTime;
    private float journeyLength;

	private bool sailing;
	private bool reachedEnd;

	// Use this for initialization
	void Start () {

		North.enabled = false;
		South.enabled = true;
		East.enabled  = true;
		West.enabled  = true;

		startMarker = start.transform;
		endMarker   = end.transform;

		sailing = false;
		reachedEnd = false;

	}
	
	// Update is called once per frame
	void Update () {
	
		if(sailing && !reachedEnd)
		{

			float distCovered = (Time.time - startTime) * speed;
       		float fracJourney = distCovered / journeyLength;
        	transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
        	playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z + 2);

        	// Debug.Log(fracJourney);

        	if(transform.position == endMarker.position)
        	{
        		sailing = false;
        		South.enabled = false;
        		reachedEnd = true;
        		Debug.Log("Destination reached");

        	}
		}

	}

	void OnTriggerEnter(Collider other){

		if(other.gameObject.CompareTag("Player") && !reachedEnd)
		{
			Debug.Log("Start your sailing!");

			South.enabled = true;
			North.enabled = true;
			East.enabled  = true;
			West.enabled  = true;

			sailing = true;

			startTime = Time.time;
	        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

	        playerTransform = other.gameObject.transform;
		}
	}
}
