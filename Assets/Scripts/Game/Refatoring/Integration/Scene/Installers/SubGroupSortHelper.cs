using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Commons;
using System.Linq;

namespace RTSEngine.Refactoring
{
    public class SubGroupSortHelper : GroupSortComparerComponent
    {
        public override int Compare(IGrouping<ISelectable, ISelectable> x, IGrouping<ISelectable, ISelectable> y)
        {
            if (x.Key is GameDefaultObject && y.Key is GameDefaultObject)
            {
                var xSubGroup = (x.Key as GameDefaultObject).GetComponent<GameSubGroup>();
                var ySubGroup = (y.Key as GameDefaultObject).GetComponent<GameSubGroup>();
                if (xSubGroup && ySubGroup)
                {
                    return xSubGroup.CompareTo(ySubGroup);
                }
            }
            return 0;
        }
    }
}
