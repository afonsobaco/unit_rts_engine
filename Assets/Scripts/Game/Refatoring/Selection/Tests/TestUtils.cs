using System.Linq;
using System.Collections.Generic;
using NSubstitute;
using UnityEngine;
using RTSEngine.Core;

namespace Tests.Utils
{
    public class TestUtils
    {

        public static ISelectable[] GetListByIndex(int[] indexes, ISelectable[] mainList)
        {
            var list = new List<ISelectable>();
            for (var i = 0; i < indexes.Length; i++)
            {
                if (indexes[i] < mainList.Length)
                {
                    list.Add(mainList[indexes[i]]);
                }
            }
            return list.ToArray();
        }

        public static ISelectable[] GetSomeObjects(int qtt)
        {
            var list = new List<ISelectable>();
            for (var i = 0; i < qtt; i++)
            {
                ISelectable item = Substitute.For<ISelectable>();
                item.Index = i;
                list.Add(item);
            }
            return list.ToArray();
        }

        public static CaseStruct[] GetDefaultCases()
        {
            return SelectionList();
        }

        public static string GetCaseName(CaseStruct selectionStruct)
        {

            string name = "";
            name += NameForCollectionLength(selectionStruct.oldSelection, "Old");
            name += NameForCollectionLength(selectionStruct.newSelection, "New");
            name += NameForCollectionContains(selectionStruct.oldSelection, selectionStruct.newSelection, "Old", "New");
            return name;
        }

        private static string NameForCollectionLength(int[] collection, string collectionName)
        {

            var name = " [" + string.Join(", ", collection) + "] ";

            if (collection.Length == 0)
                return "EMPTY " + collectionName + ", ";
            if (collection.Length == 1)
                return "SINGLE " + collectionName + name + ", ";
            return "MULTIPLE " + collectionName + name + ", ";
        }

        private static string NameForCollectionContains(int[] collection, int[] otherCollection, string collectionName, string otherCollectionName)
        {
            if (collection.Length == 0) return "";
            int[] list = collection.ToList().FindAll(x => otherCollection.Contains(x)).ToArray();
            if (list.Length == 0) return collectionName + " Does not contains ANY of " + otherCollectionName + "";
            if (list.Length < otherCollection.Length) return collectionName + " Contains SOME of " + otherCollectionName + "";
            return collectionName + " Contains ALL of " + otherCollectionName + "";
        }



        private static CaseStruct[] SelectionList()
        {
            List<CaseStruct> list = new List<CaseStruct>();
            list.Add(new CaseStruct(10, new int[] { }, new int[] { }));
            list.Add(new CaseStruct(10, new int[] { }, new int[] { 0 }));
            list.Add(new CaseStruct(10, new int[] { }, new int[] { 0, 1, 2, 3, 4 }));
            list.Add(new CaseStruct(10, new int[] { 0 }, new int[] { }));
            list.Add(new CaseStruct(10, new int[] { 0 }, new int[] { 0 }));
            list.Add(new CaseStruct(10, new int[] { 0 }, new int[] { 1 }));
            list.Add(new CaseStruct(10, new int[] { 0 }, new int[] { 1, 2, 3, 4, 5 }));
            list.Add(new CaseStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { }));
            list.Add(new CaseStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }));
            list.Add(new CaseStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }));
            list.Add(new CaseStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }));
            list.Add(new CaseStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 5, 6, 7, 8, 9 }));
            list.Add(new CaseStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8, 9 }));
            return list.ToArray();
        }
    }

    public struct CaseStruct
    {
        public int amount;
        public int[] oldSelection;
        public int[] newSelection;
        public CaseStruct(int amount, int[] oldSelection, int[] newSelection)
        {
            this.amount = amount;
            this.oldSelection = oldSelection;
            this.newSelection = newSelection;
        }
    }
}
