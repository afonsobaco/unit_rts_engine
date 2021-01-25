using System.Collections.Generic;
using NSubstitute;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Manager;

namespace Tests
{
    public class ModifierTestUtils
    {

        public static List<ISelectable> GetListByIndex(int[] indexes, List<ISelectable> mainList)
        {
            var list = new List<ISelectable>();
            for (var i = 0; i < indexes.Length; i++)
            {
                if (indexes[i] < mainList.Count)
                    list.Add(mainList[indexes[i]]);
            }
            return list;
        }

        public static List<ISelectable> GetSomeObjects(int qtt)
        {
            var list = new List<ISelectable>();
            for (var i = 0; i < qtt; i++)
            {
                list.Add(Substitute.For<ISelectable>());
            }
            return list;
        }

    }
}
