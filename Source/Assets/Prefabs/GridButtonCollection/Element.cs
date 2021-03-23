//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HoloBrain.IsolateMode
{
    public class Element : MonoBehaviour
    {
        public static Element ActiveElement;

        public GameObject MainBrain;
        public TextMesh ElementName;
        public TextMesh ElementNameDetail;


        public Renderer BoxRenderer;
        public MeshRenderer[] PanelSides;
        public MeshRenderer PanelFront;
        public MeshRenderer PanelBack;
        public MeshRenderer[] InfoPanels;

        [HideInInspector]
        public ElementData data;

        private BoxCollider boxCollider;
        private Material highlightMaterial;
        private Material dimMaterial;
        private Material clearMaterial;
        private PresentToPlayer present;

        public void SetActiveElement()
        {
            Element element = gameObject.GetComponent<Element>();
            ActiveElement = element;
        }

        public void ResetActiveElement()
        {
            ActiveElement = null;
        }

        public void Start()
        {
            // Turn off our animator until it's needed
            GetComponent<Animator>().enabled = false;
            BoxRenderer.enabled = true;
            present = GetComponent<PresentToPlayer>();
            MainBrain = GameObject.Find("Brain");
        }

        public void Open()
        {
            if (present.Presenting)
                return;

            StartCoroutine(UpdateActive());
        }

        public void Highlight()
        {
            if (ActiveElement == this)
                return;

            for (int i = 0; i < PanelSides.Length; i++)
            {
                PanelSides[i].sharedMaterial = highlightMaterial;
            }
            PanelBack.sharedMaterial = highlightMaterial;
            PanelFront.sharedMaterial = highlightMaterial;
            BoxRenderer.sharedMaterial = highlightMaterial;
        }

        public void Dim()
        {
            if (ActiveElement == this)
                return;

            for (int i = 0; i < PanelSides.Length; i++)
            {
                PanelSides[i].sharedMaterial = dimMaterial;
            }
            PanelBack.sharedMaterial = dimMaterial;
            PanelFront.sharedMaterial = dimMaterial;
            BoxRenderer.sharedMaterial = dimMaterial;
        }

        private void EnableStructures(ElementData data)
        {
            switch (data.structure)
            {
                case "Basal":
                    transform.Find("BrainContainer/BrainStructures/Basal").gameObject.SetActive(true);
                    break;
                case "Limbic":
                    transform.Find("BrainContainer/BrainStructures/Limbic").gameObject.SetActive(true);
                    break;
                case "Vessel":
                    transform.Find("BrainContainer/BrainStructures/Vessel").gameObject.SetActive(true);
                    break;
                case "Cerebellum":
                    transform.Find("BrainContainer/BrainStructures/Cerebellum").gameObject.SetActive(true);
                    break;
                case "DTI":
                    transform.Find("BrainContainer/BrainStructures/DTI").gameObject.SetActive(true);
                    break;
                default:
                    print("Please select a structure in JSON");
                    break;
            }
        }    

        public IEnumerator UpdateActive()
        {
            present.Present();

            while (!present.InPosition)
            {
                // Wait for the item to be in presentation distance before animating
                yield return null;
            }

            // Start the animation
            Animator animator = gameObject.GetComponent<Animator>();
            animator.enabled = true;
            animator.SetBool("Opened", true);
            MainBrain.SetActive(false);

            //Color elementNameColor = ElementName.GetComponent<MeshRenderer>().material.color;

            while (Element.ActiveElement == this)
            {
                //ElementName.GetComponent<MeshRenderer>().material.color = elementNameColor;
                // Wait for the player to send it back
                yield return null;
            }

            animator.SetBool("Opened", false);

            yield return new WaitForSeconds(0.66f); // TODO get rid of magic number   
            

            // Return the item to its original position
            present.Return();

            Dim();

            MainBrain.SetActive(true);
        }


        /**
         * Set the display data for this element based on the given parsed JSON data
         */
        public void SetFromElementData(ElementData data, Dictionary<string, Material> typeMaterials)
        {
            this.data = data;
            ElementName.text = data.name;

            // Set up our materials
            if (!typeMaterials.TryGetValue(data.category.Trim(), out dimMaterial))
            {
                Debug.Log("Couldn't find " + data.category.Trim() + " in element " + data.name);
            }

            // Create a new highlight material and add it to the dictionary so other can use it
            string highlightKey = data.category.Trim() + " highlight";
            if (!typeMaterials.TryGetValue(highlightKey, out highlightMaterial))
            {
                highlightMaterial = new Material(dimMaterial);
                highlightMaterial.color = highlightMaterial.color * 1.5f;
                typeMaterials.Add(highlightKey, highlightMaterial);
            }

            EnableStructures(data);
            Dim();


            foreach (Renderer infoPanel in InfoPanels)
            {
                // Copy the color of the element over to the info panels so they match
                infoPanel.material.color = dimMaterial.color;
            }

            BoxRenderer.enabled = false;

            // Set our name so the container can alphabetize
            transform.parent.name = data.name;
        }
    }
}
