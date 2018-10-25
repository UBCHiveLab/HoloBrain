using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Accessibility;

public class AccessibilityManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Debug.Log("starting iteration");
        AccIterator iterator = AccIterator.Instance;
        AccListenerFactory listenerFactory = AccListenerFactory.Instance;
        Transform[] transforms = GetComponentsInChildren<Transform>();
        GameObject[] children = transforms.Select(e => e.gameObject).ToArray();

        iterator.RegisterCompareEventHandlers(listenerFactory.GetComparers());
        iterator.RegisterCheckEventHandlers(listenerFactory.GetCheckers());
        iterator.Iterate(children);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
