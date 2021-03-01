using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Commons;
using RTSEngine.Core;

public class EqualityComparer : EqualityComparerComponent
{
    public override bool Equals(ISelectable x, ISelectable y)
    {
        if (x is GameSelectable && y is GameSelectable)
        {
            var first = x as GameSelectable;
            var second = y as GameSelectable;
            return first.Type == second.Type;
        }
        return x.Equals(y);
    }

    public override int GetHashCode(ISelectable obj)
    {
        if (obj is GameSelectable)
        {
            var first = obj as GameSelectable;
            int hCode = first.Type.GetHashCode();
            return hCode;
        }
        return obj.GetHashCode();
    }
}
