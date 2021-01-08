using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Selection.Mod
{
    public abstract class AbstracClickSelectionMod : AbstractSelectionMod
    {

        protected override List<SelectableObject> Apply(SelectionArgs args)
        {
            if (!args.Clicked)
            {
                return args.NewList;
            }
            return Execute(args);
        }

        protected abstract List<SelectableObject> Execute(SelectionArgs args);

    }
}