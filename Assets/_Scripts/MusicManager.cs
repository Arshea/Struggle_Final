using UnityEngine;
using System.Collections;
/*This file handles narrations and music cues
It also adjusts lantern stun volumes but does not trigger them.*/

//This is in order of priority. 0 high, 3 low
enum NarrationType {
	PROGRESS,
	INSTRUCTION,
	INTERACTIVE,
	IDLE
}

enum ObjectTriggerType {
	STORY_START,
	LIGHT_1,
	LIGHT_2,
	LIGHT_3,
	LIGHT_4,
	STORY_END,					//Not implemented yet /o\

	ENEMY,
	CLIMBING_START,				
	CLIMBING_END,				//Not implemented yet /o\
	DOMINO,
	WATER,

	WAND,
	BOAT,
	CHESSBOARD,
	CLOCK,
	AFTER_DOMINO,
	EARPHONES,
	FATHERS_DAY,
	CARDS,
	CRANE,
	RUBIKS,
	SCRABBLE,
	TOP,
	SPONGE,

	CARPET_EDGE
	/*
	RANDOM*/


}
struct Narration {
	public AudioClip clip;
	public NarrationType type;
	public ObjectTriggerType trigger;
	public float delay;
	public Narration(AudioClip _clip, NarrationType _type, int _trigger, float _delay = 0.0f) {
		clip = _clip;
		type = _type;
		trigger = (ObjectTriggerType)_trigger;
		delay = _delay;
	}
}
public class MusicManager : MonoBehaviour {

	private NarrationType[] types = {
		NarrationType.PROGRESS,
		NarrationType.PROGRESS,
		NarrationType.PROGRESS,
		NarrationType.PROGRESS,
		NarrationType.PROGRESS,
		NarrationType.PROGRESS,						//Not implemented yet /o\

		NarrationType.INSTRUCTION,
		NarrationType.INSTRUCTION,
		NarrationType.INSTRUCTION,
		NarrationType.INSTRUCTION,
		NarrationType.INSTRUCTION,
		NarrationType.INSTRUCTION,

		NarrationType.INTERACTIVE,
		NarrationType.INTERACTIVE,
		NarrationType.INTERACTIVE,
		NarrationType.INTERACTIVE,
		NarrationType.INTERACTIVE,
		NarrationType.INTERACTIVE,
		NarrationType.INTERACTIVE,
		NarrationType.INTERACTIVE,
		NarrationType.INTERACTIVE,
		NarrationType.INTERACTIVE,
		NarrationType.INTERACTIVE,
		NarrationType.INTERACTIVE,
		NarrationType.INTERACTIVE,

		NarrationType.INSTRUCTION
		/*
		NarrationType.IDLE,
		NarrationType.IDLE,
		NarrationType.IDLE*/


	};

	private float[] narration_delays =  {
		0.0f,
		2.0f,
		2.0f,
		2.0f,
		2.0f,
		0.0f,

		0.0f,
		0.0f,
		0.0f,
		0.0f,
		0.0f,
		0.0f,

		0.7f,
		0.0f,
		0.0f,
		0.0f,
		0.0f,
		0.0f,
		0.0f,
		0.0f,
		0.0f,
		0.0f,
		0.0f,
		0.6f,
		0.0f,

		0.0f

	};

	//Audio sources
	private AudioSource main_source;			//Main theme music
	private AudioSource ambient_source;			//Ambient sounds like light pickups
	private AudioSource narration_audio_source;	//Narration
	public AudioSource[] lantern_stun_sources;	//Needs to be public unfortunately. Might find fix for this later

	//Audio clips
	public AudioClip[] main_theme;
	public AudioClip light_variation;

	//Narration essentials
	private Narration[] narrations;
	private int num_of_narrations = 0;	
	public bool []has_played_before;
	public AudioClip[] narration_clips;
	private int current_narration_index;

	private bool main_music_trigger;
	public static string enemy_name;
	//public static string light_name;
	void Start() {

		//Assign all sources
		main_source = GameObject.Find ("main_audio_source").GetComponent<AudioSource>();
		ambient_source = GameObject.Find ("ambient_audio_source").GetComponent<AudioSource>();
		narration_audio_source = GameObject.Find ("narration_audio_source").GetComponent<AudioSource>();
		num_of_narrations = narration_clips.Length;
		narrations = new Narration[num_of_narrations];

		//Load all audio clips, with type and trigger for narration
		for(int i = 0; i<num_of_narrations; i++) {
			Narration narration = new Narration (narration_clips[i], types[i], i,narration_delays[i]);
			narrations[i] = narration;

		}

		/*When the game starts, it is quiet and the first narration plays*/
		current_narration_index = 0;
		main_source.Stop ();
		playNarrationOfTrigger (ObjectTriggerType.STORY_START);
		main_music_trigger = false;

	}

