using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Selection.Mod
{
    public class AddRemoveOnClickSelectionMod : AbstractClickSelectionMod
    {
        protected override List<SelectableObject> Execute(SelectionArgs args)
        {
            if (args.OldList.Contains(args.Clicked))
            {
                args.NewList.Remove(args.Clicked);
            }
            return args.NewList;

        }
    }
}