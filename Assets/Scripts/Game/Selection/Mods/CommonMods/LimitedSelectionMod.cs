using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Selection.Util;
using System.Linq;
using System;

namespace RTSEngine.Selection.Mod
{
    public class LimitedSelectionMod : AbstractSelectionMod
    {
        protected override List<SelectableObject> Apply(SelectionArgs args)
        {
            if (args.NewList.Count <= GetSelectionSettings().SelectionLimit)
            {
                return args.NewList;
            }
            if (args.Clicked && args.NewList.Contains(args.Clicked))
            {
                List<SelectableObject> list = new List<SelectableObject>();
                list.Add(args.Clicked);
                list.AddRange(args.NewList.FindAll(a => a != args.Clicked).Take(GetSelectionSettings().SelectionLimit - 1).ToList());
                return list;
            }
            return args.NewList.Take(GetSelectionSettings().SelectionLimit).ToList();
        }


    }



}