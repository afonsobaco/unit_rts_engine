using System.Collections.Generic;
using RTSEngine.Core;
using System.Linq;

namespace RTSEngine.Selection.Mod
{
    public class PreserveLastOnClickSelectionMod<T, E> : AbstractClickSelectionMod<T, E>
    {
        // protected override List<SelectableObject> Execute(SelectionArgs args)
        // {
        //     if (args.OldList.Contains(args.Clicked) && args.OldList.Count == 1 && args.NewList.Count == 0)
        //     {
        //         args.NewList.Add(args.Clicked);
        //     }
        //     return args.NewList;
        // }
        public override SelectionArgsXP<T, E> Apply(SelectionArgsXP<T, E> args)
        {
            throw new System.NotImplementedException();
        }
    }
}