using System.Linq;

namespace RTSEngine.Manager
{
    public class LimitSelectionModifier : ISelectionModifier
    {
        public SelectionArgsXP Apply(SelectionArgsXP args, params object[] other)
        {
            SelectionResult result = args.Result;

            int maxLimit = GetMaxLimit(other);
            result = new SelectionResult(args.Result.ToBeAdded.Take(maxLimit).ToList());
            args.Result = result;

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
