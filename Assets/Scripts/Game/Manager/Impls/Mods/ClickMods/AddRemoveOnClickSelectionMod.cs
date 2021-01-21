using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Manager.Impls.Deprecated
{
    public class AddRemoveOnClickSelectionMod<T, E, O> : AbstractClickSelectionMod<T, E, O>
    {
        // protected override List<SelectableObject> Execute(SelectionArgs args)
        // {
        //     if (args.OldList.Contains(args.Clicked))
        //     {
        //         args.NewList.Remove(args.Clicked);
        //     }
        //     return args.NewList;

        // }
        public override SelectionArgsXP<T, E, O> Apply(SelectionArgsXP<T, E, O> args)
        {
            throw new System.NotImplementedException();
        }
    }
}