using UnityEngine;
using System.Collections;

public class WaterAbsorb : MonoBehaviour {

	// Use this for initialization
	private bool shrink = false;
	public float targetScale = 0.0f;
	public float shrinkSpeed = 2.0f;
	public GameObject sponge;
	public Collider waterCollider;

	void Start () {
		Physics.IgnoreCollision(sponge.GetComponent<Collider>(), waterCollider);
	}
	
	// Update is called once per frame
	void Update () {

		if(shrink)
		{
			this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(targetScale, targetScale, targetScale), Time.deltaTime*shrinkSpeed);
		}
	
	}

	void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Sponge"))
        {
        	shrink = true;
        	waterCollider.enabled = false;
        	Debug.Log("Sponge entered water");
        }
    }

	void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Sponge"))
        {
        	shrink = false;
        	waterCollider.enabled = false;
        	Debug.Log("Sponge exited water");
        }

    }

}
