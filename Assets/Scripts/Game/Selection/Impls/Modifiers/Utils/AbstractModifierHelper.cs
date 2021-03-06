using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.RTSSelection
{
    public abstract class AbstractModifierHelper : Zenject.ZenAutoInjecter, IModifierHelper
    {
        public abstract ISelectable[] Apply(ISelectable[] selection);
    }
}
