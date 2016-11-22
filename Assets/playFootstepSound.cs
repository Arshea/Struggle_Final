using UnityEngine;
using System.Collections;

public class playFootstepSound : MonoBehaviour {

	public AudioClip[] footsteps;

	void FootstepSound(){
		var audio_source = GetComponentInParent<AudioSource>();
		audio_source.clip = footsteps[Random.Range(0, (footsteps.Length-1))];
		audio_source.Play ();
		}
}