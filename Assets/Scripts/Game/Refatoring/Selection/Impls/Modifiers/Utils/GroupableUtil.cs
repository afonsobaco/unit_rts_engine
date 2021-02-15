using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using System;

namespace RTSEngine.Refactoring
{

    public static class GroupableUtil
    {

        public static List<ISelectable> GetFromSameTypeOnScreen(ISelectable[] groupables, ISelectable selected, Func<ISelectable, bool> IsInsideViewportPoints)
        {
            List<ISelectable> result = new List<ISelectable>();
            foreach (var item in GetAllInsideViewportPoints(groupables, selected.Position, IsInsideViewportPoints))
            {
                if (item is IGroupable)
                {
                    var obj = item as IGroupable;
                    if (obj.IsCompatible(selected))
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        private static List<ISelectable> GetAllInsideViewportPoints(ISelectable[] groupables, Vector3 startPosition, Func<ISelectable, bool> IsInsideViewportPoints)
        {
            List<ISelectable> result = new List<ISelectable>();
            foreach (var item in groupables)
            {
                if (item is IGroupable && IsInsideViewportPoints(item))
                {
                    result.Add(item);
                }
            }
            return DistanceHelper.SortWorldSpace(result, startPosition).ToList();
        }

    }
}