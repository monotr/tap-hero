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

	void Start () {
		dataGame = GameObject.Find ("Main Camera").GetComponent<GameScript> ();
		listChange = GameObject.Find ("Spawner").GetComponent<SpawnScript> ();
		targetPos = new Vector3 (transform.position.x, -10, transform.position.z);
	}

	void FixedUpdate () {
		transform.position = Vector3.Lerp (transform.position, targetPos, Time.deltaTime * speed);

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
