using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Selection.Mod
{
    public class AddRemoveOnClickSelectionMod<T, E> : AbstractClickSelectionMod<T, E>
    {
        // protected override List<SelectableObject> Execute(SelectionArgs args)
        // {
        //     if (args.OldList.Contains(args.Clicked))
        //     {
        //         args.NewList.Remove(args.Clicked);
        //     }
        //     return args.NewList;

        // }
        public override SelectionArgsXP<T, E> Apply(SelectionArgsXP<T, E> args)
        {
            throw new System.NotImplementedException();
        }
    }
}