using UnityEngine;
using System.Collections;

public class Sailing : MonoBehaviour {

	public GameObject start;
    public GameObject end;
    public float speed = 1.0F;

	public Material []indicator_materials; //0 is the dim one, 1 is the bright one
	public GameObject interaction_indicator;


	private Transform startMarker;
    private Transform endMarker;
    private Transform playerTransform;
    private Vector3 prev_player_pos;
    private Vector3 prev_boat_pos;

    private float startTime;
    private float journeyLength;

	private bool sailing;
	private bool reachedEnd;

	//Audio
	public AudioSource sailingStartStopSfx;
	public AudioSource sailingLoopSfx;
	public AudioClip[] sailingStartStopClip;

	// Use this for initialization
	void Start () {

		startMarker = start.transform;
		endMarker   = end.transform;

		sailing = false;
		reachedEnd = false;

		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

		interaction_indicator.GetComponent<Renderer>().material = indicator_materials[0];

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(sailing)
		{
			float distCovered = (Time.time - startTime) * speed;
       		float fracJourney = distCovered / journeyLength;

			if(!reachedEnd)
			{
	        	transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);

	        	if(transform.position == endMarker.position)
	        	{
	        		sailing = false;
	        		reachedEnd = true;
					sailingLoopSfx.Stop ();
					sailingStartStopSfx.clip = sailingStartStopClip [0];
					sailingStartStopSfx.Play ();

					interaction_indicator.GetComponent<Animator> ().SetTrigger ("RotateIdle");
					interaction_indicator.GetComponent<Renderer> ().material = indicator_materials [0];
	        	}
			}
			else
			{
	        	transform.position = Vector3.Lerp(endMarker.position, startMarker.position, fracJourney);

	        	if(transform.position == startMarker.position)
	        	{
	        		sailing = false;
	        		reachedEnd = false;
					sailingLoopSfx.Stop ();
					sailingStartStopSfx.clip = sailingStartStopClip [0];
					sailingStartStopSfx.Play ();

					interaction_indicator.GetComponent<Animator> ().SetTrigger ("RotateIdle");
					interaction_indicator.GetComponent<Renderer> ().material = indicator_materials [0];
	        	}
			}

        	playerTransform.position = prev_player_pos + (transform.position - prev_boat_pos);

        	prev_boat_pos = transform.position;
        	prev_player_pos = playerTransform.position;

		}
	}

	void OnTriggerStay(Collider other)
	{
		if (!sailing)
		{
			if (Input.GetButtonDown("Interact") && LanternManager.ammunition > 0)
			{
				// if(other.gameObject.CompareTag("Player") && !reachedEnd)
				if(other.gameObject.CompareTag("Player") && other.transform.position.y > 2.0f)
				{
					Debug.Log("Start your sailing!");

					interaction_indicator.GetComponent<Animator> ().SetTrigger ("IdleRotate");
					interaction_indicator.GetComponent<Renderer>().material = indicator_materials[1];

					sailing = true;

					sailingStartStopSfx.clip = sailingStartStopClip [1];
					sailingStartStopSfx.Play ();
					sailingLoopSfx.Play ();

					startTime = Time.time;

			        playerTransform = other.gameObject.transform;
			        prev_player_pos = playerTransform.position;

			        prev_boat_pos = transform.position;
				}
			}
		}
	}
}
