using UnityEngine;
using System.Collections;

// Pushes carpet hairs away from enemies
public class PushCarpetHairs : MonoBehaviour
{

    public GameObject[] carpetHair;
    public float distTriggerCarpetHair = 3.0f; // Distance to start carpet hair animation
    public float maxAngle = 80; // Max angle of carpet hair tilt


    // Use this for initialization
    void Start()
    {
		// Who knows if this will work
    }

    // Update is called once per frame
    void Update()
    {
		Collider[] activeCarpetHairs = Physics.OverlapSphere (this.transform.position, distTriggerCarpetHair);
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

}
