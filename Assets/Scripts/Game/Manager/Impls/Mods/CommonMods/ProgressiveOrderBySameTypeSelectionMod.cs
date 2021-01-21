using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;


namespace RTSEngine.Manager.Impls.Deprecated
{
    public class ProgressiveOrderBySameTypeSelectionMod<T, E, O> : AbstractSelectionMod<T, E, O>
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
        public override SelectionArgsXP<T, E, O> Apply(SelectionArgsXP<T, E, O> args)
        {
            throw new System.NotImplementedException();
        }
    }

}