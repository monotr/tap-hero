using UnityEngine;
using System.Collections;

public class HitMoveScript : MonoBehaviour {
	private Vector3 targetPos;
	public float speed;
	private SpawnScript listChange;
	public int colorHit;
	private bool missed = false;
	public bool hitted = false;
	public GameScript dataGame;

	private float initialTime;
	private Transform thisTrans, targetTrans;
	public AudioSource musicTime;
	float avance_por_tiempo;
	public float timerOnRoad;
	private float initialY;

	public float segundos;

	void Start () {
		thisTrans = transform;
		musicTime = GameObject.Find ("Main Camera").GetComponent<AudioSource> ();
		targetTrans = GameObject.Find ("Blue_pad").transform;

		initialTime = musicTime.time;
		float dif = targetTrans.position.y - thisTrans.position.y;
		avance_por_tiempo = (Mathf.Abs (dif) / segundos);
		initialY = thisTrans.position.y;

		dataGame = GameObject.Find ("Main Camera").GetComponent<GameScript> ();
		listChange = GameObject.Find ("Spawner").GetComponent<SpawnScript> ();
		targetPos = new Vector3 (transform.position.x, -10, transform.position.z);
	}

	void FixedUpdate () {
		timerOnRoad = initialTime - musicTime.time;
		transform.position = new Vector3(transform.position.x,
		                                 initialY + (avance_por_tiempo * timerOnRoad),
		                                 transform.position.z);

		if (transform.position.y < -0.7f && !missed && !hitted) {
			listChange.updateAnnoun();
			missed = true;
			dataGame.combo = 0;
			dataGame.combo_txt.text = "COMBO: " + 0;
		}
		if (transform.position.y < -10+4) {
			listChange.spawnedList.Remove(this.gameObject);
			Destroy (this.gameObject);
		}
	}
}
