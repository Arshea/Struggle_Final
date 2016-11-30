using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ReportCollision : MonoBehaviour {

public CapsuleCollider myCollider;

#if UNITY_EDITOR
	void Update () 
	{
		if(UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) 
		{
			//this.enabled = false;
		} 
		else 
		{
			foreach (Transform child in this.transform) {
				if(child.CompareTag("CarpetHair")) {
						
					// editor code here!
					myCollider = child.gameObject.GetComponent<CapsuleCollider> ();

					Vector3 start = new Vector3 (child.transform.position.x, myCollider.radius + 1, child.transform.position.z);
					Vector3 end = new Vector3 (child.transform.position.x, myCollider.height - (myCollider.radius * 2), child.transform.position.z);

					Collider[] overlaps = Physics.OverlapCapsule (start, end, myCollider.radius * 3);


					for (int i = 0; i < overlaps.Length; ++i) {
						if (!(overlaps [i].gameObject.CompareTag ("Plane")) && !(overlaps [i].gameObject.CompareTag ("CarpetHair"))) {
							Debug.Log ("Disabling Carpet Hair  ");
							// Debug.Log(overlaps[i].gameObject.tag);
							//this.gameObject.SetActive(false);
							if(child != null) DestroyImmediate (child.gameObject);
						}
					}
				}
					
			}
		}
	}
	
#endif

}
