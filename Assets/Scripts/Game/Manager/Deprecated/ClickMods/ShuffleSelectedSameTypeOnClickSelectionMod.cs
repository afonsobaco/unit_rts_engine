// using System;

// using RTSEngine.Manager.SelectionMods;

// namespace RTSEngine.Manager.Deprecated
// {
//     public class ShuffleSelectedSameTypeOnClickSelectionMod<T, ST> : AbstractClickSelectionMod
//     {
//         // protected override List<ISelectable> Execute(SelectionArgs args)
//         // {
//         //     if ((!args.IsSameType && !args.IsDoubleClick) || !args.NewList.Contains(args.Clicked) )
//         //     {
//         //         return args.NewList;
//         //     }
//         //     return ShuffleOnlyClickedSameType(args);
//         // }

//         // private List<ISelectable> ShuffleOnlyClickedSameType(SelectionArgs args)
//         // {
//         //     List<ISelectable> listOfSameType = args.NewList.FindAll(a => SameTypeUtil.isSameType(args.Clicked, a));
//         //     List<ISelectable> listWhitout = args.NewList.FindAll(a => !SameTypeUtil.isSameType(args.Clicked, a));

//         //     listOfSameType = SelectionUtil.Shuffle(listOfSameType);

//         //     return listWhitout.Union(listOfSameType).ToList();
//         // }
//         public override SelectionArgsXP Apply(SelectionArgsXP args)
//         {
//             throw new NotImplementedException();
//         }
//     }
// }