using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIConvexHull;
using DbscanImplementation;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class RenderNifti : MonoBehaviour {

    public string filename;
    public GameObject cube;
    float threshold = -8f;
    public float epsilon = 1f;
    private List<Vector3> points;

    public class ColourBucket
    {
        public float upperBound;
        public float lowerBound;
        public Color bucketColour;

        public ColourBucket(float upper, float lower, Color colour)
        {
            upperBound = upper;
            lowerBound = lower;
            bucketColour = colour;
        }
    }

    public ColourBucket[] buckets = {new ColourBucket(-7f, -8f, Color.blue), new ColourBucket(-5f, -7f, Color.green), new ColourBucket(-0.25f, -0.5f,new Color(1f,0f,0f,0.5f)), new ColourBucket(-3f, -5f, Color.cyan), new ColourBucket(10f, 5f, Color.yellow)};

	// Use this for initialization
	void Start () {
        points = new List<Vector3>();
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
        System.Random rand = new System.Random();


        //if (intensity >7)
        foreach (ColourBucket bucket in buckets)
        {
            points = new List<Vector3>();
            foreach (string line in lines)
            {
                string[] coords = line.Split('\t');
                float intensity = System.Convert.ToSingle(coords[3]);

                if (intensity >= bucket.lowerBound && intensity < bucket.upperBound)
                {
                    float x = System.Convert.ToSingle(coords[0]);
                    float y = System.Convert.ToSingle(coords[1]);
                    float z = System.Convert.ToSingle(coords[2]);

                    // We are doing this offset so that points dont fall on the same plane
                    x += 0.001f * ((float)rand.NextDouble());
                    y += 0.001f * ((float)rand.NextDouble());
                    z += 0.001f * ((float)rand.NextDouble());



                    Vector3 pos = new Vector3(x, y, z);
                    points.Add(pos);
                    //Instantiate(cube, pos, Quaternion.identity, transform);
                }
            }


            //Debug.Log("# of points added: " + points.Count.ToString());
            HashSet<ClusterPoint[]> clusters;

            ClusterPoint[] clusterPointData = points.Select(x => new ClusterPoint(x)).ToArray();
            var dbscan = new DbscanAlgorithm<ClusterPoint>((x, y) => Vector3.Distance(x.vector, y.vector));

            dbscan.ComputeClusterDbscan(allPoints: clusterPointData, epsilon: epsilon, minPts: 4, clusters: out clusters);

            foreach (ClusterPoint[] cluster in clusters)
            {
                List<Vector3> clusterPoints = cluster.Select(x => x.vector).ToList();


                GameObject clusterGameObject;

                clusterGameObject = Instantiate(new GameObject(), gameObject.transform.position, Quaternion.identity);
                clusterGameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                clusterGameObject.transform.Rotate(90, 0, 0);
                //clusterGameObject.transform.parent = gameObject.transform;

                if (clusterPoints.Count >= 4)
                {
                    //Debug.Log("# of points in cluster: " + clusterPoints.Count.ToString());

                    /*
                    foreach(Vector3 pt in clusterPoints)
                    {
                        Debug.Log("coordinates of the point: " + pt.x.ToString() + " " + pt.y.ToString() + " " + pt.z.ToString());
                    }
                    */

                    Mesh mesh = CreateMesh(clusterPoints);

                    MeshFilter meshFilter = (MeshFilter)clusterGameObject.AddComponent(typeof(MeshFilter));

                    meshFilter.mesh = mesh;

                    MeshRenderer renderer = clusterGameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

                    renderer.material = new Material(Shader.Find("Specular"));
                    renderer.material.color = bucket.bucketColour;
                    //renderer.material.color.a = 0.5f;

                }
            }
        }

    }



    Mesh CreateMesh(IEnumerable<Vector3> stars)
    {
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        List<int> triangles = new List<int>();

        var vertices = stars.Select(x => new Vertex(x)).ToList();

        var result = MIConvexHull.ConvexHull.Create(vertices);

        Debug.Log("# of points in mesh: " + result.Points.ToArray().Length.ToString());
        m.vertices = result.Points.Select(x => x.ToVec()).ToArray();
        var xxx = result.Points.ToList();

        foreach (var face in result.Faces)
        {
            triangles.Add(xxx.IndexOf(face.Vertices[0]));
            triangles.Add(xxx.IndexOf(face.Vertices[1]));
            triangles.Add(xxx.IndexOf(face.Vertices[2]));
        }

        m.triangles = triangles.ToArray();
        m.RecalculateNormals();
        return m;
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
