﻿using System; using System.Collections; using System.Collections.Generic; using System.Linq; using UnityEngine; using UnityEngine.UI;   public class TextColorChanger : MonoBehaviour {      public Text text;     bool changedFlag = false;     private List<string> textString = new List<string>();     private const string COLORHEAD = "<color=#00ffffff>";     private const string COLORCLOSE = "</color>";      // Use this for initialization     void Start () {         textString = text.text.Split(' ').ToList();         if (text.IsActive()) StartCoroutine(AnimateText());     }      IEnumerator AnimateText()     {         int count = 0;         int loc = -1;         while (count < textString.Count)         {             yield return new WaitForSeconds(0.5f);             if (loc != -1)             {                 textString[loc] = textString[loc].Replace(COLORCLOSE, "");                 textString[loc + 1] = textString[loc + 1] + COLORCLOSE;             }             else             {                 textString[0] = COLORHEAD + textString[0];                 textString[0] = textString[0] + COLORCLOSE;             }              changedFlag = true;             loc++;             count++;         }              } 	 	// Update is called once per frame 	void Update () {         if (changedFlag)         {             text.text = String.Join(" ", textString.ToArray());             changedFlag = false;         }     }      void OnDisable()     {         text.text = text.text.Replace(COLORHEAD, "").Replace(COLORCLOSE, "");     }      void OnEnable()     {         StartCoroutine(AnimateText());     }  } 