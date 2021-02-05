using System.Linq;
using System.Collections.Generic;

namespace RTSEngine.Manager
{
    public class LimitSelectionModifier : AbstractSelectionModifier
    {
        private ISelectionManager<ISelectableObject, SelectionTypeEnum> selectionManager;

        public LimitSelectionModifier(ISelectionManager<ISelectableObject, SelectionTypeEnum> selectionManager)
        {
            this.selectionManager = selectionManager;
        }
        public override SelectionArgsXP Apply(SelectionArgsXP args)
        {
            args.ToBeAdded = new HashSet<ISelectableObject>(args.ToBeAdded.Take(selectionManager.GetSettings().Limit));
            return args;
        }

    }


}
