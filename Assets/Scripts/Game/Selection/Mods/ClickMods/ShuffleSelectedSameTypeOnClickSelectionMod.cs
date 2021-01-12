using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Selection.Util;
using UnityEngine;
using System;

namespace RTSEngine.Selection.Mod
{
    public class ShuffleSelectedSameTypeOnClickSelectionMod<T,E> : AbstractClickSelectionMod<T,E>
    {
        // protected override List<SelectableObject> Execute(SelectionArgs args)
        // {
        //     if ((!args.IsSameType && !args.IsDoubleClick) || !args.NewList.Contains(args.Clicked) )
        //     {
        //         return args.NewList;
        //     }
        //     return ShuffleOnlyClickedSameType(args);
        // }

        // private List<SelectableObject> ShuffleOnlyClickedSameType(SelectionArgs args)
        // {
        //     List<SelectableObject> listOfSameType = args.NewList.FindAll(a => SameTypeUtil.isSameType(args.Clicked, a));
        //     List<SelectableObject> listWhitout = args.NewList.FindAll(a => !SameTypeUtil.isSameType(args.Clicked, a));

        //     listOfSameType = SelectionUtil.Shuffle(listOfSameType);

        //     return listWhitout.Union(listOfSameType).ToList();
        // }
        public override SelectionArgsXP<T, E> Apply(SelectionArgsXP<T, E> args)
        {
            throw new NotImplementedException();
        }
    }
}