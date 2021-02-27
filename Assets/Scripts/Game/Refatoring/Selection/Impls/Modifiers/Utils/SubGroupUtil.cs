using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using System;

namespace RTSEngine.Refactoring
{

    public static class SubGroupUtil
    {
        public static ISelectable[] GetFromSubGroupOnScreen(ISelectable[] allOnScreen, ISelectable selected, EqualityComparerComponent equalityComparer)
        {
            List<ISelectable> result = new List<ISelectable>();
            foreach (var item in allOnScreen)
            {
                if (equalityComparer.Equals(selected, item))
                {
                    result.Add(item);
                }
            }
            return result.ToArray();
        }
    }
}