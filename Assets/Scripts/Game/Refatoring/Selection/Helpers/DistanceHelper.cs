using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{

    public static class DistanceHelper
    {
        public static ISelectable[] SortScreenSpace(IEnumerable<ISelectable> selection, Vector3 startPoint)
        {
            List<ISelectable> result = new List<ISelectable>(selection);
            result.Sort(new DistanceComparerScreen(startPoint));
            return result.ToArray();
        }

        public static ISelectable[] SortWorldSpace(IEnumerable<ISelectable> selection, Vector3 startPoint)
        {
            List<ISelectable> result = new List<ISelectable>(selection);
            result.Sort(new DistanceComparerWorld(startPoint));
            return result.ToArray();
        }
    }
}
