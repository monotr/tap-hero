using UnityEngine;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine.UI;

public class ScrollableList : MonoBehaviour
{
	public GameObject itemPrefab;
	public int itemCount = 10, columnCount = 1;
	public GameObject scriptdata;
	Text[] textosUpgrades;
	public GameObject[] newItem;
	public GameObject[] newItemU;
	public GameObject[] newItemS;
	CookieScript1 datos;
	TimeConvert tiempo;
	float[] minymaxrec;

	[SerializeField] Button[] comprar;
	[SerializeField] Button[] comprarU;
	[SerializeField] Button[] comprarS;

	public Image[] soldiericon;

	void Start(){
		minymaxrec = new float[88*4];
	}

	public void MakeBuildingScroll()
	{
		RectTransform rowRectTransform = itemPrefab.GetComponent<RectTransform>();
		RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();
		
		//calculate the width and height of each child item.
		float width = containerRectTransform.rect.width / columnCount;
		float ratio = width / rowRectTransform.rect.width;
		float height = rowRectTransform.rect.height * ratio;
		int rowCount = itemCount / columnCount;
		if (itemCount % rowCount > 0)
			rowCount++;
		
		//adjust the height of the container so that it will just barely fit all its children
		float scrollHeight = height * rowCount;
		containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
		containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);
		
