using System;

namespace RTSEngine.Manager.Impls.Deprecated
{
    public class FindSameTypeInScreenOnClickSelectionMod<T, ST> : AbstractClickSelectionMod<T, ST>
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
        public override SelectionArgsXP<T, ST> Apply(SelectionArgsXP<T, ST> args)
        {
            throw new NotImplementedException();
        }
    }
}