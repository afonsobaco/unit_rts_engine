using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Core
{
    public class DistanceComparerWorld : IComparer<ISelectable>
    {
        private Vector3 initialPosition;

        public DistanceComparerWorld(Vector3 initialPosition)
        {
            this.initialPosition = initialPosition;
        }

        public int Compare(ISelectable v1, ISelectable v2)
        {
            return (v1.Position - initialPosition).sqrMagnitude.CompareTo((v2.Position - initialPosition).sqrMagnitude);
        }
    }
}