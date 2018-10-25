using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Accessibility {
    public class AccListenerFactory {

        private static AccListenerFactory instance = null;
        public static AccListenerFactory Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new AccListenerFactory();
                }
                return instance;
            }
        }

        public List<AccComparer> GetComparers()
        {
            List<AccComparer> result = new List<AccComparer>();
            result.Add(new ColliderChecker());
            result.Add(new MobilityMinimizer());
            return result;
        }

        public List<AccChecker> GetCheckers()
        {
            List<AccChecker> result = new List<AccChecker>();
            result.Add(new MobilityMinimizer());
            result.Add(new VolumeChecker());

            return result;
        }

        public AccListenerFactory()
        {

        }
    }
}
