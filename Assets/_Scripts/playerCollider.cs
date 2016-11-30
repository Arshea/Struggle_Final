using UnityEngine;
using System.Collections;

public class playerCollider : MonoBehaviour
{

	public GameObject player;

    public float pushPower = 2.0F; // Push force of player
    public GameObject[] carpetHair;
    public float distTriggerCarpetHair = 2.0f; // Distance to start carpet hair animation
    public float maxAngle = 80; // Max angle of carpet hair tilt
	//public AudioClip pickupClip;
	public float speed = 2.5f; // Speed at which pickup moves to player
	public static bool pickedUp = false;	//Is true if a light is picked up, false after pick up animation	
	public AudioClip smallPickupAudio;
	public static bool hit_by_enemy = false;
	// Oh hit! overlay
	private bool isOverlay = false;
	private float overlayStartTime;
	public float overlayCooldownTime = 500.0f;
	private float max_health_overlay_intensity = 3.0f;
	public float health_overlay_increment = 0.5f;
	private bool water_narration_trigger;
	private bool edge_narration_trigger;
	// Enemy push back params
	private float pushSpeed = 0.7f, pushTime = 0.5f;

    // Use this for initialization
    void Start()
    {
		if(player == null) player = GameObject.Find ("Player");
		water_narration_trigger = false;
		edge_narration_trigger = false;
    }

    // Update is called once per frame
    void Update()
    {
		Collider[] activeCarpetHairs = Physics.OverlapSphere (player.transform.position, distTriggerCarpetHair);
		for (int i = 0; i < activeCarpetHairs.Length; i++)
        {
			GameObject hair = activeCarpetHairs [i].gameObject;
			if(hair != null && hair.transform.CompareTag("CarpetHair")) {
				//Debug.Log ("Found");
				Vector3 distance = transform.position - activeCarpetHairs[i].transform.position;
	            distance.y = 0; // Only look on x,z
	            float distanceMag = distance.magnitude;
	            if (distanceMag < distTriggerCarpetHair)
	            {
					adjustCarpetHair(hair, distanceMag);
	            }
				else activeCarpetHairs[i].transform.eulerAngles = new Vector3(0, 0, 0);
			}
        }

    }

    // Animate carpet hairs
    void adjustCarpetHair(GameObject carpetHair, float distance)
    {
        //carpetHair.SetActive(false); // Testing
        float magnitude = ((distTriggerCarpetHair - distance) / distTriggerCarpetHair);
        // Non-linear
        magnitude *= magnitude;


        float xRatio, zRatio;
        xRatio = (carpetHair.transform.position.x - transform.position.x) / distance * maxAngle;
        zRatio = (carpetHair.transform.position.z - transform.position.z) / distance * maxAngle;

        //Debug.Log("Distance: " + distance + "Magnitude: " + magnitude + " Ratios " + xRatio + " " + zRatio);
        carpetHair/*[i]*/.transform.eulerAngles = new Vector3(zRatio * magnitude, 0, -xRatio * magnitude);// carpetHairAngles[i];

    }

	// Physics for collision (testing) - may be needed later but currently unused

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Top"))
        {
			hit.gameObject.GetComponentInParent<InteractionManager>().SendMessage("InteractWithObject",0.0f, SendMessageOptions.DontRequireReceiver);
        }
        if (hit.gameObject.CompareTag("HouseOfCards"))
        {
            hit.gameObject.GetComponent<BoxCollider>().enabled = false;
			hit.gameObject.GetComponentInParent<InteractionManager>().SendMessage("InteractWithObject",0.0f, SendMessageOptions.DontRequireReceiver);
        }
		if (hit.gameObject.CompareTag ("DominoTrigger")) {
			hit.gameObject.GetComponentInParent<InteractionManager>().SendMessage("InteractWithObject",0.0f, SendMessageOptions.DontRequireReceiver);
			//hit.transform.parent.SendMessage("FallDown", SendMessageOptions.DontRequireReceiver);


		}
		if (hit.gameObject.CompareTag ("FreggoCollider")/* || hit.gameObject.CompareTag ("FreggoCollider2") || hit.gameObject.CompareTag ("FreggoCollider3")*/) {
			if(hit.transform.parent.parent.gameObject.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName("Run"))
				enemyHit (hit.transform.position);
		}
		if (hit.gameObject.CompareTag ("JengaNarrationTrigger")) {
			MusicManager musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
			musicManager.SendMessage ("playNarrationOfTrigger", ObjectTriggerType.CLIMBING_END,SendMessageOptions.DontRequireReceiver);
		}
		if (hit.gameObject.CompareTag ("ClimbingNarrationTrigger")) {
			hit.collider.enabled = false;
			MusicManager musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
			musicManager.SendMessage ("playNarrationOfTrigger", ObjectTriggerType.CLIMBING_START,SendMessageOptions.DontRequireReceiver);
		}
		if (hit.gameObject.CompareTag ("DominoNarrationTrigger")) {
			hit.collider.enabled = false;
			MusicManager musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
			musicManager.SendMessage ("playNarrationOfTrigger", ObjectTriggerType.DOMINO,SendMessageOptions.DontRequireReceiver);
		}


