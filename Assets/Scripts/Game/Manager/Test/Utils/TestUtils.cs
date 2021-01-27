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
                ISelectable item = Substitute.For<ISelectable>();
                item.Index = i;
                list.Add(item);
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
            return "MULTIPLE elements in " + collectionName + ", ";
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

        public static List<CaseStruct> GetDefaultCases()
        {
            ModifiersStruct modifiersStruct = default;
            return GetSelectionCases(modifiersStruct, false);
        }

        public static List<CaseStruct> GetCasesWithAdditionalInfo()
        {
            ModifiersStruct modifiersStruct = default;
            return GetSelectionCases(modifiersStruct, true);
        }


        public static List<CaseStruct> GetCasesWithModifiers(ModifiersStruct modifiersStruct)
        {
            return GetSelectionCases(modifiersStruct, false);
        }

        public static List<CaseStruct> GetCustomCases(ModifiersStruct modifiersStruct, bool additionalInfo)
        {
            List<CaseStruct> caseStructs = new List<CaseStruct>();
            foreach (var item in ModifiersList(modifiersStruct))
            {
                caseStructs.AddRange(GetSelectionCases(item, additionalInfo));
            }
            return caseStructs;
        }

        private static List<CaseStruct> GetSelectionCases(ModifiersStruct modifiersStruct, bool additionalInfo)
        {
            List<CaseStruct> caseStructs = new List<CaseStruct>();
            foreach (var selectionStruct in GetSelectionListWithAdditionalInfos(additionalInfo))
            {
                caseStructs.Add(new CaseStruct(selectionStruct, modifiersStruct, GetCaseName(selectionStruct, modifiersStruct)));
            }
            return caseStructs;
        }

        private static List<SelectionStruct> GetSelectionListWithAdditionalInfos(bool additionalInfo)
        {
            return SelectionList(additionalInfo);
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
            return SelectionList(false);
        }
        private static List<SelectionStruct> SelectionList(bool additionalInfo)
        {
            List<SelectionStruct> list = new List<SelectionStruct>();

            AdditionalInfo addInfo = new AdditionalInfo();
            if (additionalInfo)
            {
                addInfo = new AdditionalInfo()
                {
                    group_a = new int[] { 0, 2, 4, 6, 8 },
                    group_b = new int[] { 1, 3, 5, 7, 9 }
                };
            }

            list.Add(new SelectionStruct(10, new int[] { }, new int[] { }, addInfo));
            list.Add(new SelectionStruct(10, new int[] { }, new int[] { 0 }, addInfo));
            list.Add(new SelectionStruct(10, new int[] { }, new int[] { 0, 1, 2, 3, 4 }, addInfo));
            list.Add(new SelectionStruct(10, new int[] { 0 }, new int[] { }, addInfo));
            list.Add(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, addInfo));
            list.Add(new SelectionStruct(10, new int[] { 0 }, new int[] { 1 }, addInfo));
            list.Add(new SelectionStruct(10, new int[] { 0 }, new int[] { 1, 2, 3, 4, 5 }, addInfo));
            list.Add(new SelectionStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { }, addInfo));
            list.Add(new SelectionStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }, addInfo));
            list.Add(new SelectionStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }, addInfo));
            list.Add(new SelectionStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }, addInfo));
            list.Add(new SelectionStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 5, 6, 7, 8, 9 }, addInfo));
            list.Add(new SelectionStruct(10, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8, 9 }, addInfo));

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

        public AdditionalInfo additionalInfo;

        public SelectionStruct(int mainListAmount, int[] oldSelection, int[] newSelection, AdditionalInfo additionalInfo)
        {
            this.mainListAmount = mainListAmount;
            this.oldSelection = oldSelection;
            this.newSelection = newSelection;
            this.additionalInfo = additionalInfo;
        }
    }

    public struct AdditionalInfo
    {
        public int[] group_a;
        public int[] group_b;
        public AdditionalInfo(int[] group_a, int[] group_b)
        {
            this.group_a = group_a;
            this.group_b = group_b;
        }
    }


    public struct ResultStruct
    {
        public int[] toBeAdded;

        public ResultStruct(int[] toBeAdded)
        {
            this.toBeAdded = toBeAdded;
        }
    }


}
