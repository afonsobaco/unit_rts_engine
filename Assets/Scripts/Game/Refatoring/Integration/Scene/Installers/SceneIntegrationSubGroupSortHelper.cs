using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Commons;
using System.Linq;

namespace RTSEngine.Refactoring
{
    public class SceneIntegrationSubGroupSortHelper : GroupSortComparerComponent
    {
        public override int Compare(IGrouping<ISelectable, ISelectable> x, IGrouping<ISelectable, ISelectable> y)
        {
            if (x.Key is SceneIntegrationObject && y.Key is SceneIntegrationObject)
            {
                var xSubGroup = (x.Key as SceneIntegrationObject).GetComponent<SceneIntegrationGameSubGroup>();
                var ySubGroup = (y.Key as SceneIntegrationObject).GetComponent<SceneIntegrationGameSubGroup>();
                if (xSubGroup && ySubGroup)
                {
                    return xSubGroup.CompareTo(ySubGroup);
                }
            }
            return 0;
        }
    }
}
