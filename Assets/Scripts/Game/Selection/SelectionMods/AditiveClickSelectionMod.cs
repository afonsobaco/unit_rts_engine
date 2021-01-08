using System.Collections.Generic;
using RTSEngine.Core;
using System.Linq;

namespace RTSEngine.Selection.Mod
{
    public class AditiveClickSelectionMod : AbstractSelectionMod
    {
        protected override List<SelectableObject> Apply(SelectionArgs args)
        {
            if (args.IsAditive && args.Clicked)
            {
                return AddRemoveFromList(args);
            }
            else
            {
                return new List<SelectableObject>(args.NewList);
            }

        }

        private List<SelectableObject> AddRemoveFromList(SelectionArgs args)
        {

            List<SelectableObject> result = new List<SelectableObject>(args.OldList);
            if (result.Contains(args.Clicked))
            {
                if (result.Count > 1)
                {
                    result.Remove(args.Clicked);
                }
            }
            else
            {
                result.Add(args.Clicked);
            }
            return result;

        }
    }
}