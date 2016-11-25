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
	STORY_END,

	ENEMY,
	CLIMBING_START,
	CLIMBING_END,
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
	SPONGE
	/*
	RANDOM*/


}
struct Narration {
	public AudioClip clip;
	public NarrationType type;
	public ObjectTriggerType trigger;

	public Narration(AudioClip _clip, NarrationType _type, int _trigger) {
		clip = _clip;
		type = _type;
		trigger = (ObjectTriggerType)_trigger;
	}
}
public class MusicManager : MonoBehaviour {
	private string[] narrationFileNames = {
		//DELETETHISLINE audioclipfilename,type
		"vox_story_start_proc.wav",
		/*"vox_light_pickup_01_proc.wav",
		"vox_light_pickup_02_proc.wav",
		"vox_light_pickup_03_proc.wav",
		"vox_light_pickup_04_proc.wav",
		"vox_story_end_proc.wav",*/

		/*"vox_enemy_encounter_proc.wav",
		"vox_puzzle_climbing_start.wav",
		"vox_puzzle_climbing_end_proc.wav",
		"vox_puzzle_domino_proc.wav",
		"vox_puzzle_water_proc.wav",

		"vox_wand_proc.wav",
		"vox_boat_proc.wav",
		"vox_chessboard_proc.wav",
		"vox_clock_proc.wav",
		"vox_domino_proc.wav",
		"vox_earphones_proc.wav",
		"vox_fathers_day_proc.wav",
		"vox_house_of_cards_proc.wav",
		"vox_paper_crane_proc.wav",
		"vox_rubiks_proc.wav",
		"vox_scrabble_proc.wav",
		"vox_spinning_top_proc.wav",
		"vox_sponge_proc.wav",*/

		/*"vox_idle_01_proc.wav",
		"vox_idle_02_proc.wav",
		"vox_idle_03_proc.wav"*/
	};
	private NarrationType[] types = {
		NarrationType.PROGRESS,
		NarrationType.PROGRESS,
		NarrationType.PROGRESS,
		NarrationType.PROGRESS,
		NarrationType.PROGRESS,
		NarrationType.PROGRESS,

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

		/*
		NarrationType.IDLE,
		NarrationType.IDLE,
		NarrationType.IDLE*/


	};

	public GameObject player;
	//Needs to move elsewhere later
	public GameObject victory_message;
	public GameObject winning_particles;
	public GameObject back_home_guide;

	//Audio sources
	private AudioSource main_source;			//Main theme music
	private AudioSource ambient_source;			//Ambient sounds like light pickups
	private AudioSource narration_audio_source;	//Narration
	public AudioSource[] lantern_stun_sources;	//Needs to be public unfortunately. Might find fix for this later

	//Audio clips
	private AudioClip main_theme;
	private AudioClip light_variation;

	//Narration essentials
	private Narration[] narrations;
	private int num_of_narrations = 0;	
	//private int current_narrative_index = 0;
	public bool []has_played_before;
	public AudioClip[] narration_clips;

	public static string enemy_name;
	//public static string light_name;

	void Start() {

		//Assign all sources
		main_source = GameObject.Find ("main_audio_source").GetComponent<AudioSource>();
		ambient_source = GameObject.Find ("ambient_audio_source").GetComponent<AudioSource>();
		narration_audio_source = GameObject.Find ("narration_audio_source").GetComponent<AudioSource>();;
		num_of_narrations = narrationFileNames.Length;
		narrations = new Narration[num_of_narrations];
		//Load all audio clips, with type and trigger for narration
		for(int i = 0; i<num_of_narrations; i++) {
			/*string fileName = "Assets/Audio/Narrations/" + narrationFileNames [i];
			AudioClip narrationClip = Resources.Load<AudioClip>("Audio/Narrations/vox_story_start_proc");*/
			/*if (narrationClip == null)
				Debug.Log ("waaaah");
				//Debug.Log ("STORY START: "+narrationClip.length);*/
			Narration narration = new Narration (narration_clips[i], types [i], i);
			narrations[i] = narration;

		}
		//Debug.Log ("STORY START: "+ narrations[0].clip);

		//main_theme = (AudioClip)Resources.Load ("Audio/Music/main_theme_01.wav");
		//light_variation = (AudioClip)Resources.Load ("Audio/Music/light_pickup_01a.wav");

		/*When the game starts, it is quiet and the first narration plays*/
		int index = searchNarrationByTrigger (ObjectTriggerType.STORY_START);
		narration_audio_source.clip = narrations [index].clip;
		narration_audio_source.loop = false;
		narration_audio_source.volume = 1.0f;
		narration_audio_source.Play();	
		/*When the game starts the background music plays softly while the narrator speaks.*/
		/*main_source.clip = main_theme;
		main_source.loop = true;
		ambient_source.Stop ();
		main_source.volume = 0.5f;
		StartCoroutine(audio_util.play_sound_fade_in(main_source,2,false));
		//main_source.Play ();*/

		/*Note 1: Would love to take away player control at this intro_clip. Maybe just a dark screen and her voice playing*/
		/*narration_audio_source.clip = narration [current_narrative_index];
		narration_audio_source.loop = false;
		narration_audio_source.volume = 1.0f;
		narration_audio_source.PlayDelayed (2);					//Sounds weird if not delayed
		has_played_before[0] = true;*/

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
	/*void narration_transition_with_dependency (int previous_narration_index) {
		if (has_played_before [previous_narration_index + 1] == false) {
			if ((narration_audio_source.isPlaying == false) && (current_narrative_index == previous_narration_index)) {
				current_narrative_index++;
				narration_audio_source.clip = narration [current_narrative_index];
				narration_audio_source.loop = false;
				narration_audio_source.volume = 1.0f;
				main_source.volume = 0.5f;
				narration_audio_source.Play ();	
			}
		}

	}

	void narration_transition_without_dependency (int next_narration_index, int delayInSeconds) {
		if (!has_played_before[next_narration_index]) {
			if ((narration_audio_source.isPlaying == false) && (current_narrative_index != next_narration_index)) {
				current_narrative_index = next_narration_index;
				narration_audio_source.clip = narration [current_narrative_index];
				narration_audio_source.loop = false;
				narration_audio_source.volume = 1.0f;
				main_source.volume = 0.5f;
				narration_audio_source.PlayDelayed (delayInSeconds);	
				has_played_before[next_narration_index] = true;
			}
		}
	}*/


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

	/*public void playLightPickupNarration(int lightNumber) {
		// Narration ---------------------------------------------------------------------
		if (lightNumber == 0) {
			narration_transition_without_dependency (4,2); //Pass the narration index that you want to change it to
		}

		if (lightNumber == 1) {
			narration_transition_without_dependency (6, 2);

		}
		if (lightNumber == 2) {
			narration_transition_without_dependency (7, 2);
			narration_transition_with_dependency (7);
		}
		if (lightNumber == 3) {
			narration_transition_without_dependency (9, 2);
			back_home_guide.SetActive (true);
		}
	}*/

	void Update() {



		if((ambient_source.isPlaying == false && main_source.isPlaying == false)) {
			StartCoroutine( audio_util.play_sound_fade_in( main_source, 2, true ) );
		}

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
		if (narration_audio_source.isPlaying == false) {
			main_source.volume = 1.0f;
			for (int i = 0; i < 5; i++) {
				lantern_stun_sources [i].volume = 1.0f;
			}
		} 
		else {
			for (int i = 0; i < 5; i++) {
				lantern_stun_sources [i].volume = 0.5f;
			}
		}
		
	}

}
