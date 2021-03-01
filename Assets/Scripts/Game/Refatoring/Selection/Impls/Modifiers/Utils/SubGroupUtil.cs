using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Commons;
using System;

namespace RTSEngine.Refactoring
{

    public static class SubGroupUtil
    {
        public static ISelectable[] FilterBySubGroup(ISelectable[] selectables, ISelectable selected, EqualityComparerComponent equalityComparer)
        {
            List<ISelectable> result = new List<ISelectable>();
            if (selectables == null || selected == null || equalityComparer == null)
            {
                return result.ToArray();
            }
            foreach (var item in selectables)
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