using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Commons
{
    public abstract class GroupSortComparerComponent : MonoBehaviour, IComparer<IGrouping<ISelectable, ISelectable>>
    {
        public abstract int Compare(IGrouping<ISelectable, ISelectable> x, IGrouping<ISelectable, ISelectable> y);
    }
}