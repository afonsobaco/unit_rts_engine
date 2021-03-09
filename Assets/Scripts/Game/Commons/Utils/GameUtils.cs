using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RTSEngine.Utils
{
    public static class GameUtils
    {
        public static ISelectable[] GetOrderedSelection(ISelectable[] actualSelection, IEqualityComparer<ISelectable> equalityComparer, IComparer<IGrouping<ISelectable, ISelectable>> groupSortComparer)
        {
            List<ISelectable> list = new List<ISelectable>();
            if (actualSelection.Length > 0)
            {
                var grouped = actualSelection.GroupBy(x => x, equalityComparer);
                var sorted = grouped.ToList();
                sorted.Sort(groupSortComparer);
                foreach (var item in sorted)
                {
                    list.AddRange(item);
                }
            }
            return list.ToArray();
        }

        public static int GetAnyPartyKeyPressed()
        {
            return GetAnyKeyNumberPressed(_partyKeys);
        }

        public static int GetAnyNumpadKeyPressed()
        {
            return GetAnyKeyNumberPressed(_numpadKeys);
        }

        private static int GetAnyKeyNumberPressed(Dictionary<KeyCode, int> dict)
        {
            foreach (KeyValuePair<KeyCode, int> entry in dict)
            {
                if (Input.GetKeyDown(entry.Key))
                {
                    return entry.Value;
                }
            }
            return 0;
        }

        public static G GetComponent<T, G>(ISelectable obj) where T : DefaultSelectable
        {
            var defaultObject = obj as T;
            if (defaultObject)
            {
                var component = defaultObject.GetComponent<G>();
                return component;
            }
            return default;
        }

        private static Dictionary<KeyCode, int> _partyKeys = new Dictionary<KeyCode, int>()
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

        private static Dictionary<KeyCode, int> _numpadKeys = new Dictionary<KeyCode, int>()
            {
                {KeyCode.Keypad0, 0},
                {KeyCode.Keypad1, 1},
                {KeyCode.Keypad2, 2},
                {KeyCode.Keypad3, 3},
                {KeyCode.Keypad4, 4},
                {KeyCode.Keypad5, 5},
                {KeyCode.Keypad6, 6},
                {KeyCode.Keypad7, 7},
                {KeyCode.Keypad8, 8},
                {KeyCode.Keypad9, 9},
            };

        public static T[] GetAllInactiveChildren<T>(GameObject parent) where T : MonoBehaviour
        {
            var allChildren = parent.GetComponentsInChildren<T>(true).ToList();
            allChildren.RemoveAll(x => x.gameObject.activeInHierarchy);
            return allChildren.ToArray();
        }

        public static T FindInComponent<T>(GameObject parent) where T : MonoBehaviour
        {
            T found = parent.GetComponentInChildren<T>(true);
            if (found)
            {
                return found;
            }
            return parent.GetComponent<T>();
        }
    }
}
