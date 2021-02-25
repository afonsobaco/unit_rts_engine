using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

public abstract class EqualityComparerComponent : MonoBehaviour, IEqualityComparer<ISelectable>
{
    public abstract bool Equals(ISelectable x, ISelectable y);

    public abstract int GetHashCode(ISelectable obj);
}
