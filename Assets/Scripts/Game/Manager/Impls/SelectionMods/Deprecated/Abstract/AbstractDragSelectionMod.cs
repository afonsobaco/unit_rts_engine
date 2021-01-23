using System.Collections.Generic;
using RTSEngine.Core;

using RTSEngine.Manager.Impls.SelectionMods.Abstracts;

namespace RTSEngine.Manager.Impls.Deprecated
{
    public abstract class AbstractDragSelectionMod<T, ST> : AbstractSelectionMod<T, ST>
    {

        // protected override List<SelectableObject> Apply(SelectionArgs args)
        // {
        //     if (args.Clicked)
        //     {
        //         return args.NewList;
        //     }
        //     return Execute(args);
        // }

        // protected abstract List<SelectableObject> Execute(SelectionArgs args);
    }
}
