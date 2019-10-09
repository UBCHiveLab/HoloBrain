using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppConfiguration : MonoBehaviour {

    public string appConfigWord;

    private List<string> activeHolograms;
    private List<string> activeMenuItems;
    
	// Use this for initialization
	void Start () {
        activeHolograms = new List<string>();
        activeMenuItems = new List<string>();

        PlayerPrefs.SetString("appConfig", appConfigWord);

        TextAsset holograms = Resources.Load<TextAsset>("Configs/" + appConfigWord + "Holograms");
        string hologramsRaw = holograms.text;
        string[] hologramList = hologramsRaw.Split(',');
        foreach(string s in hologramList)
        {
            activeHolograms.Add(s);
        }

        TextAsset menuItems = Resources.Load<TextAsset>("Configs/" + appConfigWord + "Menu");
        string menuItemsRaw = menuItems.text;
        string[] menuItemList = menuItemsRaw.Split(',');
        foreach(string s in menuItemList)
        {
            activeMenuItems.Add(s);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<string> GetActiveHolograms()
    {
        return activeHolograms;
    }

    public List<string> GetActiveMenuItems()
    {
        return activeMenuItems;
    }
}
