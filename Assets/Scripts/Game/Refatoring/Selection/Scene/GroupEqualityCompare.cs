using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;


namespace RTSEngine.Refactoring
{
    public class GroupEqualityCompare : ModifierEqualityComparerComponent
    {
        public override bool Equals(ISelectable x, ISelectable y)
        {
            var first = x as SelectableObjectSelection;
            var second = y as SelectableObjectSelection;
            return first.objectType == second.objectType;
        }

        public override int GetHashCode(ISelectable obj)
        {
            var first = obj as SelectableObjectSelection;
            int hCode = first.objectType.GetHashCode();
            return hCode;
        }
    }
}
