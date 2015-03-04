using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.IO;

public class SpawnScript : MonoBehaviour, AudioProcessor.AudioCallbacks {
	public GameObject[] obj;
	public List<GameObject> spawnedList;
	public Text announcement_txt;
	public List<byte> MIDIgoodnotes;
	public Slider tempoSlider;
	private float timer = 1;
	private bool init = false;
	public AudioSource musica;

	void Start () {
		Invoke ("playMusic", 2.75f);
		spawnedList = new List<GameObject>();
		Spawn ();

		byte[] MIDI = File.ReadAllBytes("test2.mid");
		for(int i=0;i<MIDI.Length;i++){
			if(MIDI[i]==144) {
				MIDIgoodnotes.Add(MIDI[i+1]);
				//print(MIDI[i+1]);  //This is the note value
			}
		}
	}

	public void Update(){
		if (timer < 0 && !init) {
			AudioProcessor processor = FindObjectOfType<AudioProcessor> ();
			this.GetComponent<AudioSource>().Play();
			processor.addAudioCallback (this);
			init = true;
		} else if (!init)
			timer -= Time.deltaTime;
	}

	private void playMusic(){
		musica.enabled = true;
	}

	public void updateAnnoun(){
		announcement_txt.color = Color.red;
		announcement_txt.text = "MISS!";
	}

	public void onOnbeatDetected()
	{
		Debug.Log("Beat!!!");
		Spawn ();
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
	
	void Spawn () {
		int rando = 0;
		int randoPos = Random.Range(1,4);
		Vector3 spawnPos = transform.position;

		switch (randoPos) {
		case 2:
			spawnPos = new Vector3 (transform.position.x + 0.65f, transform.position.y, transform.position.z);
			rando = 1;
			break;
		case 3:
			spawnPos = new Vector3 (transform.position.x + 1.3f, transform.position.y, transform.position.z);
			rando = 2;
			break;
		}

		GameObject spawned = Instantiate (obj [rando], spawnPos, Quaternion.identity) as GameObject;
		spawnedList.Add (spawned);
		//dingSound.Play();
		//Invoke ("Spawn", tempo);
	}
}
