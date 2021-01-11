using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Selection.Util;
using System.Linq;
using System;

namespace RTSEngine.Selection.Mod
{
    public class PreSelectionLimitSelectionMod : AbstractSelectionMod
    {
        protected override List<SelectableObject> Apply(SelectionArgs args)
        {
            if (args.OldList.Count >= GetSelectionSettings().SelectionLimit)
            {
                if(args.OldList.FindAll(a => !args.NewList.Contains(a)).Count == 0 ){
                    return args.OldList;
                }
            }
            return args.NewList;
        }

    }



}