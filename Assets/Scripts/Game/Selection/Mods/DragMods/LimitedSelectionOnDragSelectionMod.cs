using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Selection.Util;
using System.Linq;
using UnityEngine;

namespace RTSEngine.Selection.Mod
{
    public class LimitedSelectionOnDragSelectionMod : AbstractDragSelectionMod
    {
        protected override List<SelectableObject> Execute(SelectionArgs args)
        {
            if (args.NewList.Count <= GetSelectionSettings().SelectionLimit)
            {
                return args.NewList;
            }
            List<SelectableObject> list = new List<SelectableObject>();


            list.AddRange(args.PreSelectionList.FindAll(a => args.NewList.Contains(a)).Take(GetSelectionSettings().SelectionLimit).ToList());

            list.AddRange(args.NewList.Take(GetSelectionSettings().SelectionLimit - list.Count).ToList());
            return list;
        }


    }



}