using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Selection.Util;
using System.Linq;
using UnityEngine;

namespace RTSEngine.Selection.Mod
{
    public class LimitedSelectionOnClickSelectionMod : AbstractClickSelectionMod
    {
        protected override List<SelectableObject> Execute(SelectionArgs args)
        {
            if (args.NewList.Count <= GetSelectionSettings().SelectionLimit)
            {
                return args.NewList;
            }
            List<SelectableObject> list = new List<SelectableObject>();
            if (args.Clicked && args.NewList.Contains(args.Clicked))
            {
                list.Add(args.Clicked);
            }
            list.AddRange(args.NewList.Take(GetSelectionSettings().SelectionLimit - list.Count).ToList());
            return list;
        }


    }



}