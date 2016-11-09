using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ReportCollision : MonoBehaviour {

public CapsuleCollider myCollider;

#if UNITY_EDITOR
	void Update () 
	{
		// Debug.Log("Report Collision Update");

		if(UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) 
		{
			this.enabled = false;
		} 
		else 
		{
			// editor code here!
			myCollider = this.gameObject.GetComponent<CapsuleCollider>();

			Vector3 start = new Vector3(transform.position.x, myCollider.radius + 1, transform.position.z);
			Vector3 end   = new Vector3(transform.position.x, myCollider.height - (myCollider.radius * 2), transform.position.z);

			Collider[] overlaps = Physics.OverlapCapsule(start, end, myCollider.radius * 3);


			for(int i = 0; i < overlaps.Length; ++i)
			{
				if( !(overlaps[i].gameObject.CompareTag("Plane")) && !(overlaps[i].gameObject.CompareTag("CarpetHair")))
				{
					Debug.Log("Disabling Carpet Hair  ");
					// Debug.Log(overlaps[i].gameObject.tag);
					this.gameObject.SetActive(false);
        			// DestroyImmediate(this.gameObject);
				}
			}
		
		}
	}

	void OnTriggerEnter(Collider other) {

		Debug.Log("report collision on trigger enter");

		if(UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) 
		{
			this.enabled = false;
		} 
		else 
		{
			// editor code here!

			
		}
    }

    void OnTriggerStay(Collider other) {
    	Debug.Log("report collision on trigger stay");

    }

	
#endif

}
