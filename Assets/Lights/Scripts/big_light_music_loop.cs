using UnityEngine;
using System.Collections;

public class big_light_music_loop : MonoBehaviour {

	private GameObject player;
	private AudioSource main_theme;
	public AudioSource audio_source;

	//Variables
	float minDistance;
	float maxDistance;
	float distancePlayerFromSource;
	float multiplier = 1;
	float mainThemeStartVol;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		main_theme = GameObject.Find ("main_audio_source").GetComponent<AudioSource>();
		minDistance = audio_source.minDistance;
		maxDistance = audio_source.maxDistance;
		mainThemeStartVol = main_theme.volume;
	}
	
	// Update is called once per frame
	void Update () {
		
		distancePlayerFromSource = (Vector3.Distance (player.transform.position, audio_source.gameObject.transform.position));

		if (distancePlayerFromSource < maxDistance) {
			multiplier = (1-((maxDistance-distancePlayerFromSource)/minDistance));

			main_theme.volume = (mainThemeStartVol * multiplier);
		}
	}

	public void Stop_light_loop () {
		audio_source.Stop ();
		main_theme.volume = mainThemeStartVol;
		Destroy (this);
	}
}
