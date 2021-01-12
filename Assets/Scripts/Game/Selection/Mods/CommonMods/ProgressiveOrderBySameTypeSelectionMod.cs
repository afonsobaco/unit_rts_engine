using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;
using RTSEngine.Selection.Util;

namespace RTSEngine.Selection.Mod
{
    public class ProgressiveOrderBySameTypeSelectionMod<T, E> : AbstractSelectionMod<T, E>
    {
        // protected override List<SelectableObject> Apply(SelectionArgs args)
        // {
        //     List<SelectableObject> result = new List<SelectableObject>();
        //     foreach (var selected in args.NewList)
        //     {
        //         if (!result.Contains(selected))
        //         {
        //             result.AddRange(args.NewList.FindAll(a => SameTypeUtil.isSameType(a, selected)));
        //         }
        //     }
        //     return result;
        // }
        public override SelectionArgsXP<T, E> Apply(SelectionArgsXP<T, E> args)
        {
            throw new System.NotImplementedException();
        }
    }

}