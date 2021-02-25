using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

public abstract class GroupingComparerComponent : MonoBehaviour, IComparer<IGrouping<ISelectable, ISelectable>>
{
    public abstract int Compare(IGrouping<ISelectable, ISelectable> x, IGrouping<ISelectable, ISelectable> y);
}
