using System.ComponentModel.Design;
using System.Linq;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.RTSSelection
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
                if (mod.RestrictedTypes == null || mod.RestrictedTypes.Length == 0 || mod.RestrictedTypes.Contains(type))
                {
                    SelectionInfo info = new SelectionInfo { OldSelection = oldSelection, NewSelection = newSelection, ActualSelection = actualSelection, SelectionType = type };
                    actualSelection = mod.Apply(info);
                }
            }
            return actualSelection;
        }
    }
}