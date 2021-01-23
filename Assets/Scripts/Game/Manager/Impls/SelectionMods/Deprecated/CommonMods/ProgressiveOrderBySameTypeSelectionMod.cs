using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;


using RTSEngine.Manager.Impls.SelectionMods.Abstracts;

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
        public override ISelectionArgsXP<T, ST> Apply(ISelectionArgsXP<T, ST> args)
        {
            throw new System.NotImplementedException();
        }
    }

}