		if (hit.gameObject.CompareTag ("WaterNarrationTrigger")) {
			if(!water_narration_trigger) { 
				MusicManager musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
				musicManager.SendMessage ("playNarrationOfTrigger", ObjectTriggerType.WATER,SendMessageOptions.DontRequireReceiver);
				water_narration_trigger = true;
			}
		}

		if (hit.gameObject.CompareTag ("EdgeOfTheWorld")) {
			if(!edge_narration_trigger) { 
				MusicManager musicManager = (MusicManager)GameObject.Find ("Music_Manager").GetComponent(typeof(MusicManager));
				musicManager.SendMessage ("playNarrationOfTrigger", ObjectTriggerType.CARPET_EDGE,SendMessageOptions.DontRequireReceiver);
				edge_narration_trigger = true;
			}
		}
		if (hit.gameObject.name == "BookFallTrigger")
		{
			Debug.Log ("Player collided with book fall trigger");
			hit.gameObject.GetComponent<SphereCollider>().enabled = false;
			hit.gameObject.GetComponentInParent<BookFallScript>().SendMessage("TriggerBookFall", SendMessageOptions.DontRequireReceiver);
		}
		if (hit.gameObject.name == "GoodbyeTrigger")
		{
			Debug.Log ("Player collided with goodbye trigger");
			hit.gameObject.GetComponent<BoxCollider>().enabled = false;
			GameObject startEndEvents = GameObject.Find("Start_And_End_Events");
			startEndEvents.GetComponentInParent<StartAndEndEvts>().SendMessage("Goodbye", SendMessageOptions.DontRequireReceiver);
		}


        
		// Push ability

		/*
		Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;*/
    }


    // Collect light source. Should probably not be here.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUpMini"))
        {
            GameObject otherObject = other.transform.gameObject;
			other.gameObject.transform.parent.gameObject.GetComponent<AudioSource> ().Play ();
			other.isTrigger = false;
            other.enabled = false;
            //other.gameObject.SetActive(false);

            playPickupAnim(otherObject.transform.parent.gameObject);

			float percent = GameManager.getGameCompletion ();

        }

		if (other.gameObject.CompareTag("PickUp"))
		{
			//Debug.Log ("Sending pick up sigal");
			other.gameObject.GetComponent<SphereCollider>().enabled = false;
			other.transform.SendMessage("pickUp", SendMessageOptions.DontRequireReceiver);
		}
    }

	// Got hit by enemy -- bounce backwards
	void enemyHit(Vector3 enemyPos) {
		Vector3 knockBackDirection = player.transform.position - enemyPos;
		knockBackDirection.y = 0.0f; // Don't jump up
		knockBackDirection.Normalize ();
		knockBackDirection.y = 0.35f; // Ok jump up a bit
		StartCoroutine (hitKnockBack (knockBackDirection));

		if (UnityStandardAssets.ImageEffects.ScreenOverlay.intensity < max_health_overlay_intensity) {
			UnityStandardAssets.ImageEffects.ScreenOverlay.intensity += health_overlay_increment;
		}
		overlayStartTime = Time.time;
		if (!isOverlay) {
			isOverlay = true;
			StartCoroutine (screenOverlayCooldown());
		}
		//player.GetComponent<CharacterController> ().attachedRigidbody.AddForce (knockBackDirection * 10.0f);
		//player.GetComponent<CharacterController>().Move(knockBackDirection * 10.0f);

		Debug.Log ("Enemy hit");
	}

	IEnumerator hitKnockBack(Vector3 direction) {
		float startTime = Time.time;
		float endTime = startTime + pushTime;

		while (Time.time < endTime) {
			float complete = (Time.time - startTime) / (endTime - startTime); // 0 when coroutine starts; 1 at end (proportion of completion)
			player.GetComponent<CharacterController>().Move(direction * pushSpeed * (1 - complete));
			yield return null;
		}
	}

	IEnumerator screenOverlayCooldown() {

		while (UnityStandardAssets.ImageEffects.ScreenOverlay.intensity > 0) {
			float complete = (Time.time - overlayStartTime) / (overlayCooldownTime); // 0 at start of cooldown; 1 at end (proportion of completion)
			UnityStandardAssets.ImageEffects.ScreenOverlay.intensity = Mathf.Lerp(UnityStandardAssets.ImageEffects.ScreenOverlay.intensity, 0.0f, complete);
			yield return null;
		}
		UnityStandardAssets.ImageEffects.ScreenOverlay.intensity = 0;
		isOverlay = false;

	} 

	void playPickupAnim(GameObject pickup) {
//		pickup.GetComponent<Animation>().Play ();
		//currentLight = pickup;
		StartCoroutine(moveLightToPlayer (pickup));
		//StartCoroutine (lightPickupWindEffect ());
	}

	IEnumerator moveLightToPlayer(GameObject pickup) {
		if (pickup != null) {
			//yield return new WaitForSeconds (3.4f);
			float startTime = Time.time;
			Vector3 posA = pickup.transform.position;
			Vector3 playerToCam = new Vector3 (0, 4.0f, 0); // Player =/= camera -> need to adjust
			while (Vector3.Distance(pickup.transform.position,transform.position-playerToCam) > 0.1f) {
				pickup.transform.position = Vector3.Lerp (posA, transform.position-playerToCam, (Time.time - startTime)*speed);
				yield return null;
			}
		
			pickup.SetActive (false);
		}
		pickedUp = false;
	}



}
