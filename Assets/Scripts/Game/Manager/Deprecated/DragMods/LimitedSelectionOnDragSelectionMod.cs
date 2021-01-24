// using System.Collections.Generic;
// using RTSEngine.Core;

// using System.Linq;
// using UnityEngine;

// using RTSEngine.Manager.SelectionMods;

// namespace RTSEngine.Manager.Deprecated
// {
//     public class LimitedSelectionOnDragSelectionMod<T, ST> : AbstractDragSelectionMod<T, ST>
//     {
//         // protected override List<SelectableObject> Execute(SelectionArgs args)
//         // {
//         //     if (args.NewList.Count <= args.Settings.SelectionLimit)
//         //     {
//         //         return args.NewList;
//         //     }
//         //     List<SelectableObject> list = new List<SelectableObject>();


//         //     list.AddRange(args.PreSelectionList.FindAll(a => args.NewList.Contains(a)).Take(args.Settings.SelectionLimit).ToList());

//         //     list.AddRange(args.NewList.Take(args.Settings.SelectionLimit - list.Count).ToList());
//         //     return list;
//         // }
//         public override SelectionArgsXP Apply(SelectionArgsXP args)
//         {
//             throw new System.NotImplementedException();
//         }
//     }



// }