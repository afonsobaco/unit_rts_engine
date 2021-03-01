using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Commons;
using System.Linq;
using RTSEngine.Core;

public class GroupingComparer : GroupingComparerComponent
{
    public override int Compare(IGrouping<ISelectable, ISelectable> x, IGrouping<ISelectable, ISelectable> y)
    {
        return 0;
    }
}