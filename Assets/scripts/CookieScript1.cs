using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class CookieScript1 : MonoBehaviour {
	public string[] buildingName = {
		"name1", "name2", "name3", "name4", "name5", "name6", "name7", "name8",
		"name9", "name10", "name11"};
	public int[] edificios = {0,0,0,0,0,0,0,0,0,0,0};
	public double clicktotales;
	public float[] precios = {15,100,500,3000,10000,40000,200000,1666666,123456789,3999999999,75000000000};
	float[] preciosoriginales = {15,100,500,3000,10000,40000,200000,1666666,123456789,3999999999,75000000000};
	public float[] galletasporsegundo = {0.1f,0.5f,4,10,40,100,400,6666,98765,999999,10000000};
	float[] galletasporsegundoauxa = {0.1f,0.5f,4,10,40,100,400,6666,98765,999999,10000000};
	public float[] timetobuild = {15,30,60,120,240,480,960,1920,3840,7680,15360};
	public float[] buildingritenow = {0,0,0,0,0,0,0,0,0,0,0};
	public float[] timeremaintobuild = {0,0,0,0,0,0,0,0,0,0,0};
	//public Text[] totalporsegundo;
	public double[] gems;
	float[] upgrades = {1,1,1,1,1,1,1,1,1,1,1};
	TimeConvert tiempo;
	public Text[] Gems_txt;
	public Text coin_txt;
	public Text total_txt;
	public double coins;
	int rando;
	public GameObject[] gotgems;
	float increaser;
	double valperclick = 1;
	public Text click;

	public Sprite[] torres;


	public GameObject[] SoldiersScroll;
	public GameObject BuildingScroll;
	public GameObject UpgradeScroll;
	bool primeravez = false;

	double totalGpS;

	public GameObject panelConScript;

	ScrollableList scrollEdificio;
	ScrollableList scrollUpgrade;
	//ScrollableList[] scrollSoldados;
	List <ScrollableList> scrollSoldados;

	//vars updates
	public struct upgradesDef{
		public double req;
		public double price;
		public string name;
		public string desc;
		public int edificio;
		public int active; //0=inactivo, 1=activo, 2=comprado
	};
	
	public upgradesDef[] upgradeB;

	void Start () {
		scrollSoldados = new List<ScrollableList>();
		scrollEdificio = BuildingScroll.GetComponent<ScrollableList> ();
		scrollUpgrade = UpgradeScroll.GetComponent<ScrollableList> ();
		for (int S=0; S<4; S++) {
			scrollSoldados.Add(SoldiersScroll[S].GetComponent<ScrollableList> ());
		}

		tiempo = this.GetComponent<TimeConvert>();

		upgradeB = new upgradesDef[88];
		string line = "";
		int contador = 0;
		int numUp = 0;
		StreamReader sr = new StreamReader ( Application.dataPath + "/upgrades.txt");
		while (!sr.EndOfStream) {
			for(int t=0;t<5;t++){
				line = sr.ReadLine();
				try{
					switch(contador){
					case 0:
						upgradeB[numUp].edificio = int.Parse(line);
						break;
					case 1:
						upgradeB[numUp].req = double.Parse(line);
						break;
					case 2:
						upgradeB[numUp].price = double.Parse(line);
						break;
					case 3:
						upgradeB[numUp].name = line;
						break;
					case 4:
						upgradeB[numUp].desc = line;
						break;
					}
				}
				catch{
					print ("error en numUp:" + contador + "en upgrade: " + numUp);
				}
				if(contador == 4)
					contador = 0;
				else
					contador++;
			}
			upgradeB[numUp].active = 0;
			numUp++;
		}
		sr.Close();

		for (int gem=0; gem<4; gem++) {
			gems [gem] = 987654321123456789.01;
		}
		
		for (int buil=0; buil<11; buil++) {
			edificios [buil] = 0;
			timeremaintobuild[buil] = 0;
			buildingritenow[buil] = 0;
		}
		clicktotales = 1000000000000000;

		scrollEdificio.MakeBuildingScroll ();
		scrollUpgrade.MakeUpgradeScroll (primeravez);
		primeravez = true;
		for(int S=0;S<4;S++)
			scrollSoldados[S].MakeBuildingScrollSoldier (S);

		for (int i=0; i<precios.Length; i++) {
			updateNumbers(i);
		}
		coins = 50000;

		for (int co=0; co<4; co++) {
			Gems_txt[co].text = numbertotext(gems[co]);
		}

	}

	public string numbertotext(double gem){
		int len ;
		len = (int)gem.ToString("f0").Length;
		if (len < 7) { //999,999
						return (gem.ToString ("f2"));
				} else if (len >= 7 && len <= 9) { //999,999,999 -> 999.99mil
						return ((gem / 1000000).ToString ("f3") + " Millions");
				} else if (len >= 10 && len <= 12) { //999,999,999,999 -> 999.99bil
						return ((gem / 1000000000).ToString ("f3") + " Billions");
				} else if (len >= 13 && len <= 15) { //999,999,999,999,999 -> 999.99tril
						return ((gem / 1000000000000).ToString ("f3") + " Trillions");
				} else if (len >= 16 && len <= 18) { //999,999,999,999,999,999 -> 999.99qua
						return ((gem / 1000000000000000).ToString ("f3") + " Quadrillions");
				} else if (len >= 19 && len <= 21) { //999,999,999,999,999,999,999 -> 999.99quinti
						return ((gem / 1000000000000000000).ToString ("f3") + " Quintillion");
				} else {
						return "";
				}
	}

	void Update () {
		updateCookies ();
		somethingbuildingwa ();
	}
	
	void showUpdates () {
		for (int e=0; e<11; e++) {
			for(int u=0; u<88; u++){
				if(upgradeB[u].edificio == e && upgradeB[u].active == 0){
					if(edificios[e] >= upgradeB[u].req){
						upgradeB[u].active = 1;
					}
				}
			}
		}
	}
	
	void updateNumbers (int i){
		precios[i] = Mathf.Pow(1.15f, edificios [i] + buildingritenow[i]) * preciosoriginales[i];
		scrollEdificio.printstuff(i);
	}
	
	void updateCookies (){
		for(int j=0;j<11;j++){
			if (edificios [j] > 0) {
				for (int i=0; i<4; i++) {
					increaser = (galletasporsegundo [j] * edificios [j] * Time.deltaTime); 
					gems [i] += increaser;
					Gems_txt [i].text = numbertotext(gems [i]);
					//totalearned[0] += increaser;
				}
			}
		}
		coin_txt.text = coins.ToString ("f2");
		totalgalletasporsegundo ();
	}
	
	public void buyUpgrade(int numB){
		if (gems[0] >= upgradeB[numB].price && gems[1] >= upgradeB[numB].price &&
		    gems[2] >= upgradeB[numB].price && gems[3] >= upgradeB[numB].price && upgradeB[numB].active < 2) {
			for(int r=0;r<4;r++)
				gems[r] -= upgradeB[numB].price;
			upgradeB[numB].active = 2;

			if(numB == 0 || numB == 12 || numB == 27 || numB == 33 || numB == 39 || numB == 45 ||
			   numB == 51 || numB == 57 || numB == 63 || numB == 69 || numB == 75)
			{
				float[] extraCpS = {0.1f, 0.3f, 1, 4, 10, 30, 100, 1666, 9876, 99999, 1000000};
				if (upgradeB [numB].edificio == 0) {
					valperclick++;
				}
				galletasporsegundo[upgradeB[numB].edificio] += extraCpS[upgradeB[numB].edificio];
			}
			else if(numB == 1 || numB == 2 || (numB >= 13 && numB <= 26) || (numB >= 28 && numB <= 32) ||
			        (numB >= 34 && numB <= 38) || (numB >= 40 && numB <= 44) || (numB >= 46 && numB <= 50) ||
			        (numB >= 52 && numB <= 56) || (numB >= 58 && numB <= 62) || (numB >= 64 && numB <= 68) ||
			        (numB >= 70 && numB <= 74) || (numB >= 76 && numB <= 80))
			{
				if (upgradeB [numB].edificio == 0) {
					valperclick *= 2;
				}
				galletasporsegundo[upgradeB[numB].edificio] *= 2;
			}
			else if(numB >= 3 && numB <= 11)
			{
				float[] cookiepernonminer = {0.1f, 0.5f, 2, 10, 20, 100, 200, 400, 800};
				int sumatodosedificios = 0;
				for(int tot=1;tot<11;tot++)
					sumatodosedificios += edificios[tot];

				valperclick += (sumatodosedificios*cookiepernonminer[numB-3]);
				galletasporsegundo[0] += (sumatodosedificios*cookiepernonminer[numB-3]);
			}
			else if(numB >= 81 && numB <= 87){
				valperclick += (totalGpS * 0.1f);
			}
			if(numB <=80)
				scrollEdificio.printstuff(upgradeB[numB].edificio);
			scrollUpgrade.muevecuadros();
		}
	}

	void totalgalletasporsegundo(){
		totalGpS = 0;
		for (int total=0; total<11; total++)
			totalGpS += galletasporsegundo [total] * edificios [total];
		total_txt.text = numbertotext(totalGpS);
	}

	public void buyBuilding(int numB){
		if (gems[0] >= precios [numB] && gems[1] >= precios [numB] &&
		    gems[2] >= precios [numB] && gems[3] >= precios [numB]) {

			//edificios[numB] ++;
			buildingritenow [numB] ++;
			timeremaintobuild [numB] += timetobuild [numB];

			for(int r=0;r<4;r++)
				gems[r] -= precios[numB];
			updateNumbers(numB);
			scrollUpgrade.muevecuadros();
		}
	}

	public void somethingbuildingwa (){
		for(int edif=0;edif<11;edif++){
			if (buildingritenow[edif] > 0) {
				timeremaintobuild[edif] -= Time.deltaTime;

				Text[] textosUpgrades = scrollEdificio.newItem[edif].GetComponentsInChildren<Text>();

				if(timeremaintobuild[edif] >= timetobuild[edif]*(buildingritenow[edif]-1)){
					textosUpgrades [6].text = "Time: " + tiempo.convert_time(timeremaintobuild[edif]);
					scrollEdificio.newItem[edif].GetComponentInChildren<BarScript>().actual = timeremaintobuild[edif] -
						(timetobuild[edif] * (buildingritenow[edif]-1));
					scrollEdificio.newItem[edif].GetComponentInChildren<BarScript>().valormax = timetobuild[edif];
					scrollEdificio.newItem[edif].GetComponentInChildren<BarScript>().semueve = true;
				}
				else {
					buildingritenow[edif]--;
					edificios[edif]++;
					scrollEdificio.printstuff(edif);
				}
			}
		}
	}

	public void CookieClick(){
		int rando = Random.Range(0, 4);
		gems [rando] += valperclick;
		clicktotales++;
		//Gems_txt[rando].text = gems[rando].ToString("f2");
		Gems_txt[rando].text = numbertotext(gems[rando]);


		Vector3 posaux = Input.mousePosition;
		posaux.z = 1;
		Text Clicko = Instantiate (click, new Vector3(posaux.x+50, posaux.y+20, posaux.z), Quaternion.identity) as Text;
		Clicko.text = "+" + numbertotext(valperclick);
		switch(rando){
		case 0:
			Clicko.color = new Color (1,0,0,1);
			break;
		case 1:
			Clicko.color = new Color (0,1,0,1);
			break;
		case 2:
			Clicko.color = new Color (0,0,1,1);
			break;
		case 3:
			Clicko.color = new Color (1,1,0,1);
			break;
		}
		Clicko.transform.parent = gameObject.transform;
	}

}
