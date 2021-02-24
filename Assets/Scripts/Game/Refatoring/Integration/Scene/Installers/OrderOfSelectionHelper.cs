using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;


namespace RTSEngine.Refactoring
{
    public class OrderOfSelectionHelper : AbstractModifierHelper
    {

        [SerializeField] private List<ObjectTypeEnum> types;
        public override ISelectable[] Apply(ISelectable[] selection)
        {
            List<ISelectable> result = new List<ISelectable>();
            foreach (var item in selection)
            {
                if (item is GameDefaultObject)
                {

                }
            }
            return selection;
        }

    }
}
