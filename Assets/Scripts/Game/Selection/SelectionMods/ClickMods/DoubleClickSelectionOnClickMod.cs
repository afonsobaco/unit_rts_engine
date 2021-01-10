using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Selection.Util;
using UnityEngine;
using System;

namespace RTSEngine.Selection.Mod
{
    public class DoubleClickSelectionOnClickMod : AbstractClickSelectionMod
    {
        protected override List<SelectableObject> Execute(SelectionArgs args)
        {
            if (args.IsDoubleClick && GetSelectionSettings().CanSelectSameType.Contains(args.Clicked.type))
            {
                List<SelectableObject> list = SameTypeUtil.GetFromSameTypeInSelection(args, GetSelectionSettings().InitialGameScreenPos, GetSelectionSettings().FinalGameScreenPos);
                if (!args.OldList.Contains(args.Clicked))
                {
                    args.NewList.RemoveAll(a => list.Contains(a));
                }
                else
                {
                    args.NewList = args.NewList.Union(list).ToList();
                }

                if (args.NewList.Count == 0)
                {
                    args.NewList.AddRange(list);
                }
            }
            return args.NewList;
        }
    }
}