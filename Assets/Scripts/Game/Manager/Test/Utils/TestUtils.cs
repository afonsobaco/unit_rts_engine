using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Manager;

namespace Tests.Utils
{
    public class TestUtils
    {

        public static HashSet<T> GetListByIndex<T>(int[] indexes, HashSet<T> mainList) where T : ISelectable
        {
            var list = new HashSet<T>();
            for (var i = 0; i < indexes.Length; i++)
            {
                if (indexes[i] < mainList.Count)
                    list.Add(mainList.ElementAt(indexes.ElementAt(i)));
            }
            return list;
        }

        public static HashSet<T> GetSomeObjects<T>(int qtt) where T : class
        {
            var list = new HashSet<T>();
            for (var i = 0; i < qtt; i++)
            {
                T item = Substitute.For<T>();
                ISelectable selectable = (ISelectable)item;
                selectable.Index = i;
                selectable.IsCompatible(Arg.Any<ISelectableObjectBehaviour>()).Returns((x) =>
                {
                    return ((ISelectable)x[0]).Index % 2 == 0;
                });
                list.Add(item);
            }
            return list;
        }

        public static string GetCaseName(SelectionStruct selectionStruct, ModifiersStruct modifiersStruct)
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
            if (!modifiersStruct.isAdditive && !modifiersStruct.isSameType) return "NO MODIFIERS | ";

            string name = "";
            if (modifiersStruct.isAdditive)
                name += "ADDITIVE | ";
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

            var first = new HashSet<int>(collection);
            var second = new HashSet<int>(otherCollection);

            HashSet<int> list = new HashSet<int>(first.ToList().FindAll(x => second.Contains(x)));
            if (list.Count == 0) return collectionName + " Does not contains ANY of " + otherCollectionName + "";
            if (list.Count < second.Count) return collectionName + " Contains SOME of " + otherCollectionName + "";
            return collectionName + " Contains ALL of " + otherCollectionName + "";
        }

        public static HashSet<CaseStruct> GetDefaultCases()
        {
            ModifiersStruct modifiersStruct = default;
            return GetSelectionCases(modifiersStruct, false);
        }

        public static HashSet<CaseStruct> GetCasesWithAdditionalInfo()
        {
            ModifiersStruct modifiersStruct = default;
            return GetSelectionCases(modifiersStruct, true);
        }


        public static HashSet<CaseStruct> GetCasesWithModifiers(ModifiersStruct modifiersStruct)
        {
            return GetSelectionCases(modifiersStruct, false);
        }

        public static HashSet<CaseStruct> GetCustomCases(ModifiersStruct modifiersStruct, bool additionalInfo)
        {
            HashSet<CaseStruct> caseStructs = new HashSet<CaseStruct>();
            foreach (var item in ModifiersList(modifiersStruct))
            {
                caseStructs.UnionWith(GetSelectionCases(item, additionalInfo));
            }
            return caseStructs;
        }

        private static HashSet<CaseStruct> GetSelectionCases(ModifiersStruct modifiersStruct, bool additionalInfo)
        {
            HashSet<CaseStruct> caseStructs = new HashSet<CaseStruct>();
            foreach (var selectionStruct in GetSelectionListWithAdditionalInfos(additionalInfo))
            {
                caseStructs.Add(new CaseStruct(selectionStruct, modifiersStruct, default, GetCaseName(selectionStruct, modifiersStruct)));
            }
            return caseStructs;
        }

        private static HashSet<SelectionStruct> GetSelectionListWithAdditionalInfos(bool additionalInfo)
        {
            return SelectionList(additionalInfo);
        }

        private static HashSet<ModifiersStruct> ModifiersList(ModifiersStruct modifiersStruct)
        {
            HashSet<ModifiersStruct> list = new HashSet<ModifiersStruct>();
            list.Add(new ModifiersStruct(false, false));
            list.Add(new ModifiersStruct(false, true));
            list.Add(new ModifiersStruct(true, false));
            list.Add(new ModifiersStruct(true, true));

            if (!modifiersStruct.isAdditive)
            {
                list.RemoveWhere(a => a.isAdditive);
            }
            if (!modifiersStruct.isSameType)
            {
                list.RemoveWhere(a => a.isSameType);
            }
            list.Add(new ModifiersStruct(false, false));
            return list;
        }

        private static HashSet<SelectionStruct> SelectionList()
        {
            return SelectionList(false);
        }
        private static HashSet<SelectionStruct> SelectionList(bool additionalInfo)
        {
            HashSet<SelectionStruct> list = new HashSet<SelectionStruct>();

            AdditionalInfo addInfo = new AdditionalInfo();
            if (additionalInfo)
            {
                addInfo = new AdditionalInfo()
                {
                    group_evens = new int[] { 0, 2, 4, 6, 8 },
                    group_odds = new int[] { 1, 3, 5, 7, 9 }
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

        public static HashSet<T> Shuffle<T>(IEnumerable<T> collection)
        {
            T[] list = collection.ToArray();
            System.Random rng = new System.Random();
            int n = list.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return new HashSet<T>(list);
        }
    }

    public struct CaseStruct
    {
        public SelectionStruct selection;
        public ModifiersStruct modifiers;

        public int[] result;

        public string name;

        public CaseStruct(SelectionStruct selection, ModifiersStruct modifiers, int[] result, string name)
        {
            this.selection = selection;
            this.modifiers = modifiers;
            this.result = result;
            this.name = name;
        }
    }

    public struct ModifiersStruct
    {
        public bool isAdditive;
        public bool isSameType;

        public ModifiersStruct(bool isAdditive, bool isSameType)
        {
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

        public SelectionTypeEnum type;
        public int[] group_evens;
        public int[] group_odds;
        public AdditionalInfo(SelectionTypeEnum type, int[] evens, int[] odds)
        {
            this.type = type;
            this.group_evens = evens;
            this.group_odds = odds;
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
