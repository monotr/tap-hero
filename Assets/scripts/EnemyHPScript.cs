using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHPScript : MonoBehaviour {
	public float enemyHP;
	private float toReduce;
	public Slider hpBarUI;
	public Text enemyHPTxt;
	private SpriteRenderer thisSprite;
	public Sprite[] monsters;
	public Text damageGiven;

	public Text roundtext;
	private int round;
	private float timerAnim = 0.5f;
	public float timerAux;
	private bool animAct = false;
	private Animator animator;

	void Start () {
		animator = this.GetComponent<Animator> ();
		round = 1;
		roundtext.text = round + "/10";
		thisSprite = this.GetComponent<SpriteRenderer> ();
		hpBarUI.maxValue = enemyHP;
		hpBarUI.value = enemyHP;
		enemyHPTxt.text = "HP: " + hpBarUI.value + "/" + hpBarUI.maxValue;
		thisSprite.sprite = monsters[Random.Range(0, monsters.Length)];
		//thisSprite.color = new Color(Random.Range(0,2), Random.Range(0,2), Random.Range(0,2));
	}

	private void stopShake(){
		animator.SetBool ("Hit", false);
	}

	private void Update(){
		if (animAct) {
			if(timerAux > timerAnim)
			{
				timerAux = 0;
				stopShake();
				animAct = false;
			}
			else{
				timerAux += Time.deltaTime;
			}
		}
	}

	public void GetHurt (int damage) {
		enemyHP -= damage;

		Text damageTxt = Instantiate (damageGiven) as Text;
		damageTxt.text = damage.ToString ();
		damageTxt.transform.SetParent (GameObject.Find("Canvas").transform);
		damageTxt.transform.position += new Vector3 (Screen.width/2, Screen.height/2, 0);

		animator.SetTrigger ("Hit");
		animAct = true;
		//Invoke ("stopShake", 1);

		if (enemyHP <= 0) {
			hpBarUI.value = hpBarUI.maxValue;
			enemyHP = hpBarUI.maxValue;
			thisSprite.sprite = monsters[Random.Range(0, monsters.Length)];

			round ++;
			if(round<10)
				roundtext.text = round + "/10";
			else if(round == 10)
				roundtext.text = "BOSS FIGHT";
			else if(round == 11){
				round = 1;
				roundtext.text = round + "/10";
			}

			//thisSprite.color = new Color(Random.Range(0,2), Random.Range(0,2), Random.Range(0,2));
		}
		else
			hpBarUI.value -= damage;

		enemyHPTxt.text = "HP: " + hpBarUI.value + "/" + hpBarUI.maxValue;


	}
}
