using RTSEngine.Core;
using System;

namespace RTSEngine.Refactoring
{
    public class ModifiersInterface
    {

        private ISelectionModifier[] modifiers;

        public ModifiersInterface(ISelectionModifier[] modifiers)
        {
            this.modifiers = modifiers;
        }

        public virtual ISelectable[] ApplyAll(ISelectable[] oldSelection, ISelectable[] newSelection, SelectionType type)
        {
            ISelectable[] actualSelection = newSelection;
            foreach (var mod in modifiers)
            {
                if (mod.Type == type || mod.Type == SelectionType.ANY)
                {
                    actualSelection = mod.Apply(oldSelection, newSelection, actualSelection, type);
                }
            }
            return actualSelection;
        }
    }
}