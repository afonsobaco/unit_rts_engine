using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Refactoring
{
    public class ModifiersInterface
    {

        private IModifiersComponent modifiers;

        public ModifiersInterface(IModifiersComponent modifiers)
        {
            this.modifiers = modifiers;
        }

        public virtual ISelectable[] ApplyAll(ISelectable[] oldSelection, ISelectable[] newSelection, SelectionType type)
        {
            ISelectable[] actualSelection = newSelection;
            foreach (var mod in modifiers.GetModifiers())
            {
                if (mod.Type == type || mod.Type == SelectionType.ANY)
                {
                    actualSelection = mod.Apply(oldSelection, newSelection, actualSelection);
                }
            }
            return actualSelection;
        }
    }
}