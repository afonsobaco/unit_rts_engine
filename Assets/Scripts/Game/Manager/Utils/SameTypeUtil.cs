using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;
namespace RTSEngine.Manager
{
    public class SameTypeUtil
    {

        public static List<ISelectable> GetFromSameTypeInScreen(SelectionArgsXP args)
        {
            if (args.NewSelection == null || args.NewSelection.Count != 1)
            {
                return new List<ISelectable>();
            }
            List<ISelectable> mainList = null;
            List<ISelectable> list = SelectionUtil.GetAllObjectsInsideSelectionArea(mainList, args.SameTypeArgs.initialScreenPosition, args.SameTypeArgs.finalScreenPosition);
            return list.FindAll(a => args.NewSelection[0].IsCompatible(a));
        }

    }
}
