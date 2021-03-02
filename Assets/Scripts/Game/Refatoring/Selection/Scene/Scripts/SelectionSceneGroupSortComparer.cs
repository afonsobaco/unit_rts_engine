using System;
using RTSEngine.Commons;
using System.Linq;
using RTSEngine.Core;

namespace RTSEngine.Refactoring.Scene.Selection
{
    public class SelectionSceneGroupSortComparer : GroupSortComparerComponent
    {
        public override int Compare(IGrouping<ISelectable, ISelectable> x, IGrouping<ISelectable, ISelectable> y)
        {
            if (x.Key is IComparable && y.Key is IComparable)
            {
                return (x.Key as IComparable).CompareTo(y.Key as IComparable);
            }
            return 0;
        }
    }
}