using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public interface IModifier
    {
        ISelectable[] Apply(ISelectable[] actualSelection);
    }
}
