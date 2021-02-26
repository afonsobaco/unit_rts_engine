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
            var xGameSubGroup = GetGameSubGroup(x);
            var yGameSubGroup = GetGameSubGroup(y);
            if (xGameSubGroup && yGameSubGroup)
            {
                Debug.Log(xGameSubGroup.SubGroup + " == " + yGameSubGroup.SubGroup);
                return xGameSubGroup.SubGroup.Equals(yGameSubGroup.SubGroup);
            }
            return false;
        }

        public override int GetHashCode(ISelectable obj)
        {
            var result = obj.GetHashCode();
            var gameSubGroup = GetGameSubGroup(obj);
            if (gameSubGroup)
            {
                return gameSubGroup.SubGroup.GetHashCode();
            }
            return result;
        }

        private GameSubGroup GetGameSubGroup(ISelectable obj)
        {
            var defaultObject = obj as GameDefaultObject;
            if (defaultObject)
            {
                var gameSubGroup = defaultObject.GetComponent<GameSubGroup>();
                return gameSubGroup;
            }
            return null;
        }

    }
}
