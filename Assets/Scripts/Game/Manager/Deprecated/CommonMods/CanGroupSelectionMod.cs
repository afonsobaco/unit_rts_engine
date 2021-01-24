// using System.Collections.Generic;
// using RTSEngine.Core;
// using UnityEngine;

// using RTSEngine.Manager.SelectionMods;

// namespace RTSEngine.Manager.Deprecated
// {
//     public class CanGroupSelectionMod<T, ST> : IBaseSelectionMod
//     {
//         //     protected override List<ISelectable> Apply(SelectionArgs args)
//         //     {
//         //         var allNewObjectsThatCanGroup = args.NewList.FindAll(a => args.Settings.CanGroupTypes.Contains(a.type));
//         //         if (allNewObjectsThatCanGroup.Count > 0)
//         //         {
//         //             return allNewObjectsThatCanGroup;
//         //         }
//         //         else if (args.NewList.Count > 0)
//         //         {
//         //             return new List<ISelectable>() { args.NewList[Random.Range(0, args.NewList.Count)] };

//         //         }
//         //         else if (args.OldList.Count > 0)
//         //         {
//         //             return args.OldList;
//         //         }
//         //         else
//         //         {
//         //             return args.NewList;
//         //         }
//         //     }

//         public override SelectionArgsXP Apply(SelectionArgsXP args)
//         {
//             throw new System.NotImplementedException();
//         }
//     }

// }