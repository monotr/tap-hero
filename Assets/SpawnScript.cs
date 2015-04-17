using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.IO;

public class SpawnScript : MonoBehaviour, AudioProcessor.AudioCallbacks {
	public GameObject obj;
	//public List<GameObject> spawnedList;
	private List<int> enemyListColor, enemyListMonster;
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
	public GameObject[] enemies;
	public Sprite[] monsters;
	public Animator[] padsAnim;
	public Slider playerHp;
	public float tiempoPasado;
	public float tiempoAtaque;
	public int beatsAttack;
	private List<bool> tiempoSpawn;
	private int auxSpawn, auxaCount;
	private float bps;
	private int totalBeatsOnTime;
	public AudioSource audio;
	public float segundos;

	void Start () {
		tiempoSpawn = new List<bool>();
		enemyListColor = new List<int>();
		enemyListMonster = new List<int>();
		for (int i=0; i<5; i++) {
			enemyListColor.Add(Random.Range(0,4));
			enemyListMonster.Add(Random.Range(0,4));
			enemies[i].GetComponent<SpriteRenderer>().sprite = monsters[enemyListMonster[i]];
			Color monsterColor = new Color();
			switch(enemyListColor[i]){
			case 0:
				monsterColor = Color.red;
				break;
			case 1:
				monsterColor = Color.green;
				break;
			case 2:
				monsterColor = Color.yellow;
				break;
			case 3:
				monsterColor = Color.blue;
				break;
			}
			enemies[i].GetComponent<SpriteRenderer>().color = monsterColor;
		}

		setTempos (bpm);
		tiempoAux = 0;
		tiempoEntreNotas = 0.5f;
		//spawnedList = new List<GameObject>();

		for (int i=0; i<pads.Length; i++) {
			padsAnim[i] = pads[i].GetComponent<Animator>();
		}
		beatPad ();
		attackReceived (bpm, beatsAttack, tiempoAtaque);
		Spawn ();
	}

	
	void attackReceived(float bpm, int totalBeats, float tiempo){
		bps = bpm / 60;
		float beatTime = 1 / bps;
		int contador = 0;
		List<int> pos = new List<int> ();
		totalBeatsOnTime = (int)(tiempo * bps);

		while (pos.Count < totalBeats) {
			pos.Add(Random.Range(0,totalBeatsOnTime));
			pos.Distinct();
		}

		while (contador < totalBeatsOnTime) {
			if(!pos.Contains(contador))
				tiempoSpawn.Add(false);
			else{
				tiempoSpawn.Add(true);
			}
			contador ++;
		}
		print (tiempoSpawn.Count);
	}

	public bool killEnemy(int color){
		if (color == enemyListColor [0]) {
			enemyListColor.RemoveAt (0);
			enemyListMonster.RemoveAt (0);
			enemyListColor.Add (Random.Range (0, 4));
			enemyListMonster.Add (Random.Range (0, 4));

			for (int i=0; i<5; i++) {
				enemies [i].GetComponent<SpriteRenderer> ().sprite = monsters [enemyListMonster [i]];
				Color monsterColor = new Color ();
				switch (enemyListColor [i]) {
				case 0:
					monsterColor = Color.red;
					break;
				case 1:
					monsterColor = Color.green;
					break;
				case 2:
					monsterColor = Color.yellow;
					break;
				case 3:
					monsterColor = Color.blue;
					break;
				}
				enemies [i].GetComponent<SpriteRenderer> ().color = monsterColor;
			}
			return true;
		} else
			return false;
	}

	private void setTempos(float bmp){
		Padtempo = 60 / bpm;
		tempo = 1/(bpm / 60);
	}

	public void Update(){

		//print ("music: " + musica.time + " beat: " + beatmusic.time);

		/*float[] spectrum = beatmusic.GetSpectrumData(1024, 0, FFTWindow.Hamming);
		int i = 1;
		while (i < 1023) {
			Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
			Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
			Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
			Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
			i++;
		}*/
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
		playerHp.value--;
	}

	public void onOnbeatDetected()
	{
		//Debug.Log("Beat!!!");

		//Spawn ();
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
		//print ("Beat");
		if (tiempoSpawn[auxSpawn]) {
			Instantiate (obj, transform.position, Quaternion.identity);
			//audio.pitch = Random.Range(1,3);
			//audio.Play ();
			//spawnedList.Add (spawned);
		}
		auxSpawn++;
		int rando = Random.Range (0, 9);
		if (auxSpawn == tiempoSpawn.Count) {
			tiempoAtaque = Random.Range(1,20);
			beatsAttack = (int)(Random.Range(tiempoAtaque, tiempoAtaque * bps));
			tiempoSpawn = new List<bool>();
			attackReceived (bpm, beatsAttack, tiempoAtaque);
			auxSpawn = 0;
			Invoke ("Spawn", segundos*1.25f);
		}
		else if(rando > 2 && rando < 6)
			Invoke ("Spawn", Padtempo / 2);
		else if(rando > 6 && rando < 8)
			Invoke ("Spawn", Padtempo / 3);
		else
			Invoke ("Spawn", Padtempo);
		//dingSound.Play();
	}

	private void beatPad(){
		////beat pads
		for (int i=0; i<pads.Length; i++) {
			padsAnim[i].SetTrigger("beat");
		}
		////
		Invoke ("beatPad", Padtempo);

		//Invoke ("beatPad", tempo);

	}
}
