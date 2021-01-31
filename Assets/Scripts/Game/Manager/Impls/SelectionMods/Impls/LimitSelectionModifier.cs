using System.Linq;
using System.Collections.Generic;

namespace RTSEngine.Manager
{
    public class LimitSelectionModifier : ISelectionModifier
    {


        public SelectionTypeEnum Type { get { return SelectionTypeEnum.ANY; } }
        public bool ActiveOnPreSelection { get { return false; } }

        public int MaxLimit { get => maxLimit; set => maxLimit = value; }

        private int maxLimit = 10;

        public SelectionArgsXP Apply(SelectionArgsXP args)
        {
            args.ToBeAdded = new HashSet<ISelectableObjectBehaviour>(args.ToBeAdded.Take(MaxLimit));
            return args;
        }

    }


}
