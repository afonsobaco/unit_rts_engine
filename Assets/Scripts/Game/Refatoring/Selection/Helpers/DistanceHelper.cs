using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{

    public static class DistanceHelper
    {
        public static ISelectable[] Sort(IEnumerable<ISelectable> selection, Vector2 startPoint)
        {
            List<ISelectable> result = new List<ISelectable>(selection);
            Ray ray = Camera.main.ScreenPointToRay(startPoint);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                result.Sort(new DistanceComparer(hit.point));
            }
            return result.ToArray();
        }
    }
}
