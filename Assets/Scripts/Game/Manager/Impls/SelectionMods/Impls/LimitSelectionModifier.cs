using System.Linq;
using System.Collections.Generic;

namespace RTSEngine.Manager
{
    public class LimitSelectionModifier : ISelectionModifier
    {
        public SelectionArgsXP Apply(SelectionArgsXP args, params object[] other)
        {
            int maxLimit = GetMaxLimit(other);
            args.ToBeAdded = new HashSet<ISelectableObjectBehaviour>(args.ToBeAdded.Take(maxLimit));
            return args;
        }

        private int GetMaxLimit(object[] other)
        {
            int limit = 10;
            if (other.Length > 0 && other[0] is int)
            {
                limit = (int)other[0];
            }
            return limit;
        }
    }


}
