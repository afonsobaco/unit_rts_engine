// using System;

// using RTSEngine.Manager;

// namespace RTSEngine.Manager.Deprecated
// {
//     public class FindSameTypeInScreenOnClickSelectionMod<T, ST> : AbstractClickSelectionMod
//     {
//         // protected override List<ISelectable> Execute(SelectionArgs args)
//         // {
//         //     if (args.IsSameType && args.Settings.CanSelectSameType.Contains(args.Clicked.type))
//         //     {
//         //         List<ISelectable> list = SameTypeUtil.GetFromSameTypeInSelection(args, args.Settings.InitialGameScreenPos, args.Settings.FinalGameScreenPos);
//         //         if (args.OldList.Contains(args.Clicked))
//         //         {
//         //             args.NewList.RemoveAll(a => list.Contains(a));
//         //         }
//         //         else
//         //         {
//         //             args.NewList = args.NewList.Union(list).ToList();
//         //         }

//         //         if (args.NewList.Count == 0)
//         //         {
//         //             args.NewList.AddRange(list);
//         //         }
//         //     }
//         //     return args.NewList;
//         // }
//         public override SelectionArgsXP Apply(SelectionArgsXP args)
//         {
//             throw new NotImplementedException();
//         }
//     }
// }