using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {
	public RaycastHit[] hits;
	private SpawnScript listChange;
	public Text announce_txt;
	public Text score_txt, combo_txt;
	public int score, combo;
	public EnemyHPScript enemyData;
	public int perfects, greats, almosts, boos;

	void Start () {

		perfects = greats = almosts = boos = 0;

		listChange = GameObject.Find ("Spawner").GetComponent<SpawnScript> ();
		score_txt.text = "SCORE: " +  score.ToString();
	}


	void Update () {

		#if UNITY_ANDROID && !UNITY_EDITOR
		if (Input.touchCount == 1){
			//Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch (0).position);
			//Ray ray2 = Camera.main.ScreenPointToRay(Input.GetTouch (0).position);
			hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.GetTouch (0).position),Mathf.Infinity);
		#elif UNITY_EDITOR
			if (Input.GetMouseButtonDown (0)) {
			//Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			//Ray ray2 = Camera.main.ScreenPointToRay (Input.mousePosition);
			hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition),Mathf.Infinity);
		#endif

		//if (Physics.RaycastAll (ray, out hits)) {

				print (hits.Length);

		if(hits.Length == 1){
			score -= 1;
			announce_txt.color = Color.red;
			announce_txt.text = "MISS!";
			combo = 0;
		}
		else if(hits.Length == 2){
			if(hits[0].collider.tag == "Pad" && hits[1].collider.tag == "Hits"){
				Vector3 hitpadPos = hits[0].collider.transform.position;
				GameObject hittedHit = hits[1].collider.gameObject;
				PadHitted (hitpadPos, hittedHit);
			}
			else if(hits[1].collider.tag == "Pad" && hits[0].collider.tag == "Hits"){
				Vector3 hitpadPos = hits[1].collider.transform.position;
				GameObject hittedHit = hits[0].collider.gameObject;
				PadHitted (hitpadPos, hittedHit);
			}
		}
		//}
		}
			if (!GetComponent<AudioSource> ().isPlaying)
			print ("music off");
	}

	void PadHitted(Vector3 padPos, GameObject hitted){
		Vector3 hitPos = hitted.transform.position;
			float dif = Mathf.Abs(hitted.GetComponent<HitMoveScript> ().timerOnRoad) -
				hitted.GetComponent<HitMoveScript> ().segundos;

	//print ("hit time: " + Mathf.Abs(hitted.GetComponent<HitMoveScript> ().timerOnRoad) +
	//	" segundos: " + hitted.GetComponent<HitMoveScript> ().segundos +
	//	" dif: " + dif);

		if (Mathf.Abs(dif) <= 0.1f*3) {
			score += 3;
			enemyData.GetHurt (3);
			announce_txt.color = Color.yellow;
			announce_txt.text = "PERFECT!";
				perfects ++;
				hitted.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
			combo++;
	} else if (Mathf.Abs(dif) <= 0.3f*3) {
			score += 2;
			enemyData.GetHurt (2);
			announce_txt.color = Color.green;
			announce_txt.text = "GREAT!";
				greats ++;
				hitted.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
			combo++;
	} else if (Mathf.Abs(dif) <= 0.4f*3) {
			score += 1;
			enemyData.GetHurt (1);
			announce_txt.color = Color.cyan;
			announce_txt.text = "ALMOST!";
				almosts ++;
				hitted.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
			combo++;
	} else if (Mathf.Abs(dif) <= 0.5f*3) {
			announce_txt.color = Color.magenta;
			announce_txt.text = "BOO!";
				boos ++;
				hitted.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
		}
		
		hitted.GetComponent<HitMoveScript> ().hitted = true;
		hitted.GetComponent<SpriteRenderer> ().enabled = false;

		combo_txt.text = "COMBO: " + combo.ToString ();
		score_txt.text = "SCORE: " + score.ToString ();
	}
	
}
