using UnityEngine;
using System.Collections;

public class JengaAnim : MonoBehaviour {

	private AudioSource jengaAudioSource;

	// Use this for initialization
	void Start () {
		jengaAudioSource = GetComponentInParent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void TriggerInteraction(){
		GetComponent<Animator> ().SetTrigger ("IdleStairs");
		jengaAudioSource.Play ();
	}
}
