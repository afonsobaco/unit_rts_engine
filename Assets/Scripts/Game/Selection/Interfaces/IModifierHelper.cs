using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.RTSSelection
{
    public interface IModifierHelper
    {
        ISelectable[] Apply(ISelectable[] actualSelection);
    }
}
