using System.Linq;
using System.Collections.Generic;

namespace RTSEngine.Manager
{
    public class LimitSelectionModifier : AbstractSelectionModifier
    {
        private ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> selectionManager;

        public LimitSelectionModifier(ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> selectionManager)
        {
            this.selectionManager = selectionManager;
        }
        public override SelectionArgsXP Apply(SelectionArgsXP args)
        {
            args.ToBeAdded = new HashSet<ISelectableObjectBehaviour>(args.ToBeAdded.Take(selectionManager.GetSettings().Limit));
            return args;
        }

    }


}
