using System.Collections.Generic;
using RTSEngine.Core;

using System.Linq;
using System;

namespace RTSEngine.Manager.Impls.Deprecated
{
    public class PreventSelectionChangeSelectionMod<T, E, O> : AbstractSelectionMod<T, E, O>
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
        public override SelectionArgsXP<T, E, O> Apply(SelectionArgsXP<T, E, O> args)
        {
            throw new NotImplementedException();
        }
    }



}