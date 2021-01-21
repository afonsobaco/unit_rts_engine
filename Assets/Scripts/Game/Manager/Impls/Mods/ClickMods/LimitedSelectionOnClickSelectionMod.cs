
namespace RTSEngine.Manager.Impls.Deprecated
{
    public class LimitedSelectionOnClickSelectionMod<T, E, O> : AbstractClickSelectionMod<T, E, O>
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
        public override SelectionArgsXP<T, E, O> Apply(SelectionArgsXP<T, E, O> args)
        {
            throw new System.NotImplementedException();
        }
    }



}