using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;


namespace RTSEngine.Refactoring
{
    public class GroupRestrictionHelper : AbstractModifierHelper
    {
        public override ISelectable[] Apply(ISelectable[] selection)
        {
            List<ISelectable> selectables = selection.ToList();
            selectables.RemoveAll(x => (x as GameDefaultObject).selectionOrder > 1);
            if (selectables.Count == 0)
            {
                return selection;
            }
            return selectables.ToArray();
        }
    }
}
