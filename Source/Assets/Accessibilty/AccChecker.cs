namespace Accessibility
{
    using UnityEngine;
    public abstract class AccChecker
    {
        public abstract bool check(GameObject item);
        public abstract void OnCheckReady(object source, CheckEvent evnt);
    }
}