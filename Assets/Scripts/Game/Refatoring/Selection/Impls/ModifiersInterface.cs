using RTSEngine.Core;
using System;

namespace RTSEngine.Refactoring
{
    public class ModifiersInterface
    {
        public virtual ISelectable[] ApplyAll(ISelectable[] expected, SelectionType type)
        {
            throw new NotImplementedException();
        }
    }
}