using System.Collections.Generic;
using RTSEngine.Core;

using System.Linq;
using System;

using RTSEngine.Manager.Impls.SelectionMods.Abstracts;

namespace RTSEngine.Manager.Impls.Deprecated
{
    public class PreventSelectionChangeSelectionMod<T, ST> : AbstractSelectionMod<T, ST>
    {
        // protected override List<SelectableObject> Apply(SelectionArgs args)
        // {
        //     if (args.OldList.Count >= args.Settings.SelectionLimit)
        //     {
        //         if(args.OldList.FindAll(a => !args.NewList.Contains(a)).Count == 0 ){
        //             return args.OldList;
        //         }
        //     }
        //     return args.NewList;
        // }
        public override ISelectionArgsXP<T, ST> Apply(ISelectionArgsXP<T, ST> args)
        {
            throw new NotImplementedException();
        }
    }



}