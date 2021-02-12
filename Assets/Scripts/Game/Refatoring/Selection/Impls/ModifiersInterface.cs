using RTSEngine.Core;
using System;

namespace RTSEngine.Refactoring
{
    public class ModifiersInterface
    {
        public virtual ISelectable[] ApplyAll(ISelectable[] selection, SelectionType type)
        {
            throw new NotImplementedException();
        }
    }
}