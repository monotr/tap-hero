using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {
	public RaycastHit[] hits;
	private SpawnScript listChange;
	public Text announce_txt;
	public Text score_txt, combo_txt;
	public int score, combo;
	//public EnemyHPScript enemyData;
	public int perfects, greats, almosts, boos;
	private int padColor;
	public Slider playerHp;
	public GameObject leftHand, rightHand;
	private Animator handAnimsR, handAnimsL;
	private int posManoL, posManoR;
	public AudioSource audio;
	public int lastColorL, lastColorR;

	private float blueKilled, redKilled, greenKilled, yellowKilled;
	public float maxBlueCircle, maxRedCircle, maxGreenCircle, maxYellowCircle;
	private float currBlueCircle, currRedCircle, currGreenCircle, currYellowCircle;
	public GameObject[] powerUpsCircles;

	void Start () {
		lastColorL = 2;
		lastColorR = 3;
		posManoR = 0;
		posManoL = 0;
		handAnimsR = rightHand.GetComponent<Animator>();
		handAnimsL = leftHand.GetComponent<Animator>();

		perfects = greats = almosts = boos = 0;

		listChange = GameObject.Find ("Spawner").GetComponent<SpawnScript> ();
		score_txt.text = "SCORE: " +  score.ToString();
	}


	void Update () {

		if (Input.GetKeyDown (KeyCode.A)) {
			switch(posManoL){
			case 0:
				handAnimsL.SetTrigger("m2a1");
				break;
			case 1:
				handAnimsL.SetTrigger("m1a1");
				break;
			case 2:
				handAnimsL.SetTrigger("m1a2");
				break;
			case 3:
				handAnimsL.SetTrigger("m2a2");
				break;
			}
			posManoL++;
			if(posManoL==4)
				posManoL = 0;

		}

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
			playerHp.value--;
		}
		else if(hits.Length == 2){
			if(hits[0].collider.tag == "Pad" && hits[1].collider.tag == "Hits"){
						switch(hits[0].collider.name){
						case "Red_pad":
							switch(lastColorL){
							case 2:
								handAnimsL.SetTrigger("m2a2");
								lastColorL = 1;
								break;
							case 1:
								handAnimsL.SetTrigger("m1a2");
								lastColorL = 1;
								break;
							}
							padColor = 0;
							break;
						case "Green_pad":
							switch(lastColorR){
							case 3:
								handAnimsR.SetTrigger("m1a2");
								lastColorR = 4;
								break;
							case 4:
								handAnimsR.SetTrigger("m2a2");
								lastColorR = 4;
								break;
							}
							padColor = 1;
							break;
						case "Yellow_pad":
							switch(lastColorL){
							case 2:
								handAnimsL.SetTrigger("m2a1");
								lastColorL = 1;
								break;
							case 1:
								handAnimsL.SetTrigger("m1a1");
								lastColorL = 1;
								break;
							}
							padColor = 2;
							break;
						case "Blue_pad":
							switch(lastColorR){
							case 3:
								handAnimsR.SetTrigger("m1a1");
								lastColorR = 3;
								break;
							case 4:
								handAnimsR.SetTrigger("m2a1");
								lastColorR = 3;
								break;
							}
							padColor = 3;
							break;
						}

						Vector3 hitpadPos = hits[0].collider.transform.position;
				GameObject hittedHit = hits[1].collider.gameObject;
				PadHitted (hitpadPos, hittedHit);
			}
			else if(hits[1].collider.tag == "Pad" && hits[0].collider.tag == "Hits"){
						switch(hits[1].collider.name){
						case "Red_pad":
							switch(lastColorL){
							case 2:
								handAnimsL.SetTrigger("m2a2");
								lastColorL = 1;
								break;
							case 1:
								handAnimsL.SetTrigger("m1a2");
								lastColorL = 1;
								break;
							}
							padColor = 0;
							break;
						case "Green_pad":
							switch(lastColorR){
							case 3:
								handAnimsR.SetTrigger("m1a2");
								lastColorR = 4;
								break;
							case 4:
								handAnimsR.SetTrigger("m2a2");
								lastColorR = 4;
								break;
							}
							padColor = 1;
							break;
						case "Yellow_pad":
							switch(lastColorL){
							case 2:
								handAnimsL.SetTrigger("m2a1");
								lastColorL = 1;
								break;
							case 1:
								handAnimsL.SetTrigger("m1a1");
								lastColorL = 1;
								break;
							}
							padColor = 2;
							break;
						case "Blue_pad":
							switch(lastColorR){
							case 3:
								handAnimsR.SetTrigger("m1a1");
								lastColorR = 3;
								break;
							case 4:
								handAnimsR.SetTrigger("m2a1");
								lastColorR = 3;
								break;
							}
							padColor = 3;
							break;
						}
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
	void setColors(){

		}


	void PadHitted(Vector3 padPos, GameObject hitted){
			audio.pitch = Random.Range (-1,2);
		audio.Play ();
		Vector3 hitPos = hitted.transform.position;
			float dif = Mathf.Abs(hitted.GetComponent<HitMoveScript> ().timerOnRoad) -
				hitted.GetComponent<HitMoveScript> ().segundos;

	//print ("hit time: " + Mathf.Abs(hitted.GetComponent<HitMoveScript> ().timerOnRoad) +
	//	" segundos: " + hitted.GetComponent<HitMoveScript> ().segundos +
	//	" dif: " + dif);

		if (Mathf.Abs(dif) <= 0.1f) {
			score += 3;
			//enemyData.GetHurt (3);
			announce_txt.color = Color.yellow;
			announce_txt.text = "PERFECT!";
				perfects ++;
				hitted.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
	} else if (Mathf.Abs(dif) <= 0.3f) {
			score += 2;
			//enemyData.GetHurt (2);
			announce_txt.color = Color.green;
			announce_txt.text = "GREAT!";
				greats ++;
				hitted.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
	} else if (Mathf.Abs(dif) <= 0.4f) {
			score += 1;
			//enemyData.GetHurt (1);
			announce_txt.color = Color.cyan;
			announce_txt.text = "ALMOST!";
				almosts ++;
				hitted.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
	} else if (Mathf.Abs(dif) <= 0.5f) {
			announce_txt.color = Color.magenta;
			announce_txt.text = "BOO!";
				boos ++;
				hitted.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
		}
		
		hitted.GetComponent<HitMoveScript> ().hitted = true;
		hitted.GetComponent<SpriteRenderer> ().enabled = false;
			float scaleColor = 0;

			if (listChange.killEnemy (padColor)) {
				switch(padColor){
				case 0: //r
					currRedCircle++;
					redKilled++;
					scaleColor = currRedCircle/maxRedCircle;
					powerUpsCircles[0].transform.localScale = new Vector3(scaleColor,scaleColor,scaleColor);
					break;
				case 1: //g
					currGreenCircle++;
					greenKilled++;
					scaleColor = currGreenCircle/maxGreenCircle;
					powerUpsCircles[1].transform.localScale = new Vector3(scaleColor,scaleColor,scaleColor);
					break;
				case 2: //y
					currYellowCircle++;
					yellowKilled++;
					scaleColor = currYellowCircle/maxYellowCircle;
					powerUpsCircles[2].transform.localScale = new Vector3(scaleColor,scaleColor,scaleColor);
					break;
				case 3: //b
					currBlueCircle++;
					blueKilled++;
					scaleColor = currBlueCircle/maxBlueCircle;
					powerUpsCircles[3].transform.localScale = new Vector3(scaleColor,scaleColor,scaleColor);
					break;
				}
				combo++;
		}

		combo_txt.text = "COMBO: " + combo.ToString ();
		score_txt.text = "SCORE: " + score.ToString ();
	}
	
}
