using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Utils
{
    public static class GameUtils
    {


        public static ISelectable[] GetOrderedSelection(ISelectable[] actualSelection, IEqualityComparer<ISelectable> equalityComparer)
        {
            List<ISelectable> list = new List<ISelectable>();
            if (actualSelection.Length > 0)
            {
                var grouped = actualSelection.GroupBy(x => x, equalityComparer);
                var sorted = grouped.ToList();
                sorted.Sort(new ObjectComparer());
                foreach (var item in sorted)
                {
                    list.AddRange(item);
                }
            }
            return list.ToArray();
        }
        public static List<IGrouping<ISelectable, ISelectable>> GetGrouped(ISelectable[] actualSelection, IEqualityComparer<ISelectable> equalityComparer)
        {
            return actualSelection.GroupBy(x => x, equalityComparer).ToList();
        }


        public static int GetAnyGroupKeyPressed()
        {
            foreach (KeyValuePair<KeyCode, int> entry in _groupKeys)
            {
                if (Input.GetKeyDown(entry.Key))
                {
                    return entry.Value;
                }
            }
            return 0;
        }

        private class ObjectComparer : IComparer<IGrouping<ISelectable, ISelectable>>
        {
            public int Compare(IGrouping<ISelectable, ISelectable> x, IGrouping<ISelectable, ISelectable> y)
            {
                // return x.Key.CompareTo(y.Key);
                return 0;
            }
        }

        private static Dictionary<KeyCode, int> _groupKeys = new Dictionary<KeyCode, int>()
            {
                {KeyCode.Alpha1, 1},
                {KeyCode.Alpha2, 2},
                {KeyCode.Alpha3, 3},
                {KeyCode.Alpha4, 4},
                {KeyCode.Alpha5, 5},
                {KeyCode.Alpha6, 6},
                {KeyCode.Alpha7, 7},
                {KeyCode.Alpha8, 8},
                {KeyCode.Alpha9, 9},
                {KeyCode.Alpha0, 10}
            };
    }
}
