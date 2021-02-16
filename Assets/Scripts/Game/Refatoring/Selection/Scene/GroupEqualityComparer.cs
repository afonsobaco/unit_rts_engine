using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;


namespace RTSEngine.Refactoring
{
    public class GroupEqualityComparer : ModifierEqualityComparerComponent
    {
        public override bool Equals(ISelectable x, ISelectable y)
        {
            var first = x as DefaultObject;
            var second = y as DefaultObject;
            return first.objectType == second.objectType;
        }

        public override int GetHashCode(ISelectable obj)
        {
            var first = obj as DefaultObject;
            int hCode = first.objectType.GetHashCode();
            return hCode;
        }
    }
}
