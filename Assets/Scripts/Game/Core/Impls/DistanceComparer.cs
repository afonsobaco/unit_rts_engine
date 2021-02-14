using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Core
{
    public class DistanceComparer : IComparer<ISelectable>
    {
        private Vector3 initialPosittion;

        public DistanceComparer(Vector3 initialPosittion)
        {
            this.initialPosittion = initialPosittion;
        }

        public int Compare(ISelectable v1, ISelectable v2)
        {
            return (v1.Position - initialPosittion).sqrMagnitude.CompareTo((v2.Position - initialPosittion).sqrMagnitude);
        }
    }
}