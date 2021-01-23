
using RTSEngine.Manager.SelectionMods;

namespace RTSEngine.Manager.Deprecated
{
    public class LimitedSelectionOnClickSelectionMod<T, ST> : AbstractClickSelectionMod<T, ST>
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
        public override ISelectionArgsXP<T, ST> Apply(ISelectionArgsXP<T, ST> args)
        {
            throw new System.NotImplementedException();
        }
    }



}