		int j = 0;
		for (int i = 0; i < itemCount; i++)
		{
			//this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
			if (i % columnCount == 0)
				j++;
			
			//create a new item, name it, and set the parent
			newItem[i] = Instantiate(itemPrefab) as GameObject;
			newItem[i].name = gameObject.name + " item at (" + i + "," + j + ")";
			newItem[i].transform.SetParent(gameObject.transform);

			datos = scriptdata.GetComponent<CookieScript1> ();
			tiempo = scriptdata.GetComponent<TimeConvert>();
			printstuff(i);

			comprar[i] = newItem[i].GetComponentInChildren<Button>();
			comprar[i].onClick.RemoveAllListeners();
			int index = i;
			comprar[i].onClick.AddListener (()=> {compracasa(index);});

			//move and size the new item
			RectTransform rectTransform = newItem[i].GetComponent<RectTransform>();
			
			float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
			float y = containerRectTransform.rect.height / 2 - height * j;
			rectTransform.offsetMin = new Vector2(x, y);
			
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + height;
			rectTransform.offsetMax = new Vector2(x, y);
		}
		
		
	}

	public void MakeUpgradeScroll(bool firsttime)
	{
		RectTransform rowRectTransform = itemPrefab.GetComponent<RectTransform>();
		RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();
		
		//calculate the width and height of each child item.
		float width = containerRectTransform.rect.width / columnCount;
		float ratio = width / rowRectTransform.rect.width;
		float height = rowRectTransform.rect.height * ratio;

		print ("height: " + height);

		int rowCount = itemCount / columnCount;
		if (itemCount % rowCount > 0)
			rowCount++;
		
		//adjust the height of the container so that it will just barely fit all its children
		float scrollHeight = height * rowCount;
		containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
		containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);
		
		int j = 0;
		for (int i = 0; i < itemCount; i++)
		{
			//this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
			if (i % columnCount == 0)
				j++;

			if(!firsttime){
				//create a new item, name it, and set the parent
				newItemU[i] = Instantiate(itemPrefab) as GameObject;
				newItemU[i].name = gameObject.name + " item at (" + i + "," + j + ")";
				newItem[i].transform.SetParent(gameObject.transform);

				datos = scriptdata.GetComponent<CookieScript1> ();
				printstuff2(i);
				
				comprarU[i] = newItemU[i].GetComponentInChildren<Button>();
				comprarU[i].onClick.RemoveAllListeners();
				int index = i;
				comprarU[i].onClick.AddListener (()=> {compraupdate(index);});
			}

			float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
			float y = containerRectTransform.rect.height / 2 - height * j;
			minymaxrec[i*4] = x;
			minymaxrec[(i*4)+1] = y;

			x = x + width;
			y = y + height;
			minymaxrec[(i*4)+2] = x;
			minymaxrec[(i*4)+3] = y;

		}

		muevecuadros ();
		
	}

	public void muevecuadros(){
		int menos = 0;
		for (int u=0; u<88; u++) {
			RectTransform rectTransform2 = newItemU[u].GetComponent<RectTransform>();
			if(datos.upgradeB[u].active != 2 && datos.upgradeB[u].edificio != 11){
				if(datos.upgradeB[u].req <= datos.edificios[datos.upgradeB[u].edificio]){
					rectTransform2.offsetMin = new Vector2(minymaxrec[(u*4)-(menos*4)], minymaxrec[(u*4)+1-(menos*4)]);
					rectTransform2.offsetMax = new Vector2(minymaxrec[(u*4)+2-(menos*4)], minymaxrec[(u*4)+3-(menos*4)]);
				}
				else{
					rectTransform2.offsetMin = new Vector2(-500,500);
					rectTransform2.offsetMax = new Vector2(-500,500);
					menos++;
				}
			}
			else if(datos.upgradeB[u].active != 2 && datos.upgradeB[u].edificio == 11){
				if(datos.upgradeB[u].req <= datos.clicktotales){
					rectTransform2.offsetMin = new Vector2(minymaxrec[(u*4)-(menos*4)], minymaxrec[(u*4)+1-(menos*4)]);
					rectTransform2.offsetMax = new Vector2(minymaxrec[(u*4)+2-(menos*4)], minymaxrec[(u*4)+3-(menos*4)]);
				}
				else{
					rectTransform2.offsetMin = new Vector2(-500,500);
					rectTransform2.offsetMax = new Vector2(-500,500);
					menos++;
				}
			}
			else{
				rectTransform2.offsetMin = new Vector2(-500,500);
				rectTransform2.offsetMax = new Vector2(-500,500);
				menos++;
			}
		}
	}

	public void MakeBuildingScrollSoldier(int color)
	{
		RectTransform rowRectTransform = itemPrefab.GetComponent<RectTransform>();
		RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();
		
		//calculate the width and height of each child item.
		float width = containerRectTransform.rect.width / columnCount;
		float ratio = width / rowRectTransform.rect.width;
		float height = rowRectTransform.rect.height * ratio;
		int rowCount = itemCount / columnCount;
		if (itemCount % rowCount > 0)
			rowCount++;
		
		//adjust the height of the container so that it will just barely fit all its children
		float scrollHeight = height * rowCount;
		containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
		containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);
		
		int j = 0;
		for (int i = 0; i < itemCount; i++)
		{
			//this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
			if (i % columnCount == 0)
				j++;
			
			//create a new item, name it, and set the parent
			newItemS[i] = Instantiate(itemPrefab) as GameObject;
			newItemS[i].name = gameObject.name + " item at (" + i + "," + j + ")";
			newItem[i].transform.SetParent(gameObject.transform);

			datos = scriptdata.GetComponent<CookieScript1> ();
			textosUpgrades = newItemS[i].GetComponentsInChildren<Text>();
			textosUpgrades [4].text = "LV 999 ATK 6969";
			textosUpgrades[5].text = "123,456 ATK+789";

			soldiericon = newItemS[i].GetComponentsInChildren<Image>();
			switch(color)
			{
			case 0:
				soldiericon[0].color = new Color(1,0,0,0.5f); //fondo panel
				break;
			case 1:
				soldiericon[0].color = new Color(0,1,0,0.5f); //fondo panel
				break;
			case 2:
				soldiericon[0].color = new Color(0,0,1,0.5f); //fondo panel
				break;
			case 3:
				soldiericon[0].color = new Color(1,1,0,0.5f); //fondo panel
				break;
			}
			soldiericon[1].sprite = datos.torres[color];	//imagen

			comprarS[i] = newItemS[i].GetComponentInChildren<Button>();
			comprarS[i].onClick.RemoveAllListeners();
			int index = i;
			comprarS[i].onClick.AddListener (()=> {compracasa(index);});
			
			//move and size the new item
			RectTransform rectTransform = newItemS[i].GetComponent<RectTransform>();
			
			float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
			float y = containerRectTransform.rect.height / 2 - height * j;
			rectTransform.offsetMin = new Vector2(x, y);
			
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + height;
			rectTransform.offsetMax = new Vector2(x, y);
		}
	}



	void compracasa (int vienede){
		datos.panelConScript.GetComponent<CookieScript1>().buyBuilding (vienede);
	}

	void compraupdate (int vienede){
		datos.panelConScript.GetComponent<CookieScript1>().buyUpgrade (vienede);
	}

	public void printstuff (int i){
		textosUpgrades = newItem[i].GetComponentsInChildren<Text>();
		textosUpgrades[0].text = "Owned: " + datos.edificios[i].ToString("f0");
		textosUpgrades[1].text = datos.numbertotext(datos.precios[i]);
		textosUpgrades[2].text = datos.buildingName[i];
		textosUpgrades[3].text = "Each: " + datos.numbertotext(datos.galletasporsegundo[i]) + " GpS";
		textosUpgrades [5].text = "Building: " + datos.buildingritenow[i].ToString();
		textosUpgrades [6].text = "Time: " + tiempo.convert_time(datos.timeremaintobuild[i]);
		textosUpgrades [7].text = "Total: " + datos.numbertotext(datos.edificios[i]*datos.galletasporsegundo[i]);
	}

	public void print_building_inprogress (int i){
		textosUpgrades [5].text = "Building: " + datos.buildingritenow[i].ToString();
		textosUpgrades [6].text = "Time: " + datos.timeremaintobuild[i].ToString();
	}

	public void printstuff2 (int i){
		textosUpgrades = newItemU[i].GetComponentsInChildren<Text>();
		textosUpgrades [0].text = datos.upgradeB [i].name;
		textosUpgrades[1].text = datos.numbertotext(datos.upgradeB [i].price);
		textosUpgrades[2].text = datos.upgradeB [i].desc;
	}

}
