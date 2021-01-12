using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Selection.Mod
{
    public abstract class AbstractClickSelectionMod : AbstractSelectionMod
    {

        // protected override List<SelectableObject> Apply(SelectionArgs args)
        // {
        //     if (!args.Clicked || args.IsPreSelection)
        //     {
        //         return args.NewList;
        //     }
        //     return Execute(args);
        // }

        // protected abstract List<SelectableObject> Execute(SelectionArgs args);

    }
}