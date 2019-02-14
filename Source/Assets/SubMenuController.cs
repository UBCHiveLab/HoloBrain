using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenuController : MonoBehaviour {

    public GameObject SubMenu;

    public void OnSelect()
    {
        if (SubMenu != null)
        {
            SubMenu.SetActive(!SubMenu.activeSelf);
        }
    }
}
