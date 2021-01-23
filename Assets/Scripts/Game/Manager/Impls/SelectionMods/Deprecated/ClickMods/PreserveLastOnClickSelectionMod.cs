
using RTSEngine.Manager.Impls.SelectionMods.Abstracts;

namespace RTSEngine.Manager.Impls.Deprecated
{
    public class PreserveLastOnClickSelectionMod<T, ST> : AbstractClickSelectionMod<T, ST>
    {
        // protected override List<SelectableObject> Execute(SelectionArgs args)
        // {
        //     if (args.OldList.Contains(args.Clicked) && args.OldList.Count == 1 && args.NewList.Count == 0)
        //     {
        //         args.NewList.Add(args.Clicked);
        //     }
        //     return args.NewList;
        // }
        public override ISelectionArgsXP<T, ST> Apply(ISelectionArgsXP<T, ST> args)
        {
            throw new System.NotImplementedException();
        }
    }
}