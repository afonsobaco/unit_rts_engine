using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Refactoring
{

    public class ModifiersComponent : MonoBehaviour, IModifiersComponent
    {
        public ISelectionModifier[] GetModifiers()
        {
            var selectionModifiers = GetComponents<ISelectionModifier>().ToList();
            selectionModifiers.RemoveAll(x => !(x as BaseSelectionModifier).enabled);
            return selectionModifiers.ToArray();
        }

    }
}
