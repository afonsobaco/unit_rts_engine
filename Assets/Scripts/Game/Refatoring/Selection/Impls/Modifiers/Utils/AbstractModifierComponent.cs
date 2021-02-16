using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public abstract class AbstractModifierComponent : MonoBehaviour, IModifier
    {
        public abstract ISelectable[] Apply(ISelectable[] selection);
    }
}
