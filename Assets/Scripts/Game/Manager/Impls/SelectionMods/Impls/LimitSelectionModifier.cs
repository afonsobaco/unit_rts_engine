using System.Linq;
using System.Collections.Generic;

namespace RTSEngine.Manager
{
    public class LimitSelectionModifier : AbstractSelectionModifier
    {
        private ISelectionManager selectionManager;

        public LimitSelectionModifier(ISelectionManager selectionManager)
        {
            this.selectionManager = selectionManager;
        }
        public override SelectionArguments Apply(SelectionArguments args)
        {
            args.ToBeAdded = new HashSet<ISelectableObject>(args.ToBeAdded.Take(selectionManager.GetSettings().Limit));
            return args;
        }

    }


}
