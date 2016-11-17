using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public GameObject player;
	//Needs to move elsewhere later
	public GameObject victory_message;
	public GameObject winning_particles;
	public GameObject back_home_guide;
	public AudioSource main_source;				//Main theme music
	public AudioSource ambient_source;			//Ambient sounds like light pickups
	public AudioSource narration_audio_source;	//Narration
	public AudioSource[] lantern_stun_sources;

	public AudioClip main_theme;
	public AudioClip light_variation;
	public AudioClip  []narration;

	private int current_narrative_index = 0;
	public bool []has_played_before;

	public static string enemy_name;
	public static string light_name;

	private static int number_of_lights_picked_up;
	void Start() {
		
		number_of_lights_picked_up = 0;
		/*When the game starts the background music plays softly while the narrator speaks.*/
		main_source.clip = main_theme;
		main_source.loop = true;
		ambient_source.Stop ();
		main_source.volume = 0.5f;
		StartCoroutine(audio_util.play_sound_fade_in(main_source,2,false));
		//main_source.Play ();

		/*Note 1: Would love to take away player control at this intro_clip. Maybe just a dark screen and her voice playing*/
		narration_audio_source.clip = narration [current_narrative_index];
		narration_audio_source.loop = false;
		narration_audio_source.volume = 1.0f;
		narration_audio_source.PlayDelayed (2);					//Sounds weird if not delayed
		has_played_before[0] = true;

	}
	void narration_transition_with_dependency (int previous_narration_index) {
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
	}

	void Update() {

		/*The light pickup is an ambient sound.The main theme pauses to let the sound play and then resumes from where it had paused. Eventually we can add a fade-out/fade-in for both*/
		if (LanternManager.pickingUpAudioTrigger) {
			if (main_source.isPlaying) {

				main_source.Pause ();
			}
				
			ambient_source.clip = light_variation;
			ambient_source.loop = false;
			ambient_source.volume = 0.5f;
			StartCoroutine( audio_util.play_sound_fade_in( ambient_source, 2, false ) );
			//ambient_source.Play ();
			number_of_lights_picked_up++;
			LanternManager.pickingUpAudioTrigger = false;
		}

		if((ambient_source.isPlaying == false && main_source.isPlaying == false)) {
			StartCoroutine( audio_util.play_sound_fade_in( main_source, 2, true ) );
		}

		//Transition from intro 1 to intro 2
		/*Note 2: As in Note 1 player regains controls here*/
		//narration_transition_with_dependency (0);

		if (enemy_name == "Freggo") {
			narration_transition_without_dependency (2, 0); //Pass the narration index that you want to change it to
		} 
		else if (enemy_name == "FreggoStun") {
			narration_transition_with_dependency (2); //Pass the previous narration index (the one that this one is dependent on)
		} 
		if (enemy_name == "Freggo (3)") {
			narration_transition_without_dependency (5,0); //Pass the narration index that you want to change it to
		} 

		/*if ((enemy_name == "spider")||(enemy_name == "spider (2)") || (enemy_name == "spider (1)")) {
			narration_transition_without_dependency (10,0); //Pass the narration index that you want to change it to
		} 
		if (((enemy_name == "spider")||(enemy_name == "spider (2)") || (enemy_name == "spider (1)"))&& (has_played_before[10])) {
			narration_transition_without_dependency (11,0); //Pass the narration index that you want to change it to
		} */
		if (light_name == "LightPickup_yellow") {
			narration_transition_without_dependency (4,1); //Pass the narration index that you want to change it to
		}

		if (number_of_lights_picked_up == 2) {
			narration_transition_without_dependency (6, 8);

		}
		if (number_of_lights_picked_up == 3) {
			narration_transition_without_dependency (7, 8);
			narration_transition_with_dependency (7);
		}
		if (number_of_lights_picked_up == 4) {
			narration_transition_without_dependency (9, 8);
			back_home_guide.SetActive (true);
		}
		if ((Vector3.Distance (player.transform.position,new Vector3(-25.23f,3.29f,3.81f)) < 12)&&(number_of_lights_picked_up==4)) {
			narration_transition_without_dependency (12, 0);
			narration_transition_with_dependency (12);
			winning_particles.SetActive (true);
			back_home_guide.SetActive (false);
			victory_message.SetActive (true);

			
		}
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
