using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Selection.Util;
using UnityEngine;
using System;

namespace RTSEngine.Selection.Mod
{
    public class FindSameTypeInScreenOnClickSelectionMod : AbstractClickSelectionMod
    {
        // protected override List<SelectableObject> Execute(SelectionArgs args)
        // {
        //     if (args.IsSameType && args.Settings.CanSelectSameType.Contains(args.Clicked.type))
        //     {
        //         List<SelectableObject> list = SameTypeUtil.GetFromSameTypeInSelection(args, args.Settings.InitialGameScreenPos, args.Settings.FinalGameScreenPos);
        //         if (args.OldList.Contains(args.Clicked))
        //         {
        //             args.NewList.RemoveAll(a => list.Contains(a));
        //         }
        //         else
        //         {
        //             args.NewList = args.NewList.Union(list).ToList();
        //         }

        //         if (args.NewList.Count == 0)
        //         {
        //             args.NewList.AddRange(list);
        //         }
        //     }
        //     return args.NewList;
        // }
        protected override List<SelectableObject> Apply(SelectionArgsXP args)
        {
            throw new NotImplementedException();
        }
    }
}