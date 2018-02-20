using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourLoader : MonoBehaviour {

    [System.Serializable]
    public struct namedMaterial
    {
        public string name;
        public Material mat;
    }

    public namedMaterial[] materials;

	// Use this for initialization
	void Start () {

        foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
        {
            GameObject parentObject = rend.gameObject.transform.parent.gameObject;
            string niftiName;

            if (parentObject.name == "default")
            {
                GameObject parentNifti = parentObject.transform.parent.gameObject;
                niftiName = parentNifti.name;
            }
            else
            {
                niftiName = parentObject.name;
            }

            foreach (namedMaterial mat in materials)
            {
                if (niftiName.Contains(mat.name))
                {
                    rend.material = mat.mat;
                }
            }
        }

        for (int i = 1; i < transform.childCount; i++)
        {

            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(false);
        }




    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
