using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using System;

namespace RTSEngine.Refactoring
{

    public static class SubGroupUtil
    {

        public static List<ISelectable> GetFromSubGroupOnScreen(ISelectable[] mainList, ISelectable selected, Func<ISelectable, bool> IsInsideViewportPoints, EqualityComparerComponent equalityComparer)
        {
            List<ISelectable> result = new List<ISelectable>();
            foreach (var item in GetAllInsideViewportPoints(mainList, selected.Position, IsInsideViewportPoints))
            {
                if (equalityComparer.Equals(selected, item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        private static List<ISelectable> GetAllInsideViewportPoints(ISelectable[] mainList, Vector3 startPosition, Func<ISelectable, bool> IsInsideViewportPoints)
        {
            List<ISelectable> result = new List<ISelectable>();
            foreach (var item in mainList)
            {
                if (IsInsideViewportPoints(item))
                {
                    result.Add(item);
                }
            }
            return DistanceHelper.SortWorldSpace(result, startPosition).ToList();
        }

    }
}