	//Priority logic
	bool canIPlay(int index) {
		int new_priority = (int)narrations [index].type;
		int current_priority = (int)narrations [current_narration_index].type;

		if (!narration_audio_source.isPlaying)
			return true;

		if (new_priority <= current_priority)
			return true;

		return false;
	}
	//returns the index of valid narration object
	int searchNarrationByTrigger(ObjectTriggerType _trigger) {
		for (int i = 0; i < num_of_narrations; i++) {
			if (narrations [i].trigger == _trigger)
				return i;
		}
		Debug.Log ("Could not find narration");
		return -1;
	}

	void fadeInMainTheme(bool playFromPause, float time =0.0f) {
		main_source.time = time;
		main_source.loop = true;
		StartCoroutine( audio_util.play_sound_fade_in( main_source, 2, playFromPause ) );

	}
	void lowerAllVolumes() {
		main_source.volume = 0.5f;
		ambient_source.volume = 0.5f;
		for (int i = 0; i < lantern_stun_sources.Length; i++) {
			lantern_stun_sources [i].volume = 0.295f;
		}
	}
	void restoreAllVolumes() {
		main_source.volume = 1.0f;
		ambient_source.volume = 1.0f;
		for (int i = 0; i < lantern_stun_sources.Length; i++) {
			lantern_stun_sources [i].volume = 0.4f;
		}
	}

	IEnumerator playNarrationOfIndex(int index) {
		current_narration_index = index;
		lowerAllVolumes ();
		narration_audio_source.clip = narrations [index].clip;
		narration_audio_source.loop = false;
		narration_audio_source.volume = 1.0f;
		narration_audio_source.PlayDelayed(narrations[index].delay);
		yield return new WaitForSeconds (narration_audio_source.clip.length);
		restoreAllVolumes ();

	}
		
	void playNarrationOfTrigger(ObjectTriggerType _trigger) {
		
		int index = searchNarrationByTrigger (_trigger);
		if(canIPlay(index) ){
			StartCoroutine ("playNarrationOfIndex", index);
		}
	}
	public void playLightPickupMusic() {

		// Music -------------------------------------------------------------------------
		/*The light pickup is an ambient sound.The main theme pauses to let the sound play and then resumes from where it had paused. Eventually we can add a fade-out/fade-in for both*/
		if (main_source.isPlaying) {
			main_source.Pause ();
		}

		ambient_source.clip = light_variation;
		ambient_source.loop = false;
		ambient_source.volume = 0.5f;
		StartCoroutine( audio_util.play_sound_fade_in( ambient_source, 2, false ) );


	}

	public void playLightPickupNarration(int lightNumber) {
		// Narration ---------------------------------------------------------------------
		if (lightNumber == 0) {
			playNarrationOfTrigger(ObjectTriggerType.LIGHT_1);
			main_source.Play ();
		}
		if (lightNumber == 1) {
			playNarrationOfTrigger(ObjectTriggerType.LIGHT_2);
			float timePlayed = main_source.time;
			main_source.clip = main_theme [lightNumber];
			fadeInMainTheme (false, timePlayed);

		}
		if (lightNumber == 2) {
			playNarrationOfTrigger(ObjectTriggerType.LIGHT_3);
			float timePlayed = main_source.time;
			main_source.clip = main_theme [lightNumber];
			fadeInMainTheme (false, timePlayed);

		}
		if (lightNumber == 3) {
			playNarrationOfTrigger(ObjectTriggerType.LIGHT_4);
			//main_source = main_theme [lightNumber];

			//back_home_guide.SetActive (true);
		}
	}

	void Update() {

		if (!main_music_trigger) {
			if ((narration_audio_source.isPlaying == false)&&(current_narration_index==0)) {
				main_source.Play ();
				main_music_trigger = true;
			}
		}


		/*if((ambient_source.isPlaying == false && main_source.isPlaying == false)) {
			StartCoroutine( audio_util.play_sound_fade_in( main_source, 2, true ) );
		}*/



		//Transition from intro 1 to intro 2
		/*Note 2: As in Note 1 player regains controls here*/
		//narration_transition_with_dependency (0);


		/*if ((enemy_name == "spider")||(enemy_name == "spider (2)") || (enemy_name == "spider (1)")) {
			narration_transition_without_dependency (10,0); //Pass the narration index that you want to change it to
		} 
		if (((enemy_name == "spider")||(enemy_name == "spider (2)") || (enemy_name == "spider (1)"))&& (has_played_before[10])) {
			narration_transition_without_dependency (11,0); //Pass the narration index that you want to change it to
		} */

		/*if ((Vector3.Distance (player.transform.position,new Vector3(-25.23f,3.29f,3.81f)) < 12)&&(GameManager.progressState==4)) {
			narration_transition_without_dependency (12, 0);
			narration_transition_with_dependency (12);
			winning_particles.SetActive (true);
			back_home_guide.SetActive (false);
			victory_message.SetActive (true);

			
		}*/
		/*if (narration_audio_source.isPlaying == false) {
			main_source.volume = 1.0f;
			for (int i = 0; i < 5; i++) {
				lantern_stun_sources [i].volume = 1.0f;
			}
		} 
		else {
			for (int i = 0; i < 5; i++) {
				lantern_stun_sources [i].volume = 0.5f;
			}
		}*/
		
	}



};

