using UnityEngine;
using System;

public class MusicManagerScript : MonoBehaviour, AudioProcessor.AudioCallbacks {
	float timer = 1;
	bool init = false;

	void Start () {

	}
	

	void Update () {
		/*if (timer < 0 && !init) {
			AudioProcessor processor = FindObjectOfType<AudioProcessor> ();
			processor.addAudioCallback (this);
			init = true;
		} else if (!init)
			timer -= Time.deltaTime;*/
	}

	//this event will be called every time a beat is detected.
	//Change the threshold parameter in the inspector
	//to adjust the sensitivity
	public void onOnbeatDetected()
	{
		Debug.Log("Beat!!!");
	}
	
	//This event will be called every frame while music is playing
	public void onSpectrum(float[] spectrum)
	{
		//The spectrum is logarithmically averaged
		//to 12 bands
		
		for (int i = 0; i < spectrum.Length; ++i)
		{
			Vector3 start = new Vector3(i, 0, 0);
			Vector3 end = new Vector3(i, spectrum[i], 0);
			Debug.DrawLine(start, end);
		}
	}
}
