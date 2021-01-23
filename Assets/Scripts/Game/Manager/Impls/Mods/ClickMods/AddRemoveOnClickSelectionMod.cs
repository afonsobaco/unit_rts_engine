using System.Collections.Generic;
using RTSEngine.Core;

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
        public override SelectionArgsXP<T, ST> Apply(SelectionArgsXP<T, ST> args)
        {
            throw new System.NotImplementedException();
        }
    }
}