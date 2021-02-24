using System;
using RTSEngine.Core;
using System.Linq;

namespace RTSEngine.Refactoring
{
    public class GroupEqualityComparer : ModifierEqualityComparerComponent
    {
        public override int Compare(IGrouping<ISelectable, ISelectable> x, IGrouping<ISelectable, ISelectable> y)
        {
            if (x.Key is IComparable && y.Key is IComparable)
            {
                return (x.Key as IComparable).CompareTo(y.Key as IComparable);
            }
            return 0;
        }

        public override bool Equals(ISelectable x, ISelectable y)
        {
            var first = x as DefaultObject;
            var second = y as DefaultObject;
            return first.objectType == second.objectType;
        }

        public override int GetHashCode(ISelectable obj)
        {
            var first = obj as DefaultObject;
            int hCode = first.objectType.GetHashCode();
            return hCode;
        }
    }
}
