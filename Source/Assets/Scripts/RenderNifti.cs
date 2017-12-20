using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class RenderNifti : MonoBehaviour {

    public string filename;
    public GameObject cube;
    float threshold = -8f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q))
        {
            Render();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            CombineMeshes();
        }
	}

    void Render()
    {
        TextAsset file = Resources.Load(filename) as TextAsset;
        string[] lines = file.text.Split('\n');
        foreach (string line in lines)
        {
            string[] coords = line.Split('\t');
            float intensity = System.Convert.ToSingle(coords[3]);
            if (intensity < threshold)
            {
                float x = System.Convert.ToSingle(coords[0]);
                float y = System.Convert.ToSingle(coords[1]);
                float z = System.Convert.ToSingle(coords[2]);

                Vector3 pos = new Vector3(x, y, z);
                Instantiate(cube, pos, Quaternion.identity, transform);
            }
        }
    }

    void CombineMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int index = 0;
        for (var i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].sharedMesh == null) continue;
            combine[index].mesh = meshFilters[i].sharedMesh;
            combine[index++].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].GetComponent<Renderer>().enabled = false;
        }
        print(index);
        GetComponent<MeshFilter>().mesh = new Mesh();
        GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        GetComponent<Renderer>().material = meshFilters[1].GetComponent<Renderer>().sharedMaterial;
        GetComponent<Renderer>().enabled = true;
    }
}
