using NSubstitute;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tests.Manager
{

    [TestFixture]
    public class SelectionManagerTest
    {
        private SelectionManager manager;

        [SetUp]
        public void SetUp()
        {
            manager = Substitute.ForPartsOf<SelectionManager>();
            var so = Substitute.For<IRuntimeSet<ISelectableObject>>();
            manager.SetMainList(so.GetAllItems());
            manager.SetGroupNumperPressed(0);
            manager.When(x => x.GetObjectClicked()).DoNotCallBase();
            manager.GetObjectClicked().Returns(x => null);
        }

        private Dictionary<int, HashSet<ISelectableObject>> GetDefaultDictionary(params HashSet<ISelectableObject>[] parameters)
        {
            Dictionary<int, HashSet<ISelectableObject>> result = new Dictionary<int, HashSet<ISelectableObject>>();
            for (var i = 0; i < parameters.Length; i++)
            {
                result[i + 1] = parameters[i];
            }
            return result;
        }

        private HashSet<ISelectableObject> GetSelectionListByParams(params int[] parameters)
        {
            HashSet<ISelectableObject> result = new HashSet<ISelectableObject>();
            for (var i = 0; i < parameters.Length; i++)
            {
                ISelectableObject item = SelectionManagerTestUtils.CreateATestableObject(parameters[i]);
                item.Index = parameters[i];
                result.Add(item);
            }
            return result;
        }

        private HashSet<ISelectableObject> GetSelectionListFromMainList(HashSet<ISelectableObject> mainList, params int[] parameters)
        {
            HashSet<ISelectableObject> result = new HashSet<ISelectableObject>();
            for (var i = 0; i < parameters.Length; i++)
            {
                result.Add(mainList.ElementAt(parameters[i]));
            }
            return result;
        }

        private void AssertArgs(SelectionArguments expected, SelectionArguments actual)
        {
            CollectionAssert.AreEquivalent(expected.OldSelection, actual.OldSelection);
            CollectionAssert.AreEquivalent(expected.NewSelection, actual.NewSelection);
            CollectionAssert.AreEquivalent(expected.MainList, actual.MainList);
            CollectionAssert.AreEquivalent(expected.ToBeAdded, actual.ToBeAdded);
        }


        // TESTS ------------------------------------------------

        [Test]
        public void ShouldReturnDragSelectionType()
        {
            var type = manager.GetSelectionType();

            Assert.AreEqual(SelectionTypeEnum.DRAG, type);
        }

        [Test]
        public void ShouldReturnClickSelectionType()
        {
            ISelectableObject clicked = SelectionManagerTestUtils.CreateATestableObject(0);
            manager.GetObjectClicked().Returns(clicked);

            var type = manager.GetSelectionType();

            Assert.AreEqual(SelectionTypeEnum.CLICK, type);
            Assert.AreEqual(clicked, manager.GetObjectClicked());
        }

        [Test]
        public void ShouldReturnKeySelectionType()
        {
            manager.SetGroupNumperPressed(1);
            var type = manager.GetSelectionType();
            Assert.AreEqual(SelectionTypeEnum.KEY, type);
        }


        [Test]
        public void ShouldGetSpecificGroup()
        {
            int groupId = 1;

            var expected = new HashSet<ISelectableObject>();
            expected.Add(SelectionManagerTestUtils.CreateATestableObject(0));

            manager.When(x => x.GetAllGroupSets()).DoNotCallBase();
            manager.GetAllGroupSets().Returns(GetDefaultDictionary(expected));

            var collection = manager.GetGroupSet(groupId);

            CollectionAssert.AreEquivalent(expected, collection);
        }

        [Test]
        public void ShouldGetEmptyGroupWhenGroupDoesNotExist()
        {
            int groupId = 1;

            manager.When(x => x.GetAllGroupSets()).DoNotCallBase();
            manager.GetAllGroupSets().Returns(GetDefaultDictionary());

            var collection = manager.GetGroupSet(groupId);

            CollectionAssert.IsEmpty(collection);
        }

        [Test]
        public void ShouldReturnNewSelectionOnClick()
        {
            ISelectableObject expected = SelectionManagerTestUtils.CreateATestableObject(0);
            manager.SetClicked(expected);
            manager.SetSelectionType(SelectionTypeEnum.CLICK);

            var selection = manager.GetSelectionBySelectionType();

            Assert.True(selection.Contains(expected));
            Assert.AreEqual(1, selection.Count);
        }

        [Test]
        public void ShouldReturnNewSelectionOnDrag()
        {
            manager.SetSelectionType(SelectionTypeEnum.DRAG);

            HashSet<ISelectableObject> expected = GetSelectionListByParams(0);

            manager.When(x => x.GetDragSelection()).DoNotCallBase();
            manager.GetDragSelection().Returns(expected);

            var selection = manager.GetSelectionBySelectionType();

            CollectionAssert.AreEquivalent(expected, selection);
        }

        [Test]
        public void ShouldReturnNewSelectionOnKey()
        {
            manager.SetSelectionType(SelectionTypeEnum.KEY);

            manager.When(x => x.GetGroupSet(Arg.Any<int>())).DoNotCallBase();
            HashSet<ISelectableObject> expected = GetSelectionListByParams(0);
            manager.GetGroupSet(Arg.Any<int>()).Returns(expected);

            var selection = manager.GetSelectionBySelectionType();

            CollectionAssert.AreEquivalent(expected, selection);
        }

        [Test]
        public void ShouldReturnEmptySelectionWhenSelectionOrClickOnEmptySpace()
        {
            manager.SetSelectionType(SelectionTypeEnum.DRAG);

            manager.When(x => x.GetDragSelection()).DoNotCallBase();
            manager.GetDragSelection().Returns(new HashSet<ISelectableObject>());

            var selection = manager.GetSelectionBySelectionType();
            CollectionAssert.IsEmpty(selection);
        }


        [Test]
        public void ShouldReturnSelectionArgs()
        {
            HashSet<ISelectableObject> mainList = GetSelectionListByParams(0, 1, 2, 3);
            HashSet<ISelectableObject> newSelection = GetSelectionListFromMainList(mainList, 0);
            HashSet<ISelectableObject> currentSelection = GetSelectionListFromMainList(mainList, 0);
            manager.SetMainList(mainList);
            manager.SetCurrentSelection(currentSelection);
            manager.SetDoubleClick(false);

            var args = manager.GetSelectionArgs(newSelection);

            SelectionArguments expected = new SelectionArguments(currentSelection, newSelection, mainList);
            AssertArgs(args, expected);
        }

        [Test]
        public void ShouldRemoveClickedFromSelectionArgsWhenDoubleClick()
        {
            HashSet<ISelectableObject> mainList = GetSelectionListByParams(0, 1, 2, 3);
            HashSet<ISelectableObject> newSelection = GetSelectionListFromMainList(mainList, 0);
            HashSet<ISelectableObject> currentSelection = GetSelectionListFromMainList(mainList, 0);
            manager.SetMainList(mainList);
            manager.SetCurrentSelection(currentSelection);
            manager.SetLastClicked(mainList.ElementAt(0));
            manager.SetClicked(mainList.ElementAt(0));
            manager.SetDoubleClick(true);

            var args = manager.GetSelectionArgs(newSelection);

            SelectionArguments expected = new SelectionArguments(new HashSet<ISelectableObject>(), newSelection, mainList);
            AssertArgs(args, expected);
            Assert.True(manager.IsSameType());
        }

        [Test]
        public void ShouldAddClickedFromSelectionArgsWhenDoubleClick()
        {
            HashSet<ISelectableObject> mainList = GetSelectionListByParams(0, 1, 2, 3);
            HashSet<ISelectableObject> newSelection = GetSelectionListFromMainList(mainList, 0);
            HashSet<ISelectableObject> currentSelection = new HashSet<ISelectableObject>();
            manager.SetMainList(mainList);
            manager.SetCurrentSelection(currentSelection);
            manager.SetLastClicked(mainList.ElementAt(0));
            manager.SetClicked(mainList.ElementAt(0));
            manager.SetDoubleClick(true);

            var args = manager.GetSelectionArgs(newSelection);

            HashSet<ISelectableObject> expectedCurrent = GetSelectionListFromMainList(mainList, 0);
            SelectionArguments expected = new SelectionArguments(expectedCurrent, newSelection, mainList);
            AssertArgs(args, expected);
            Assert.True(manager.IsSameType());
        }

        private static void GetModsToTest(SelectionTypeEnum selectionType, int howManyAll, int howManyClick, int howManyDrag, int howManyKey, List<ISelectionModifier> mods, List<ISelectionModifier> expectedMods)
        {
            List<ISelectionModifier> allMods = SelectionManagerTestUtils.GetSomeModsFromType(howManyAll, SelectionTypeEnum.ANY);
            List<ISelectionModifier> clickMods = SelectionManagerTestUtils.GetSomeModsFromType(howManyClick, SelectionTypeEnum.CLICK);
            List<ISelectionModifier> dragMods = SelectionManagerTestUtils.GetSomeModsFromType(howManyDrag, SelectionTypeEnum.DRAG);
            List<ISelectionModifier> keyMods = SelectionManagerTestUtils.GetSomeModsFromType(howManyKey, SelectionTypeEnum.KEY);
            expectedMods.AddRange(allMods);
            switch (selectionType)
            {
                case SelectionTypeEnum.CLICK:
                    expectedMods.AddRange(clickMods);
                    break;
                case SelectionTypeEnum.DRAG:
                    expectedMods.AddRange(dragMods);
                    break;
                case SelectionTypeEnum.KEY:
                    expectedMods.AddRange(keyMods);
                    break;
                default:
                    break;
            }

            mods.AddRange(allMods);
            mods.AddRange(clickMods);
            mods.AddRange(dragMods);
            mods.AddRange(keyMods);
        }

        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                yield return new TestCaseData(SelectionTypeEnum.ANY, 0, 0, 0, 0);
                yield return new TestCaseData(SelectionTypeEnum.ANY, 2, 3, 2, 1);
                yield return new TestCaseData(SelectionTypeEnum.CLICK, 0, 0, 0, 0);
                yield return new TestCaseData(SelectionTypeEnum.CLICK, 2, 3, 2, 1);
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 0, 0, 0, 0);
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 2, 3, 2, 1);
                yield return new TestCaseData(SelectionTypeEnum.KEY, 0, 0, 0, 0);
                yield return new TestCaseData(SelectionTypeEnum.KEY, 2, 3, 2, 1);

            }
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifiers(SelectionTypeEnum selectionType, int howManyAll, int howManyClick, int howManyDrag, int howManyKey)
        {

            List<ISelectionModifier> mods = new List<ISelectionModifier>();
            List<ISelectionModifier> expectedMods = new List<ISelectionModifier>();
            GetModsToTest(selectionType, howManyAll, howManyClick, howManyDrag, howManyKey, mods, expectedMods);
            manager.When(x => x.GetModifiersToBeApplied(Arg.Any<SelectionTypeEnum>())).DoNotCallBase();
            manager.GetModifiersToBeApplied(Arg.Any<SelectionTypeEnum>()).Returns(expectedMods);
            SelectionArguments args = new SelectionArguments(new HashSet<ISelectableObject>(), new HashSet<ISelectableObject>(), new HashSet<ISelectableObject>());

            var result = manager.ApplyModifiers(args);

            foreach (var mod in mods)
            {
                if (mod.Type == selectionType || mod.Type == SelectionTypeEnum.ANY)
                {
                    mod.ReceivedWithAnyArgs().Apply(default);
                }
                else
                {
                    mod.DidNotReceiveWithAnyArgs().Apply(default);
                }
            }
        }


        [Test]
        public void ShouldFinalizeSelection()
        {
            HashSet<ISelectableObject> mainList = GetSelectionListByParams(0, 1, 2, 3, 4);
            HashSet<ISelectableObject> newSelection = GetSelectionListFromMainList(mainList, 0, 1, 2);
            HashSet<ISelectableObject> currentSelection = GetSelectionListFromMainList(mainList, 2, 3);
            foreach (var item in currentSelection)
            {
                item.IsSelected = true;
            }

            SelectionArguments args = new SelectionArguments(currentSelection, newSelection, mainList);
            var result = manager.GetFinalSelection(args);

            CollectionAssert.AreEquivalent(args.ToBeAdded, result);

            foreach (var item in currentSelection)
            {
                Assert.AreEqual(item.IsSelected, newSelection.Contains(item));
                Assert.False(item.IsPreSelected);
            }

            foreach (var item in result)
            {
                Assert.True(item.IsSelected);
                Assert.False(item.IsPreSelected);
            }
        }

        [Test]
        public void ShouldFinalizePreSelection()
        {
            manager.SetIsPreSelection(true);
            HashSet<ISelectableObject> mainList = GetSelectionListByParams(0, 1, 2, 3, 4);
            HashSet<ISelectableObject> newSelection = GetSelectionListFromMainList(mainList, 0, 1, 2);
            HashSet<ISelectableObject> currentSelection = GetSelectionListFromMainList(mainList, 2, 3);
            foreach (var item in currentSelection)
            {
                item.IsSelected = true;
            }

            SelectionArguments args = new SelectionArguments(currentSelection, newSelection, mainList);
            var result = manager.GetFinalSelection(args);

            CollectionAssert.AreEquivalent(args.ToBeAdded, result);

            foreach (var item in currentSelection)
            {
                Assert.True(item.IsSelected);
                Assert.AreEqual(item.IsPreSelected, newSelection.Contains(item));
            }

            foreach (var item in result)
            {
                Assert.AreEqual(item.IsSelected, currentSelection.Contains(item));
                Assert.True(item.IsPreSelected);
            }


        }

        [Test]
        public void ShouldUpdateNewAndOldPreSelectionStatus()
        {
            HashSet<ISelectableObject> mainList = GetSelectionListByParams(0, 1, 2, 3, 4);
            HashSet<ISelectableObject> newSelection = GetSelectionListFromMainList(mainList, 0, 1, 2);
            HashSet<ISelectableObject> preSelection = GetSelectionListFromMainList(mainList, 2, 3);
            manager.SetPreSelection(preSelection);
            foreach (var item in preSelection)
            {
                item.IsPreSelected = true;
            }

            var result = manager.GetUpdatedPreSelection(newSelection);

            CollectionAssert.AreEquivalent(newSelection, result);

            foreach (var item in preSelection)
            {
                Assert.AreEqual(item.IsPreSelected, result.Contains(item));
            }

            foreach (var item in result)
            {
                Assert.True(item.IsPreSelected);
            }
        }


        [Test]
        public void ShouldUpdateNewAndOldCurrentSelectionStatus()
        {
            HashSet<ISelectableObject> mainList = GetSelectionListByParams(0, 1, 2, 3, 4);
            HashSet<ISelectableObject> newSelection = GetSelectionListFromMainList(mainList, 0, 1, 2);
            HashSet<ISelectableObject> currentSelection = GetSelectionListFromMainList(mainList, 2, 3);
            manager.SetCurrentSelection(currentSelection);
            foreach (var item in currentSelection)
            {
                item.IsSelected = true;
            }

            var result = manager.GetUpdatedCurrentSelection(newSelection);

            CollectionAssert.AreEquivalent(newSelection, result);

            foreach (var item in currentSelection)
            {
                Assert.AreEqual(item.IsSelected, result.Contains(item));
            }

            foreach (var item in result)
            {
                Assert.True(item.IsSelected);
            }
        }

        [Test]
        public void ShouldDoPreSelectionAddition()
        {
            HashSet<ISelectableObject> mainList = GetSelectionListByParams(0, 1, 2, 3, 4);
            HashSet<ISelectableObject> newSelection = GetSelectionListFromMainList(mainList, 0, 1, 2);
            HashSet<ISelectableObject> preSelection = GetSelectionListFromMainList(mainList);
            manager.SetPreSelection(preSelection);
            foreach (var item in preSelection)
            {
                item.IsPreSelected = true;
            }
            manager.When(x => x.PerformSelection(Arg.Any<Vector2>())).DoNotCallBase();
            manager.PerformSelection(Arg.Any<Vector2>()).Returns(newSelection);

            manager.DoPreSelection(new Vector2(0, 1));

            CollectionAssert.AreEquivalent(manager.GetPreSelection(), newSelection);

            foreach (var item in manager.GetPreSelection())
            {
                Assert.AreEqual(item.IsPreSelected, newSelection.Contains(item));
                Assert.False(item.IsSelected);
            }

            foreach (var item in manager.GetPreSelection())
            {
                Assert.True(item.IsPreSelected);
                Assert.False(item.IsSelected);
            }
        }

        [Test]
        public void ShouldDoPreSelectionDeletion()
        {
            HashSet<ISelectableObject> mainList = GetSelectionListByParams(0, 1, 2, 3, 4);
            HashSet<ISelectableObject> newSelection = GetSelectionListFromMainList(mainList, 0, 1, 2);
            HashSet<ISelectableObject> preSelection = GetSelectionListFromMainList(mainList, 0, 3, 4);
            manager.SetPreSelection(preSelection);
            foreach (var item in preSelection)
            {
                item.IsPreSelected = true;
            }
            manager.When(x => x.PerformSelection(Arg.Any<Vector2>())).DoNotCallBase();
            manager.PerformSelection(Arg.Any<Vector2>()).Returns(newSelection);

            manager.DoPreSelection(new Vector2(0, 1));

            CollectionAssert.AreEquivalent(manager.GetPreSelection(), newSelection);

            foreach (var item in manager.GetPreSelection())
            {
                Assert.AreEqual(item.IsPreSelected, newSelection.Contains(item));
                Assert.False(item.IsSelected);
            }

        }


        [Test]
        public void ShouldAddToSelectionOrderedWhenNewSelectionIsSent()
        {
            HashSet<ISelectableObject> mainList = GetSelectionListByParams(0, 1, 2, 3, 4);
            HashSet<ISelectableObject> newSelection = GetSelectionListFromMainList(mainList, 0, 1, 2, 3, 4);
            HashSet<ISelectableObject> preSelection = GetSelectionListFromMainList(mainList, 0, 1, 2);
            manager.SetPreSelection(preSelection);

            var result = manager.OrderSelection(newSelection);

            CollectionAssert.AreEquivalent(newSelection, result);
        }

        [Test]
        public void ShouldRemoveToSelectionOrderedWhenNewSelectionIsSent()
        {
            HashSet<ISelectableObject> mainList = GetSelectionListByParams(0, 1, 2, 3, 4);
            HashSet<ISelectableObject> newSelection = GetSelectionListFromMainList(mainList, 0, 1);
            HashSet<ISelectableObject> preSelection = GetSelectionListFromMainList(mainList, 0, 1, 2, 3, 4);
            manager.SetPreSelection(preSelection);

            var result = manager.OrderSelection(newSelection);

            CollectionAssert.AreEquivalent(newSelection, result);
        }

    }
}
