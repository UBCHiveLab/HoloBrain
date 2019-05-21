
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.UI;

namespace VolumeRendering
{

    [RequireComponent (typeof(MeshRenderer), typeof(MeshFilter))]
    public class VolumeRendering : MonoBehaviour {

        [SerializeField] protected Shader shader;
        protected Material material;

        public GameObject cursor;

        [SerializeField] Color color = Color.white;
        [Range(0f, 1f)] public float threshold = 0.5f;
        [Range(0.5f, 5f)] public float intensity = 1.5f;
        [Range(0f, 1f)] public float sliceXMin = 0.0f, sliceXMax = 1.0f;
        [Range(0f, 1f)] public float sliceYMin = 0.0f, sliceYMax = 1.0f;
        [Range(0f, 1f)] public float sliceZMin = 0.0f, sliceZMax = 1.0f;
        //public Quaternion axis = Quaternion.identity;
        public Slider SliderXmin;
        public Slider SliderXmax;
        public Slider SliderYmin;
        public Slider SliderYmax;
        public Slider SliderZmin;
        public Slider SliderZmax;

        public Texture volume;

        protected virtual void Start () {
            material = new Material(shader);
            GetComponent<MeshFilter>().sharedMesh = Build();
            GetComponent<MeshRenderer>().sharedMaterial = material;
            UpdateMaterial();
            SliderXmin.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
            SliderXmax.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
            SliderYmin.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
            SliderYmax.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
            SliderZmin.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
            SliderZmax.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        }

        public void ValueChangeCheck()
        {
            //Output this to console when Button1 or Button3 is clicked
            Debug.Log("There has been a change of " + SliderXmin.value);
            UpdateMaterial();
        }

        protected void Update () {
        }

        private void UpdateMaterial()
        {
            material.SetTexture("_Volume", volume);
            material.SetColor("_Color", color);
            material.SetFloat("_Threshold", threshold);
            material.SetFloat("_Intensity", intensity);
            material.SetVector("_SliceMin", new Vector3(sliceXMin, sliceYMin, sliceZMin));
            material.SetVector("_SliceMax", new Vector3(sliceXMax, sliceYMax, sliceZMax));
        }

        Mesh Build() {
            var vertices = new Vector3[] {
                new Vector3 (-0.5f, -0.5f, -0.5f),
                new Vector3 ( 0.5f, -0.5f, -0.5f),
                new Vector3 ( 0.5f,  0.5f, -0.5f),
                new Vector3 (-0.5f,  0.5f, -0.5f),
                new Vector3 (-0.5f,  0.5f,  0.5f),
                new Vector3 ( 0.5f,  0.5f,  0.5f),
                new Vector3 ( 0.5f, -0.5f,  0.5f),
                new Vector3 (-0.5f, -0.5f,  0.5f),
            };
            var triangles = new int[] {
                0, 2, 1,
                0, 3, 2,
                2, 3, 4,
                2, 4, 5,
                1, 2, 5,
                1, 5, 6,
                0, 7, 4,
                0, 4, 3,
                5, 4, 7,
                5, 7, 6,
                0, 6, 7,
                0, 1, 6
            };

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.hideFlags = HideFlags.HideAndDontSave;
            return mesh;
        }

        void OnValidate()
        {
            Constrain(ref sliceXMin, ref sliceXMax);
            Constrain(ref sliceYMin, ref sliceYMax);
            Constrain(ref sliceZMin, ref sliceZMax);
        }

        void Constrain (ref float min, ref float max)
        {
            const float threshold = 0.025f;
            if(min > max - threshold)
            {
                min = max - threshold;
            } else if(max < min + threshold)
            {
                max = min + threshold;
            }
        }

        /* Disables the cursor when focused on the volume
        void OnStartGaze()
        {
            foreach (Renderer renderer in cursor.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
        }

        void OnEndGaze()
        {
            foreach (Renderer renderer in cursor.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = true;
            }
        }
        */

        void OnDestroy()
        {
            Destroy(material);
        }

    }

}


