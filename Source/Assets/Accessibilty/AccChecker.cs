namespace Accessibility
{
    using UnityEngine;
    public interface AccChecker
    {
        bool Check(GameObject item);
        void OnCheckReady(object source, CheckEvent evnt);
    }
}