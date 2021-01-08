using System.Collections.Generic;
using RTSEngine.Core;
using System.Linq;

namespace RTSEngine.Selection.Mod
{
    public class AditiveDragSelectionMod : AbstractSelectionMod
    {
        protected override List<SelectableObject> Apply(SelectionArgs args)
        {
            List<SelectableObject> result = new List<SelectableObject>(args.NewList);
            if (args.IsAditive && !args.Clicked)
            {
                result = result.Union(args.OldList).ToList();
            }
            return result;
        }


        // case o=[(a)] c=a => r[a]
        // case o=[b] c=a => r[a, b]
        // case o=[a, b] c=a => r[b]

    }
}