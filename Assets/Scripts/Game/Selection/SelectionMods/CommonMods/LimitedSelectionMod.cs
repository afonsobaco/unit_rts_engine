using System.Collections.Generic;
using RTSEngine.Core;
using System.Linq;

namespace RTSEngine.Selection.Mod
{
    public class LimitedSelectionMod : AbstractSelectionMod
    {
        protected override List<SelectableObject> Apply(SelectionArgs args)
        {            
            return args.NewList.Take(GetSelectionSettings().SelectionLimit).ToList();
        }

    }

}