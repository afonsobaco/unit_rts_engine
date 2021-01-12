using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Selection.Util;
using System.Linq;
using UnityEngine;

namespace RTSEngine.Selection.Mod
{
    public class LimitedSelectionOnDragSelectionMod : AbstractDragSelectionMod
    {
        // protected override List<SelectableObject> Execute(SelectionArgs args)
        // {
        //     if (args.NewList.Count <= args.Settings.SelectionLimit)
        //     {
        //         return args.NewList;
        //     }
        //     List<SelectableObject> list = new List<SelectableObject>();


        //     list.AddRange(args.PreSelectionList.FindAll(a => args.NewList.Contains(a)).Take(args.Settings.SelectionLimit).ToList());

        //     list.AddRange(args.NewList.Take(args.Settings.SelectionLimit - list.Count).ToList());
        //     return list;
        // }
        protected override List<SelectableObject> Apply(SelectionArgsXP args)
        {
            throw new System.NotImplementedException();
        }
    }



}