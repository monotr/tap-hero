using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using StartApp;

public class GameScript : MonoBehaviour {
	public RaycastHit[] hits;
	private SpawnScript listChange;
	public Text announce_txt;
	public Text score_txt, combo_txt;
	public int score, combo;
	public EnemyHPScript enemyData;

	void Start () {
		/*#if UNITY_ANDROID
		StartAppWrapper.addBanner( 
	      StartAppWrapper.BannerType.AUTOMATIC,
	      StartAppWrapper.BannerPosition.TOP);
		#endif*/

		listChange = GameObject.Find ("Spawner").GetComponent<SpawnScript> ();
		score_txt.text = "SCORE: " +  score.ToString();
	}


	void Update () {
		#if UNITY_ANDROID && !UNITY_EDITOR
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began){
			//Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch (0).position);
			//Ray ray2 = Camera.main.ScreenPointToRay(Input.GetTouch (0).position);
			hits = Physics.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Input.GetTouch (0).position),100);
		#elif UNITY_EDITOR
			if (Input.GetMouseButtonDown (0)) {
			//Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			//Ray ray2 = Camera.main.ScreenPointToRay (Input.mousePosition);
			hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition),Mathf.Infinity);
		#endif

		//if (Physics.RaycastAll (ray, out hits)) {
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
	}

	void PadHitted(Vector3 padPos, GameObject hitted){
		Vector3 hitPos = hitted.transform.position;

		if (hitPos.y <= padPos.y + 0.02f && hitPos.y >= padPos.y - 0.02f) {
			score += 3;
			enemyData.GetHurt (3);
			announce_txt.color = Color.yellow;
			announce_txt.text = "PERFECT!";
			combo++;
		} else if (hitPos.y <= padPos.y + 0.06f && hitPos.y >= padPos.y - 0.06f) {
			score += 2;
			enemyData.GetHurt (2);
			announce_txt.color = Color.green;
			announce_txt.text = "GREAT!";
			combo++;
		} else if (hitPos.y <= padPos.y + 0.1f && hitPos.y >= padPos.y - 0.1f) {
			score += 1;
			enemyData.GetHurt (1);
			announce_txt.color = Color.cyan;
			announce_txt.text = "ALMOST!";
			combo++;
		} else if (hitPos.y <= padPos.y + 0.16f && hitPos.y >= padPos.y - 0.16f) {
			announce_txt.color = Color.magenta;
			announce_txt.text = "BOO!";
		}
		
		hitted.GetComponent<HitMoveScript> ().hitted = true;
		hitted.GetComponent<SpriteRenderer> ().enabled = false;

		combo_txt.text = "COMBO: " + combo.ToString ();
		score_txt.text = "SCORE: " + score.ToString ();
	}
	
}
