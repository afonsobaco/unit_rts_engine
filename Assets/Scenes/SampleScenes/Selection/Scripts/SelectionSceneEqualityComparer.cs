using RTSEngine.Commons;
using RTSEngine.Core;
using RTSEngine.Utils;
using UnityEngine;

namespace RTSEngine.RTSSelection.Scene

{
    public class SelectionSceneEqualityComparer : EqualityComparerComponent
    {
        public override bool Equals(ISelectable x, ISelectable y)
        {
            var xGameSubGroup = GameUtils.GetComponent<SelectionSceneObject, SelectionSceneGameSubGroup>(x);
            var yGameSubGroup = GameUtils.GetComponent<SelectionSceneObject, SelectionSceneGameSubGroup>(y);
            if (xGameSubGroup && yGameSubGroup)
            {
                return xGameSubGroup.SubGroup.Equals(yGameSubGroup.SubGroup);
            }
            return false;
        }

        public override int GetHashCode(ISelectable obj)
        {
            var result = obj.GetHashCode();
            var gameSubGroup = GameUtils.GetComponent<SelectionSceneObject, SelectionSceneGameSubGroup>(obj);
            if (gameSubGroup)
            {
                return gameSubGroup.SubGroup.GetHashCode();
            }
            return result;
        }

    }
}