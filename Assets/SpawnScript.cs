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
	public AudioSource musica, beatmusic;
	private bool slowMotion = false;
	private float tiempoEntreNotas, tiempoAux;
	private int notasSalidas;
	public float bpm, tempo, Padtempo;
	public GameObject[] pads;
	public Animator[] padsAnim;

	void Start () {
		setTempos (bpm);
		tiempoAux = 0;
		tiempoEntreNotas = 0.5f;
		//Invoke ("playMusic", 3.0f);
		spawnedList = new List<GameObject>();

		for (int i=0; i<pads.Length; i++) {
			padsAnim[i] = pads[i].GetComponent<Animator>();
		}

		byte[] MIDI = File.ReadAllBytes("test2.mid");
		for(int i=0;i<MIDI.Length;i++){
			if(MIDI[i]==144) {
				MIDIgoodnotes.Add(MIDI[i+1]);
				//print(MIDI[i+1]);  //This is the note value
			}
		}
		beatPad ();
		Spawn ();
	}

	private void setTempos(float bmp){
		Padtempo = 60 / bpm;
		tempo = 1/(bpm / 60);
	}

	public void Update(){
		//print ("music: " + musica.time + " beat: " + beatmusic.time);

		float[] spectrum = beatmusic.GetSpectrumData(1024, 0, FFTWindow.Hamming);
		int i = 1;
		while (i < 1023) {
			Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
			Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
			Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
			Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
			i++;
		}
		//print (spectum.Length);

		if (!musica.isPlaying && beatmusic.time >= 3)
			playMusic ();

		//print("beat: " + beatmusic.time + " dps: " + AudioSettings.dspTime);

		tiempoAux += Time.deltaTime;
		if (tiempoAux > tiempoEntreNotas) {
			tiempoAux = 0;
			notasSalidas = 0;
		}

		if (timer < 0 && !init) {
			AudioProcessor processor = FindObjectOfType<AudioProcessor> ();
			//this.GetComponent<AudioSource>().Play();
			processor.addAudioCallback (this);
			init = true;
		} else if (!init)
			timer -= Time.deltaTime;

		if(slowMotion){
			musica.pitch = 0.5f;
			beatmusic.pitch = 0.5f;
		}
		else if(musica.pitch != 1){
			musica.pitch = 1;
			beatmusic.pitch = 1;
		}
	}

	public void slowMo(){
		slowMotion = !slowMotion;

		if (slowMotion) {
			bpm /= 2;
			setTempos (bpm);
		}
		else {
			bpm *= 2;
			setTempos (bpm);
		}
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

		if (notasSalidas < 1) {
			//Spawn ();
			notasSalidas = 0;
		}
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
		int randoPos = Random.Range(1,5);
		Vector3 spawnPos = transform.position;

		switch (randoPos) {
		case 2:
			spawnPos = new Vector3 (transform.position.x + 0.4f, transform.position.y, transform.position.z);
			rando = 1;
			break;
		case 3:
			spawnPos = new Vector3 (transform.position.x + 1.03f, transform.position.y, transform.position.z);
			rando = 2;
			break;
		case 4:
			spawnPos = new Vector3 (transform.position.x + 1.43f, transform.position.y, transform.position.z);
			rando = 3;
			break;
		}

		GameObject spawned = Instantiate (obj [rando], spawnPos, Quaternion.identity) as GameObject;
		spawnedList.Add (spawned);

		//dingSound.Play();
		Invoke ("Spawn", tempo);
	}

	private void beatPad(){
		////beat pads
		for (int i=0; i<pads.Length; i++) {
			padsAnim[i].SetTrigger("beat");
		}
		////
		Invoke ("beatPad", Padtempo);
	}
}
