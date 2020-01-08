namespace Accessibility
{
    using UnityEngine;

    public abstract class AccComparer
    {
        public abstract bool compare(GameObject item1, GameObject item2);
        public abstract void OnCompareReady(object source, CompareEvent evnt);
    }
}
