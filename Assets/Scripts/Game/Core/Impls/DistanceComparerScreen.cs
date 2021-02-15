using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Core
{
    public class DistanceComparerScreen : IComparer<ISelectable>
    {
        private Vector3 initialPosittion;

        public DistanceComparerScreen(Vector3 initialPosittion)
        {
            this.initialPosittion = initialPosittion;
        }

        public int Compare(ISelectable v1, ISelectable v2)
        {
            var first = Camera.main.WorldToScreenPoint(v1.Position);
            var second = Camera.main.WorldToScreenPoint(v2.Position);

            return (first - initialPosittion).sqrMagnitude.CompareTo((second - initialPosittion).sqrMagnitude);
        }
    }
}