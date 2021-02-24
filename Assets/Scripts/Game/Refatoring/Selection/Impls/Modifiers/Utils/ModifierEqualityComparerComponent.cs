﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

public abstract class ModifierEqualityComparerComponent : MonoBehaviour, IEqualityComparer<ISelectable>, IComparer<IGrouping<ISelectable, ISelectable>>
{
    public abstract int Compare(IGrouping<ISelectable, ISelectable> x, IGrouping<ISelectable, ISelectable> y);

    public abstract bool Equals(ISelectable x, ISelectable y);

    public abstract int GetHashCode(ISelectable obj);

}
