using UnityEngine;
using System.Collections;

public class playerCollider : MonoBehaviour
{
	public GameObject player;
    public GameObject lantern;
    public GameObject victoryText;
    public float pushPower = 2.0F; // Push force of player
    public GameObject[] carpetHair;
    public float distTriggerCarpetHair = 2.0f; // Distance to start carpet hair animation
    public float maxAngle = 80; // Max angle of carpet hair tilt
	//public AudioClip pickupClip;
	public float animationDelay = 3.3f; // Secs delay before lerping light towards player on pickup
	public float speed = 2.5f; // Speed at which pickup moves to player
	public float distWindEffect = 5f; // Distance for pickup wind effect
	public float blowOutTime = 0.5f; // Time for pickup wind effect
	public float blowBackTime = 5.0f; // Time for pickup wind effect cooldown
	public GameObject ambient;
	public static bool pickedUp = false;	//Is true if a light is picked up, false after pick up animation	
    private Light lanternLight = null;
	private bool finished = false;
	public AudioClip smallPickupAudio;
	public static bool animateLightPickupWind = false;
	public GameObject lanternCentre;


    // Use this for initialization
    void Start()
    {
        lanternLight = lantern.GetComponent<Light>();
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
		if (animateLightPickupWind) {
			animateLightPickupWind = false;
			StartCoroutine (lightPickupWindEffect ());
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
            hit.transform.SendMessage("StopSpinning", SendMessageOptions.DontRequireReceiver);
        }
        if (hit.gameObject.CompareTag("HouseOfCards"))
        {
            hit.gameObject.GetComponent<BoxCollider>().enabled = false;
            hit.transform.SendMessage("FallDown", SendMessageOptions.DontRequireReceiver);
        }

        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
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
        }

		if (other.gameObject.CompareTag("PickUp"))
		{
			//Debug.Log ("Sending pick up sigal");
			other.gameObject.GetComponent<SphereCollider>().enabled = false;
			other.transform.SendMessage("pickUp", SendMessageOptions.DontRequireReceiver);
		}
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

			// If game over

			if (finished) {
				victoryText.SetActive (true);
				//Light ambientLight = ambient.GetComponent<Light>();

				//ambientLight.intensity += 0.35f;
				finished = !finished;
			}
			pickup.SetActive (false);
		}
		pickedUp = false;
	}


	// Wind effect on surrounding carpet hairs
	IEnumerator lightPickupWindEffect() {
		yield return new WaitForSeconds (3.2f);

		// Using distTriggerCarpetHair (from player movement pushing carpet hairs aside) to produce wind
		float start = distTriggerCarpetHair;
		float max = distWindEffect;
		float startTime = Time.time;


		while (Time.time - startTime < blowOutTime + blowBackTime) {
			if (Time.time - startTime < blowOutTime) {
				distTriggerCarpetHair = Mathf.Lerp (start, max, (Time.time - startTime));
			} else {
				distTriggerCarpetHair = max;
				distTriggerCarpetHair = Mathf.Lerp (max, start, (Time.time - startTime - blowOutTime));
			}
			//Debug.Log ("New step effect range: " + distTriggerCarpetHair);
			yield return null;
		}

	}

}
