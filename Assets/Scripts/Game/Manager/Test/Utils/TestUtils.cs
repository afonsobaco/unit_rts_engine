using System.Linq;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using RTSEngine.Core;

namespace Tests.Utils
{
    public class TestUtils
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

        private static string GetCaseName(SelectionStruct selectionStruct, ModifiersStruct modifiersStruct)
        {
            string name = "";
            name += NameForModifiers(modifiersStruct);
            name += NameForCollectionLength(selectionStruct.oldSelection, "Old");
            name += NameForCollectionLength(selectionStruct.newSelection, "New");
            name += NameForCollectionContains(selectionStruct.oldSelection, selectionStruct.newSelection, "Old", "New");
            return name;
        }

        private static string NameForModifiers(ModifiersStruct modifiersStruct)
        {
            if (!modifiersStruct.isAdditive && !modifiersStruct.isPreSelection && !modifiersStruct.isSameType) return "NO MODIFIERS | ";

            string name = "";
            if (modifiersStruct.isAdditive)
                name += "ADDITIVE | ";
            if (modifiersStruct.isPreSelection)
                name += "PRESELECTION | ";
            if (modifiersStruct.isSameType)
                name += "SAMETYPE | ";
            return name;
        }

        private static string NameForCollectionLength(int[] collection, string collectionName)
        {
            if (collection.Length == 0)
                return "EMPTY " + collectionName + ", ";
            if (collection.Length == 1)
                return "SINGLE element in " + collectionName + ", ";
            return "MULTPLE element in " + collectionName + ", ";
        }

        private static string NameForCollectionContains(int[] collection, int[] otherCollection, string collectionName, string otherCollectionName)
        {
            if (collection.Length == 0) return "";

            var first = new List<int>(collection);
            var second = new List<int>(otherCollection);

            List<int> list = first.FindAll(x => second.Contains(x));
            if (list.Count == 0) return collectionName + " Does not contains ANY of " + otherCollectionName + "";
            if (list.Count < second.Count) return collectionName + " Contains SOME of " + otherCollectionName + "";
            return collectionName + " Contains ALL of " + otherCollectionName + "";
        }

        public static List<CaseStruct> GetCases()
        {
            ModifiersStruct modifiersStruct = default;
            return GetSelectionCases(modifiersStruct);
        }

        public static List<CaseStruct> GetCases(ModifiersStruct modifiersStruct)
        {
            List<CaseStruct> caseStructs = new List<CaseStruct>();
            foreach (var item in ModifiersList(modifiersStruct))
            {
                caseStructs.AddRange(GetSelectionCases(item));
            }
            return caseStructs;
        }

        private static List<CaseStruct> GetSelectionCases(ModifiersStruct modifiersStruct)
        {
            List<CaseStruct> caseStructs = new List<CaseStruct>();
            foreach (var selectionStruct in SelectionList())
            {
                caseStructs.Add(new CaseStruct(selectionStruct, modifiersStruct, GetCaseName(selectionStruct, modifiersStruct)));
            }
            return caseStructs;
        }

        private static List<ModifiersStruct> ModifiersList(ModifiersStruct modifiersStruct)
        {
            List<ModifiersStruct> list = new List<ModifiersStruct>();
            list.Add(new ModifiersStruct(true, true, true));
            list.Add(new ModifiersStruct(true, true, false));
            list.Add(new ModifiersStruct(true, false, true));
            list.Add(new ModifiersStruct(true, false, false));
            list.Add(new ModifiersStruct(false, true, true));
            list.Add(new ModifiersStruct(false, true, false));
            list.Add(new ModifiersStruct(false, false, true));
            list.Add(new ModifiersStruct(false, false, false));
            if (!modifiersStruct.isPreSelection)
            {
                list.RemoveAll(a => a.isPreSelection);
            }
            if (!modifiersStruct.isAdditive)
            {
                list.RemoveAll(a => a.isAdditive);
            }
            if (!modifiersStruct.isSameType)
            {
                list.RemoveAll(a => a.isSameType);
            }
            return list;
        }

        private static List<SelectionStruct> SelectionList()
        {
            List<SelectionStruct> list = new List<SelectionStruct>();

            list.Add(new SelectionStruct(10, new int[] { }, new int[] { }));
            list.Add(new SelectionStruct(10, new int[] { }, new int[] { 0 }));
            list.Add(new SelectionStruct(10, new int[] { }, new int[] { 0, 1, 2, 3, 4 }));
            list.Add(new SelectionStruct(10, new int[] { 0 }, new int[] { }));
            list.Add(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }));
            list.Add(new SelectionStruct(10, new int[] { 0 }, new int[] { 1 }));
            list.Add(new SelectionStruct(10, new int[] { 0 }, new int[] { 1, 2, 3, 4, 5 }));
            list.Add(new SelectionStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { }));
            list.Add(new SelectionStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }));
            list.Add(new SelectionStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }));
            list.Add(new SelectionStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }));
            list.Add(new SelectionStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 5, 6, 7, 8, 9 }));
            list.Add(new SelectionStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8, 9 }));

            return list;
        }

        public static List<T> Shuffle<T>(List<T> list)
        {
            System.Random rng = new System.Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }

    public struct CaseStruct
    {
        public SelectionStruct selection;
        public ModifiersStruct modifiers;

        public string name;

        public CaseStruct(SelectionStruct selection, ModifiersStruct modifiers, string name)
        {
            this.selection = selection;
            this.modifiers = modifiers;
            this.name = name;
        }
    }

    public struct ModifiersStruct
    {
        public bool isPreSelection;
        public bool isAdditive;
        public bool isSameType;

        public ModifiersStruct(bool isPreSelection, bool isAdditive, bool isSameType)
        {
            this.isPreSelection = isPreSelection;
            this.isAdditive = isAdditive;
            this.isSameType = isSameType;
        }
    }

    public struct SelectionStruct
    {
        public int mainListAmount;
        public int[] oldSelection;
        public int[] newSelection;

        public SelectionStruct(int mainListAmount, int[] oldSelection, int[] newSelection)
        {
            this.mainListAmount = mainListAmount;
            this.oldSelection = oldSelection;
            this.newSelection = newSelection;
        }
    }


    public struct ResultStruct
    {
        public int[] toBeAdded;
        public int[] toBeRemoved;

        public ResultStruct(int[] toBeAdded, int[] toBeRemoved)
        {
            this.toBeAdded = toBeAdded;
            this.toBeRemoved = toBeRemoved;
        }
    }


}
