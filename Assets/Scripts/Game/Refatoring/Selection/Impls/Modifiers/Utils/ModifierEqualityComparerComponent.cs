using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

public abstract class ModifierEqualityComparerComponent : MonoBehaviour, IEqualityComparer<ISelectable>
{

    public virtual bool Equals(ISelectable x, ISelectable y)
    {
        return x.Equals(y);
    }

    public virtual int GetHashCode(ISelectable obj)
    {
        return obj.GetHashCode();
    }

}
