using UnityEngine;
using System.Collections;
using UnityEditor;

public class TagSearcher : EditorWindow {

	static TagSearcher window;
	static string tagValue = "";
	static string oldTagValue;
	static Vector2 scrollValue = Vector2.zero;
	static GameObject[] searchResult;

	[MenuItem("Avi's Utility Tools/TagSearcher")]
	static void OpenTagSearcher() {


		window = (TagSearcher)EditorWindow.GetWindow(typeof(TagSearcher));
		searchResult = GameObject.FindGameObjectsWithTag(tagValue);


	}

	void OnGUI() {


		oldTagValue = tagValue;

		tagValue = EditorGUILayout.TagField(tagValue);


		if (tagValue != oldTagValue) {


			searchResult = GameObject.FindGameObjectsWithTag(tagValue);
			Selection.objects = searchResult;

		}

		scrollValue = EditorGUILayout.BeginScrollView(scrollValue);

		if (searchResult != null) {

			foreach (GameObject obj in searchResult) {
				if (obj != null) {
					if (GUILayout.Button(obj.name))//,GUIStyle.none))
					{
						Selection.activeObject = obj;
						EditorGUIUtility.PingObject(obj);
					}
				} else {
					searchResult = GameObject.FindGameObjectsWithTag(tagValue);
					Selection.objects = searchResult;
					break;
				}
			}

		}

		EditorGUILayout.EndScrollView();
	}

}