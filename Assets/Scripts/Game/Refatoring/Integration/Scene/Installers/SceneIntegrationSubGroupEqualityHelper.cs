using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Commons;
using RTSEngine.Utils;
using System.Linq;

namespace RTSEngine.Refactoring
{
    public class SceneIntegrationSubGroupEqualityHelper : EqualityComparerComponent
    {
        public override bool Equals(ISelectable x, ISelectable y)
        {
            var xGameSubGroup = GameUtils.GetComponent<SceneIntegrationObject, SceneIntegrationGameSubGroup>(x);
            var yGameSubGroup = GameUtils.GetComponent<SceneIntegrationObject, SceneIntegrationGameSubGroup>(y);
            if (xGameSubGroup && yGameSubGroup)
            {
                return xGameSubGroup.SubGroup.Equals(yGameSubGroup.SubGroup);
            }
            return false;
        }

        public override int GetHashCode(ISelectable obj)
        {
            var result = obj.GetHashCode();
            var gameSubGroup = GameUtils.GetComponent<SceneIntegrationObject, SceneIntegrationGameSubGroup>(obj);
            if (gameSubGroup)
            {
                return gameSubGroup.SubGroup.GetHashCode();
            }
            return result;
        }



    }
}
