using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace RTSEngine.Manager
{
    public class OrderSelectionModifier : AbstractSelectionModifier
    {
        private ISelectionManager selectionManager;

        public OrderSelectionModifier(ISelectionManager selectionManager)
        {
            this.selectionManager = selectionManager;
        }
        public override SelectionArguments Apply(SelectionArguments args)
        {
            args.ToBeAdded = GetOrderedSelection(args);
            return args;
        }

        private HashSet<ISelectableObject> GetOrderedSelection(SelectionArguments args)
        {
            List<ISelectableObject> list = new List<ISelectableObject>();
            if (args.ToBeAdded != null && args.ToBeAdded.Count > 0)
            {
                var grouped = args.ToBeAdded.GroupBy(x => x, new EqualityComparer());
                var sorted = grouped.ToList();
                sorted.Sort(new ObjectComparer());
                foreach (var item in sorted)
                {
                    list.AddRange(item);
                }
            }
            return new HashSet<ISelectableObject>(list);
        }

        private class ObjectComparer : IComparer<IGrouping<ISelectableObject, ISelectableObject>>
        {
            public int Compare(IGrouping<ISelectableObject, ISelectableObject> x, IGrouping<ISelectableObject, ISelectableObject> y)
            {
                int v = y.Key.SelectableObjectInfo.SelectionOrder - x.Key.SelectableObjectInfo.SelectionOrder;
                if (v == 0)
                {
                    if (y.Key.LifeStatus.MaxValue > x.Key.LifeStatus.MaxValue)
                    {
                        return 1;
                    }
                    else if (y.Key.LifeStatus.MaxValue < x.Key.LifeStatus.MaxValue)
                    {
                        return -1;
                    }
                }
                return v;
            }
        }

        private class EqualityComparer : IEqualityComparer<ISelectableObject>
        {
            public bool Equals(ISelectableObject x, ISelectableObject y)
            {
                return x.SelectableObjectInfo.Type == y.SelectableObjectInfo.Type && x.SelectableObjectInfo.ObjectName == y.SelectableObjectInfo.ObjectName;
            }

            public int GetHashCode(ISelectableObject obj)
            {
                int hCode = obj.SelectableObjectInfo.Type.GetHashCode() + obj.SelectableObjectInfo.ObjectName.GetHashCode();
                return hCode;
            }
        }

    }
}
