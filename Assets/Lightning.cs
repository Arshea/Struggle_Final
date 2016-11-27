using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour {

	private Light lightningLight;

	private float lastLightningTime = 0.0f;
	public float minDelay = 10.5f; // Must wait this long after lightning to do another lightning
	private float lastCheckTime = 0.0f;
	public float checkIntervalTime = 0.2f; // Only checks every x seconds whether to do another lightning
	public float probability = 0.1f;
	public float lightningIntensity = 0.3f;
	public float flashTime = 0.1f;

	private bool lightningOn = false;

	// Use this for initialization
	void Start () {

		lightningLight = this.GetComponent<Light> ();
		lightningLight.intensity = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {
		float time = Time.time;
		if (time - lastCheckTime > checkIntervalTime) {
			lastCheckTime = time;
			if (!lightningOn && time - lastLightningTime > minDelay) {
				float r = Random.value;
				if (r < probability) {
					lightningLight.intensity = lightningIntensity;
					lastLightningTime = time;
					lightningOn = true;
				}
			}
		}
		if (lightningOn && time - lastLightningTime > flashTime) {
			lightningOn = false;
			lightningLight.intensity = 0.0f;
		}
	}
}
