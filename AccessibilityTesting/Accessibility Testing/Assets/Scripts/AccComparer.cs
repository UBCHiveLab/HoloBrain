namespace Accessibility
{
    using UnityEngine;

    public interface AccComparer
    {
        bool Compare(GameObject item1, GameObject item2);
        void OnCompareReady(object source, CompareEvent evnt);
    }
}
