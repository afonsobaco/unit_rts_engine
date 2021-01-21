using System;

namespace RTSEngine.Manager.Impls.Deprecated
{
    public class FindSameTypeInScreenOnDoubleClickSelectionMod<T, E> : AbstractClickSelectionMod<T, E>
    {
        public override SelectionArgsXP<T, E> Apply(SelectionArgsXP<T, E> args)
        {
            throw new NotImplementedException();
        }

        // protected override List<SelectableObject> Execute(SelectionArgs args)
        // {
        //     if (args.IsDoubleClick && args.Settings.CanSelectSameType.Contains(args.Clicked.type))
        //     {
        //         List<SelectableObject> list = SameTypeUtil.GetFromSameTypeInSelection(args, args.Settings.InitialGameScreenPos, args.Settings.FinalGameScreenPos);
        //         if (!args.OldList.Contains(args.Clicked))
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
    }
}