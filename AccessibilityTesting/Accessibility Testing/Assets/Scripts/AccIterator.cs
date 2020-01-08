using UnityEngine;
using System.Collections.Generic;

namespace Accessibility
{
    public class AccIterator
    {
        private static AccIterator instance = null;
        public static AccIterator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccIterator();
                }
                return instance;
            }
        }

        //constructor
        public AccIterator()
        {

        }

        public delegate void CompareReadyEventHandler(object source, CompareEvent evnt);
        public event CompareReadyEventHandler compareReady;

        public delegate void CheckReadyEventHandler(object source, CheckEvent evnt);
        public event CheckReadyEventHandler checkReady;

        public void RegisterCheckEventHandlers(List<AccChecker> checkers)
        {
            foreach (AccChecker checker in checkers)
            {
                checkReady += checker.OnCheckReady;
            }
        }

        public void RegisterCompareEventHandlers(List<AccComparer> comparers)
        {
            foreach (AccComparer comparer in comparers)
            {
                compareReady += comparer.OnCompareReady;
            }
        }

        protected virtual void OnCompareReady(GameObject item1, GameObject item2)
        {
            if(! (compareReady == null))
            {
                compareReady(this, new CompareEvent(item1, item2));
            }
        }

        protected virtual void OnCheckReady(GameObject item)
        {
            if(! (checkReady == null))
            {
                checkReady(this, new CheckEvent(item));
            }
        }
       
        public void Iterate(IEnumerable<GameObject> children)
        {
            bool check = checkReady != null && checkReady.GetInvocationList().Length > 0;
            bool compare = compareReady != null && compareReady.GetInvocationList().Length > 0;
            foreach (GameObject child1 in children)
            {
                if(check)
                {
                    OnCheckReady(child1);
                }
                foreach(GameObject child2 in children)
                {
                    if(compare && child1.GetInstanceID() != child2.GetInstanceID())
                    {
                        OnCompareReady(child1, child2);
                    }
                }
            }
        }
    }

    public class CheckEvent
    {
        public GameObject item;

        public CheckEvent(GameObject i)
        {
            item = i;
        }

    }

    public class CompareEvent
    {
        public GameObject item1;
        public GameObject item2;

        public CompareEvent(GameObject i1, GameObject i2)
        {
            item1 = i1;
            item2 = i2;
        }
    }
}