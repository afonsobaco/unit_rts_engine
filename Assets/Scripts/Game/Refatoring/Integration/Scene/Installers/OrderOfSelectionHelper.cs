using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;


namespace RTSEngine.Refactoring
{
    public class OrderOfSelectionHelper : AbstractModifierHelper
    {
        public override ISelectable[] Apply(ISelectable[] selection)
        {
            List<IGrouping<int, ISelectable>> items = FakePriorityGroup(selection);
            items.Sort(new ObjectComparer());
            List<ISelectable> selectables = new List<ISelectable>();
            if (items.Count > 0)
            {
                switch (items[0].Key)
                {
                    case 0:
                        selectables = items[0].ToList();
                        break;
                    case 1:
                        selectables.Add(items[0].First());
                        break;
                    default:
                        break;
                }
            }
            return selectables.ToArray();
        }

        public List<IGrouping<int, ISelectable>> FakePriorityGroup(ISelectable[] selection)
        {
            return selection.GroupBy(x => (x as DefaultObject).selectionOrder).ToList();
        }

        public class ObjectComparer : IComparer<IGrouping<int, ISelectable>>
        {
            public int Compare(IGrouping<int, ISelectable> x, IGrouping<int, ISelectable> y)
            {
                return x.Key.CompareTo(y.Key);
            }
        }


    }
}
