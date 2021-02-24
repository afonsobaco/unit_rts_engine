using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using System.Linq;

namespace RTSEngine.Refactoring
{
    public class SubGroupSelectionHelper : ModifierEqualityComparerComponent
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

        public override bool Equals(ISelectable x, ISelectable y)
        {
            if (x is GameSubGroup && y is GameSubGroup)
            {
                var first = x as GameSubGroup;
                var second = y as GameSubGroup;
                return first.SubGroup == second.SubGroup;
            }
            return false;
        }

        public override int GetHashCode(ISelectable obj)
        {
            var first = obj as GameSubGroup;
            int hCode = first.SubGroup.GetHashCode();
            return hCode;
        }
    }
}
