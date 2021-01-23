using System.Collections.Generic;
using RTSEngine.Core;

using RTSEngine.Manager.Impls.SelectionMods.Abstracts;

namespace RTSEngine.Manager.Impls.Deprecated
{
    public class AddRemoveOnClickSelectionMod<T, ST> : AbstractClickSelectionMod<T, ST>
    {
        // protected override List<SelectableObject> Execute(SelectionArgs args)
        // {
        //     if (args.OldList.Contains(args.Clicked))
        //     {
        //         args.NewList.Remove(args.Clicked);
        //     }
        //     return args.NewList;

        // }
        public override ISelectionArgsXP<T, ST> Apply(ISelectionArgsXP<T, ST> args)
        {
            throw new System.NotImplementedException();
        }
    }
}