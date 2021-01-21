using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;


namespace RTSEngine.Manager.Impls.Deprecated
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