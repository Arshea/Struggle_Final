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
		2.0f,
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

		0.3f,
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
	public AudioClip[] light_variation;

	//Narration essentials
	private Narration[] narrations;
	private int num_of_narrations = 0;	
	public bool []has_played_before;
	public AudioClip[] narration_clips;
	private int current_narration_index;

	private bool main_music_trigger;
	public static string enemy_name;
	private float mainSourceVolInit;
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

		mainSourceVolInit = main_source.volume;  //Saving this init volume off to use to match volumes for the going home intro.
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
		if(time>0.0f)
			main_source.time = time;
		main_source.loop = true;
		StartCoroutine( audio_util.play_sound_fade_in( main_source, 3, playFromPause ) );

	}
	void lowerAllVolumes() {
		main_source.volume = 0.5f;
		ambient_source.volume = 0.3f;
		for (int i = 0; i < lantern_stun_sources.Length; i++) {
			lantern_stun_sources [i].volume = 0.295f;
		}
	}
	void restoreAllVolumes() {
		main_source.volume = 0.8f;
		ambient_source.volume = 0.8f;
		for (int i = 0; i < lantern_stun_sources.Length; i++) {
			lantern_stun_sources [i].volume = 0.4f;
		}
	}

	IEnumerator playNarrationOfIndex(int index) {
		current_narration_index = index;
		//lowerAllVolumes ();
		narration_audio_source.clip = narrations [index].clip;
		narration_audio_source.loop = false;
		narration_audio_source.volume = 1.0f;
		narration_audio_source.PlayDelayed(narrations[index].delay);
		yield return new WaitForSeconds (narration_audio_source.clip.length);
		//restoreAllVolumes ();
	}

	IEnumerator fadeOutMainTheme() {
		float init_volume = main_source.volume;

		float startTime = Time.time;
		float complete = 0.0f;
		float endTime = 5.0f;
		while (Time.time - startTime < endTime) {
			complete = (Time.time - startTime) / endTime;
			main_source.volume = Mathf.Lerp (init_volume, 0.0f, complete);
			yield return null;
		}
	}
	void playNarrationOfTrigger(ObjectTriggerType _trigger) {
		
		int index = searchNarrationByTrigger (_trigger);
		if(canIPlay(index) ){
			StartCoroutine ("playNarrationOfIndex", index);
			if (_trigger == ObjectTriggerType.STORY_END) {
				StartCoroutine ("fadeOutMainTheme");
			}
		}
	}
	public void playLightPickupMusic(int lightNumber) {

		// Music -------------------------------------------------------------------------
		/*The light pickup is an ambient sound.The main theme pauses to let the sound play and then resumes from where it had paused. Eventually we can add a fade-out/fade-in for both*/
		if (main_source.isPlaying) {
			main_source.Pause ();
		}
		if (narration_audio_source.isPlaying) {
			narration_audio_source.Stop ();
		}
		if(lightNumber == 0)
			ambient_source.clip = light_variation[0];
		if (lightNumber == 3) {
			ambient_source.clip = light_variation [2];
			ambient_source.volume = mainSourceVolInit;  //Going home intro is normalized with the going home loop which is played on main_source, so we want the volumes to be equal here.  Can't use main_source.volume since it is 0 at this point in time.
			StartCoroutine ("playGoingHomeMusic");
		}
		else if(lightNumber >0)
			ambient_source.clip = light_variation[lightNumber-1];

		ambient_source.loop = false;
		StartCoroutine( audio_util.play_sound_fade_in( ambient_source, 2, false ) );
			

	}

	public void playLightPickupNarration(int lightNumber) {
		// Narration ---------------------------------------------------------------------
		if (lightNumber == 0) {
			playNarrationOfTrigger(ObjectTriggerType.LIGHT_1);
			fadeInMainTheme (true);
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
			//back_home_guide.SetActive (true);
		}
	}

	IEnumerator playGoingHomeMusic() {
		ambient_source.Play ();
		yield return new WaitForSeconds (11.20f);
		main_source.clip = main_theme [3];
		StartCoroutine( audio_util.play_sound_fade_in( main_source, 2, false ) );
		Debug.Log("GOING HOME \\O/");

	}

	void Update() {

		if (!main_music_trigger) {
			if ((narration_audio_source.isPlaying == false)&&(current_narration_index==0)) {
				main_source.PlayDelayed (1.0f);
				main_music_trigger = true;
			}
		}
			
		
	}



};

