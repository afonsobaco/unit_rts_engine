using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;


namespace RTSEngine.Manager.Impls.Deprecated
{
    public class ProgressiveOrderBySameTypeSelectionMod<T, ST> : AbstractSelectionMod<T, ST>
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
        public override SelectionArgsXP<T, ST> Apply(SelectionArgsXP<T, ST> args)
        {
            throw new System.NotImplementedException();
        }
    }

}