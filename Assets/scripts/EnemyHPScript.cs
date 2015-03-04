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

	public Text roundtext;
	private int round;

	void Start () {
		round = 1;
		roundtext.text = round + "/10";
		thisSprite = this.GetComponent<SpriteRenderer> ();
		hpBarUI.maxValue = enemyHP;
		hpBarUI.value = enemyHP;
		enemyHPTxt.text = "HP: " + hpBarUI.value + "/" + hpBarUI.maxValue;
		thisSprite.sprite = monsters[Random.Range(0, monsters.Length)];
		//thisSprite.color = new Color(Random.Range(0,2), Random.Range(0,2), Random.Range(0,2));
	}
	

	public void GetHurt (int damage) {
		enemyHP -= damage;

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
