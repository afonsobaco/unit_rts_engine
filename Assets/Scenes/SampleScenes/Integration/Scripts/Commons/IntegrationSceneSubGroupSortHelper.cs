using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Commons;
using System.Linq;

namespace RTSEngine.Integration.Scene
{
    public class IntegrationSceneSubGroupSortHelper : GroupSortComparerComponent
    {
        public override int Compare(IGrouping<ISelectable, ISelectable> x, IGrouping<ISelectable, ISelectable> y)
        {
            if (x.Key is IntegrationSceneObject && y.Key is IntegrationSceneObject)
            {
                var xSubGroup = (x.Key as IntegrationSceneObject).GetComponent<IntegrationSceneGameSubGroup>();
                var ySubGroup = (y.Key as IntegrationSceneObject).GetComponent<IntegrationSceneGameSubGroup>();
                if (xSubGroup && ySubGroup)
                {
                    return xSubGroup.CompareTo(ySubGroup);
                }
            }
            return 0;
        }
    }
}
