
// using System.Linq;
// using System.Collections.Generic;
// using UnityEngine;
// using RTSEngine.Core;

// using RTSEngine.Manager;

// namespace RTSEngine.Manager.Deprecated
// {

//     public class GetByPrecendenceOrderOnDragSelectionMod<T, ST> : AbstractDragSelectionMod<T, ST>
//     {

//         // protected override List<ISelectable> Execute(SelectionArgs args)
//         // {

//         //     List<ISelectable> filteredList = args.NewList.FindAll(a => args.Settings.PrimaryTypes.Contains(a.type));
//         //     if (filteredList.Count == 0)
//         //     {
//         //         if (args.Arguments.IsPreSelection)
//         //         {
//         //             return new List<ISelectable>();
//         //         }
//         //         filteredList = GetSecondarySelection(args);
//         //     }
//         //     if (args.OldList.Count > 0)
//         //     {
//         //         filteredList = args.OldList.Union(filteredList).ToList();
//         //     }
//         //     return filteredList;
//         // }

//         // private List<ISelectable> GetSecondarySelection(SelectionArgs args)
//         // {
//         //     foreach (var secondaryType in args.Settings.SecondaryOrderedTypes)
//         //     {
//         //         List<ISelectable> list = args.NewList.FindAll(a => a.type == secondaryType);
//         //         if (list.Count > 0)
//         //         {
//         //             return new List<ISelectable>() { list[Random.Range(0, list.Count)] };
//         //         }
//         //     }
//         //     return new List<ISelectable>();
//         // }
//         public override SelectionArgsXP Apply(SelectionArgsXP args)
//         {
//             throw new System.NotImplementedException();
//         }
//     }
// }
