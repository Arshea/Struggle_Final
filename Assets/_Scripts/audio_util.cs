using UnityEngine;
using System.Collections;

public class audio_util : MonoBehaviour {

	public static IEnumerator play_sound_fade_in ( AudioSource audioSource, float fadeTime, bool playFromPause = false )
	{
		float t = 0.0f;

		float sourceTargetVolume = audioSource.volume;
		audioSource.volume = 0.0f;

		if ( playFromPause == true )
		{
			audioSource.UnPause ();
		}
		else
		{
			audioSource.Play ();
		}
		
		while ( t < fadeTime ) 
		{
			audioSource.volume = Mathf.Lerp( 0.0f, sourceTargetVolume, t/fadeTime );
			t += Time.deltaTime;
			yield return null;
		}
	}
}

