using UnityEngine;
using System.Collections;

public class selfdestroy : MonoBehaviour {
	public float timetodie = 0.1f;
	public int colortokill;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		timetodie -= Time.deltaTime;
		if(timetodie <= 0)
			Destroy (this.gameObject);
	}
}
