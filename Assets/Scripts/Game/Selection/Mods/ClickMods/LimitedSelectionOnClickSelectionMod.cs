using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Selection.Util;
using System.Linq;
using UnityEngine;

namespace RTSEngine.Selection.Mod
{
    public class LimitedSelectionOnClickSelectionMod<T, E> : AbstractClickSelectionMod<T, E>
    {
        // protected override List<SelectableObject> Execute(SelectionArgs args)
        // {
        //     if (args.NewList.Count <= args.Settings.SelectionLimit)
        //     {
        //         return args.NewList;
        //     }
        //     List<SelectableObject> list = new List<SelectableObject>();
        //     if (args.Clicked && args.NewList.Contains(args.Clicked))
        //     {
        //         list.Add(args.Clicked);
        //     }
        //     list.AddRange(args.NewList.Take(args.Settings.SelectionLimit - list.Count).ToList());
        //     return list;
        // }
        public override SelectionArgsXP<T, E> Apply(SelectionArgsXP<T, E> args)
        {
            throw new System.NotImplementedException();
        }
    }



}