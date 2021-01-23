using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Manager;

namespace Tests
{
    public class ModifierTestUtils
    {

        public static List<SelectableObject> GetListByIndex(int[] oldSelection, List<SelectableObject> mainList)
        {
            var list = new List<SelectableObject>();
            for (var i = 0; i < oldSelection.Length; i++)
            {
                if (oldSelection[i] < mainList.Count)
                    list.Add(mainList[oldSelection[i]]);
            }
            return list;
        }

        public static List<SelectableObject> GetSomeObjects(int qtt)
        {
            var list = new List<SelectableObject>();
            for (var i = 0; i < qtt; i++)
            {
                var go = new GameObject();
                var so = go.AddComponent<SelectableObject>();
                list.Add(so);
            }
            return list;
        }

        public static ISelectionArgsXP<SelectableObject, SelectionTypeEnum> GetDefaultArgs()
        {
            return new SelectionArgsXP<SelectableObject, SelectionTypeEnum>();
        }
    }
}
