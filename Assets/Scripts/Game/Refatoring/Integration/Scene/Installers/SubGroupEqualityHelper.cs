using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using System.Linq;

namespace RTSEngine.Refactoring
{
    public class SubGroupEqualityHelper : EqualityComparerComponent
    {
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
