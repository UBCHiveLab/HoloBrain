using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainContainer : MonoBehaviour
{
    Dictionary<Transform, PosRotSca> initialPositions = new Dictionary<Transform, PosRotSca>();
    public void Expand()
    {
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        this.transform.localScale = new Vector3(1, 1, 1);
        Animator animator = gameObject.GetComponent<Animator>();
        animator.enabled = true;
        animator.SetBool("Expanded", true);
    }

    public void Collapse()
    {
        RestoreLocalPosition();
        Animator animator = gameObject.GetComponent<Animator>();
        animator.enabled = true;
        animator.SetBool("Expanded", false);
    }

    private void SaveLocalPosition()
    {
        foreach( GameObject structure in GameObject.FindGameObjectsWithTag("Structure"))
        {
            PosRotSca posrotscal;
            posrotscal.initialStructurePosition = structure.transform.localPosition;
            posrotscal.initialStructureRotation = structure.transform.localRotation;
            posrotscal.initialStructureScale = structure.transform.localScale;
            structure.GetComponent<BoxCollider>().enabled = true;
            initialPositions[structure.transform] = posrotscal;
        }

    }

    public void RestoreLocalPosition()
    {
        foreach (GameObject structure in GameObject.FindGameObjectsWithTag("Structure"))
        {
            structure.GetComponent<BoxCollider>().enabled = false;
        }
        foreach ( var pair in initialPositions)
        {
            Transform t = pair.Key;
            t.localPosition = pair.Value.initialStructurePosition;
            t.localRotation = pair.Value.initialStructureRotation;
            t.localScale = pair.Value.initialStructureScale;
        }

    }

    struct PosRotSca
    {
        public Vector3 initialStructurePosition;
        public Quaternion initialStructureRotation;
        public Vector3 initialStructureScale;
    }
}
