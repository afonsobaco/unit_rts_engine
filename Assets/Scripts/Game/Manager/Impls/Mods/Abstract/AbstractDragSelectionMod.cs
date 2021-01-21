using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Manager.Impls.Deprecated
{
    public abstract class AbstractDragSelectionMod<T, E, O> : AbstractSelectionMod<T, E, O>
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
