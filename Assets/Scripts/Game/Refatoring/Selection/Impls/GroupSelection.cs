
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{

    public class GroupSelection : IGroupSelection
    {
        private Dictionary<object, ISelectable[]> groups = new Dictionary<object, ISelectable[]>();

        public ISelectable[] GetSelection(ISelectable[] mainList, object groupId)
        {
            ISelectable[] group = new ISelectable[] { };
            if (mainList == null)
            {
                return group;
            }
            ISelectable[] found;
            groups.TryGetValue(groupId, out found);
            if (found != null)
            {
                group = found;
            }
            return group;
        }

        public void ChangeGroup(object groupId, ISelectable[] selection)
        {
            groups[groupId] = selection;
        }
    }
}
