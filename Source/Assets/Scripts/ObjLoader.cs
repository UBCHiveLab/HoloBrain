using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class ObjLoader : MonoBehaviour {

    public string[] folderNames = { "Niftis", "DeltaNiftis" };
    public Transform[] parentTransforms;

	// Use this for initialization
	void Start () {

        for (int i = 0; i < folderNames.Length; i++)
        {
            string folderName = folderNames[i];
            Transform parentTransform = parentTransforms[i];

            DirectoryInfo dir = new DirectoryInfo("Assets/Resources/" + folderName);

            FileInfo[] info = dir.GetFiles();
            Debug.Log(info.Length.ToString());
            string[] fileNames;
            fileNames = info.Select(f => f.Name).ToArray();


            foreach (string name in fileNames)
            {
                string objName = name.Split('.')[0];

                GameObject loadedObject = (GameObject)Instantiate(Resources.Load(folderName + "/" + objName));
                loadedObject.transform.localScale = new Vector3(0.004f, 0.004f, 0.004f);
                loadedObject.transform.position = parentTransform.position;
                loadedObject.transform.Rotate(new Vector3(0f, -100f, 0f));

                string[] nameParts = objName.Split('_');
                string niftiNumber = nameParts[nameParts.Length - 2];

                if (parentTransform.Find(niftiNumber) == null)
                {
                    GameObject emptyObject = Instantiate<GameObject>(new GameObject());
                    emptyObject.name = niftiNumber;
                    emptyObject.transform.parent = parentTransform;
                    emptyObject.transform.position = parentTransform.position;
                    //emptyObject.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
                    loadedObject.transform.parent = emptyObject.transform;
                }
                else
                {
                    Transform niftiParentTransform = parentTransform.Find(niftiNumber);
                    loadedObject.transform.parent = niftiParentTransform.transform;
                }


            }

            ColourLoader colourLoader = parentTransform.gameObject.GetComponent<ColourLoader>();
            colourLoader.Invoke("Start", 0f);
        }


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
