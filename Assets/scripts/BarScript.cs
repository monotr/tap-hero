using UnityEngine;
using System.Collections;

public class BarScript : MonoBehaviour {
	float ancho;
	float posx;
	//public float valormin;
	public float valormax;
	float posminx;
	public float actual;
	public bool semueve = false;
	bool gotvalues = false;

	void Start () {
		ancho = this.GetComponent<RectTransform> ().sizeDelta.x;
		putitaside ();
	}

	void putitaside (){
		posx = transform.parent.transform.position.x;
		posminx = posx - ancho;
		transform.position = new Vector3 (posminx, transform.position.y, transform.position.z);
	}

	void Update () {
		if (semueve) {
			if(!gotvalues){
				putitaside();
				gotvalues = true;
			}
			if(actual < valormax){
				float movebitch = Mathf.Lerp (transform.parent.transform.position.x,
				                              transform.parent.transform.position.x - ancho, actual / valormax);
				transform.position = new Vector3 (movebitch, transform.position.y, transform.position.z);
			}
			if (actual < 0.1){
				semueve = false;
				gotvalues = false;
				putitaside();
			}
		}
	}
}
