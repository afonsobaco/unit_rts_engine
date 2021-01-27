using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;
namespace RTSEngine.Manager
{
    public class SameTypeUtil
    {

        public static List<ISelectable> GetFromSameTypeInScreen(SelectionArgsXP args)
        {
            if (args.Arguments.NewSelection == null || args.Arguments.NewSelection.Count != 1)
            {
                return new List<ISelectable>();
            }
            List<ISelectable> list = SelectionUtil.GetAllObjectsInsideSelectionArea(args.Arguments.MainList, args.ModifierArgs.InitialScreenPosition, args.ModifierArgs.FinalScreenPosition);
            return list.FindAll(a => args.Arguments.NewSelection[0].IsCompatible(a));
        }

    }
}
