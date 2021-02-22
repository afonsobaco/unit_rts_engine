using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Refactoring
{

    public class ModifiersComponent : MonoBehaviour, IModifiersComponent
    {
        public ISelectionModifier[] GetModifiers()
        {
            return GetComponents<ISelectionModifier>();
        }

    }
}
