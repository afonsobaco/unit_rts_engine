using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using RTSEngine.Manager;
using RTSEngine.Core;
using NSubstitute;
using Tests.Utils;
using System;

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
            var so = Substitute.For<IRuntimeSet<ISelectableObjectBehaviour>>();
            manager.SetMainList(so.GetList());
            manager.SetGroupNumperPressed(0);
            manager.When(x => x.GetObjectClicked()).DoNotCallBase();
            manager.GetObjectClicked().Returns(x => null);
        }

        private Dictionary<int, HashSet<ISelectableObjectBehaviour>> GetDefaultDictionary(params HashSet<ISelectableObjectBehaviour>[] parameters)
        {
            Dictionary<int, HashSet<ISelectableObjectBehaviour>> result = new Dictionary<int, HashSet<ISelectableObjectBehaviour>>();
            for (var i = 0; i < parameters.Length; i++)
            {
                result[i + 1] = parameters[i];
            }
            return result;
        }

        private HashSet<ISelectableObjectBehaviour> GetSelectionListByParams(params int[] parameters)
        {
            HashSet<ISelectableObjectBehaviour> result = new HashSet<ISelectableObjectBehaviour>();
            for (var i = 0; i < parameters.Length; i++)
            {
                result.Add(SelectionManagerTestUtils.CreateATestableObject(parameters[i]));
            }
            return result;
        }

        private HashSet<ISelectableObjectBehaviour> GetSelectionListFromMainList(HashSet<ISelectableObjectBehaviour> mainList, params int[] parameters)
        {
            HashSet<ISelectableObjectBehaviour> result = new HashSet<ISelectableObjectBehaviour>();
            for (var i = 0; i < parameters.Length; i++)
            {
                result.Add(mainList.ElementAt(parameters[i]));
            }
            return result;
        }

        private void AssertArgs(SelectionArgsXP expected, SelectionArgsXP actual)
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
            ISelectableObjectBehaviour clicked = SelectionManagerTestUtils.CreateATestableObject(0);
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

            var expected = new HashSet<ISelectableObjectBehaviour>();
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
            ISelectableObjectBehaviour expected = SelectionManagerTestUtils.CreateATestableObject(0);
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

            HashSet<ISelectableObjectBehaviour> expected = GetSelectionListByParams(0);

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
            HashSet<ISelectableObjectBehaviour> expected = GetSelectionListByParams(0);
            manager.GetGroupSet(Arg.Any<int>()).Returns(expected);

            var selection = manager.GetSelectionBySelectionType();

            CollectionAssert.AreEquivalent(expected, selection);
        }

        [Test]
        public void ShouldReturnEmptySelectionWhenSelectionOrClickOnEmptySpace()
        {
            manager.SetSelectionType(SelectionTypeEnum.DRAG);

            manager.When(x => x.GetDragSelection()).DoNotCallBase();
            manager.GetDragSelection().Returns(new HashSet<ISelectableObjectBehaviour>());

            var selection = manager.GetSelectionBySelectionType();
            CollectionAssert.IsEmpty(selection);
        }


        [Test]
        public void ShouldReturnSelectionArgs()
        {
            HashSet<ISelectableObjectBehaviour> mainList = GetSelectionListByParams(0, 1, 2, 3);
            HashSet<ISelectableObjectBehaviour> newSelection = GetSelectionListFromMainList(mainList, 0);
            HashSet<ISelectableObjectBehaviour> currentSelection = GetSelectionListFromMainList(mainList, 0);
            manager.SetMainList(mainList);
            manager.SetCurrentSelection(currentSelection);
            manager.SetDoubleClick(false);

            var args = manager.GetSelectionArgs(newSelection);

            SelectionArgsXP expected = new SelectionArgsXP(currentSelection, newSelection, mainList);
            AssertArgs(args, expected);
        }

        [Test]
        public void ShouldRemoveClickedFromSelectionArgsWhenDoubleClick()
        {
            HashSet<ISelectableObjectBehaviour> mainList = GetSelectionListByParams(0, 1, 2, 3);
            HashSet<ISelectableObjectBehaviour> newSelection = GetSelectionListFromMainList(mainList, 0);
            HashSet<ISelectableObjectBehaviour> currentSelection = GetSelectionListFromMainList(mainList, 0);
            manager.SetMainList(mainList);
            manager.SetCurrentSelection(currentSelection);
            manager.SetLastClicked(mainList.ElementAt(0));
            manager.SetClicked(mainList.ElementAt(0));
            manager.SetDoubleClick(true);

            var args = manager.GetSelectionArgs(newSelection);

            SelectionArgsXP expected = new SelectionArgsXP(new HashSet<ISelectableObjectBehaviour>(), newSelection, mainList);
            AssertArgs(args, expected);
            Assert.True(manager.IsSameType());
        }

        [Test]
        public void ShouldAddClickedFromSelectionArgsWhenDoubleClick()
        {
            HashSet<ISelectableObjectBehaviour> mainList = GetSelectionListByParams(0, 1, 2, 3);
            HashSet<ISelectableObjectBehaviour> newSelection = GetSelectionListFromMainList(mainList, 0);
            HashSet<ISelectableObjectBehaviour> currentSelection = new HashSet<ISelectableObjectBehaviour>();
            manager.SetMainList(mainList);
            manager.SetCurrentSelection(currentSelection);
            manager.SetLastClicked(mainList.ElementAt(0));
            manager.SetClicked(mainList.ElementAt(0));
            manager.SetDoubleClick(true);

            var args = manager.GetSelectionArgs(newSelection);

            HashSet<ISelectableObjectBehaviour> expectedCurrent = GetSelectionListFromMainList(mainList, 0);
            SelectionArgsXP expected = new SelectionArgsXP(expectedCurrent, newSelection, mainList);
            AssertArgs(args, expected);
            Assert.True(manager.IsSameType());
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifiers(SelectionTypeEnum selectionType, int howManyAll, int howManyClick, int howManyDrag, int howManyKey)
        {

            HashSet<IBaseSelectionMod> mods = new HashSet<IBaseSelectionMod>();
            HashSet<IBaseSelectionMod> expectedMods = new HashSet<IBaseSelectionMod>();
            GetModsToTest(selectionType, howManyAll, howManyClick, howManyDrag, howManyKey, mods, expectedMods);
            manager.When(x => x.GetModifiersToBeApplied(Arg.Any<SelectionTypeEnum>())).DoNotCallBase();
            manager.GetModifiersToBeApplied(Arg.Any<SelectionTypeEnum>()).Returns(expectedMods);
            SelectionArgsXP args = new SelectionArgsXP(new HashSet<ISelectableObjectBehaviour>(), new HashSet<ISelectableObjectBehaviour>(), new HashSet<ISelectableObjectBehaviour>());

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

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldNotApplyInactiveModifiers(SelectionTypeEnum selectionType, int howManyAll, int howManyClick, int howManyDrag, int howManyKey)
        {
            HashSet<IBaseSelectionMod> mods = new HashSet<IBaseSelectionMod>();
            HashSet<IBaseSelectionMod> expectedMods = new HashSet<IBaseSelectionMod>();

            GetModsToTest(selectionType, howManyAll, howManyClick, howManyDrag, howManyKey, mods, expectedMods);
            if (expectedMods.Count > 0)
            {
                expectedMods.First().Active = false;
            }

            manager.When(x => x.GetModifiersToBeApplied(Arg.Any<SelectionTypeEnum>())).DoNotCallBase();
            manager.GetModifiersToBeApplied(Arg.Any<SelectionTypeEnum>()).Returns(expectedMods);

            SelectionArgsXP args = new SelectionArgsXP(new HashSet<ISelectableObjectBehaviour>(), new HashSet<ISelectableObjectBehaviour>(), new HashSet<ISelectableObjectBehaviour>());

            var result = manager.ApplyModifiers(args);
            foreach (var mod in mods)
            {
                if ((mod.Type == selectionType || mod.Type == SelectionTypeEnum.ANY) && mod.Active)
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
            HashSet<ISelectableObjectBehaviour> mainList = GetSelectionListByParams(0, 1, 2, 3, 4);
            HashSet<ISelectableObjectBehaviour> newSelection = GetSelectionListFromMainList(mainList, 0, 1, 2);
            HashSet<ISelectableObjectBehaviour> currentSelection = GetSelectionListFromMainList(mainList, 2, 3);
            foreach (var item in currentSelection)
            {
                item.IsSelected = true;
            }

            SelectionArgsXP args = new SelectionArgsXP(currentSelection, newSelection, mainList);
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
            HashSet<ISelectableObjectBehaviour> mainList = GetSelectionListByParams(0, 1, 2, 3, 4);
            HashSet<ISelectableObjectBehaviour> newSelection = GetSelectionListFromMainList(mainList, 0, 1, 2);
            HashSet<ISelectableObjectBehaviour> currentSelection = GetSelectionListFromMainList(mainList, 2, 3);
            foreach (var item in currentSelection)
            {
                item.IsSelected = true;
            }

            SelectionArgsXP args = new SelectionArgsXP(currentSelection, newSelection, mainList);
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
            HashSet<ISelectableObjectBehaviour> mainList = GetSelectionListByParams(0, 1, 2, 3, 4);
            HashSet<ISelectableObjectBehaviour> newSelection = GetSelectionListFromMainList(mainList, 0, 1, 2);
            HashSet<ISelectableObjectBehaviour> preSelection = GetSelectionListFromMainList(mainList, 2, 3);
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
            HashSet<ISelectableObjectBehaviour> mainList = GetSelectionListByParams(0, 1, 2, 3, 4);
            HashSet<ISelectableObjectBehaviour> newSelection = GetSelectionListFromMainList(mainList, 0, 1, 2);
            HashSet<ISelectableObjectBehaviour> currentSelection = GetSelectionListFromMainList(mainList, 2, 3);
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

        private static void GetModsToTest(SelectionTypeEnum selectionType, int howManyAll, int howManyClick, int howManyDrag, int howManyKey, HashSet<IBaseSelectionMod> mods, HashSet<IBaseSelectionMod> expectedMods)
        {
            HashSet<IBaseSelectionMod> allMods = SelectionManagerTestUtils.GetSomeModsFromType(howManyAll, SelectionTypeEnum.ANY);
            HashSet<IBaseSelectionMod> clickMods = SelectionManagerTestUtils.GetSomeModsFromType(howManyClick, SelectionTypeEnum.CLICK);
            HashSet<IBaseSelectionMod> dragMods = SelectionManagerTestUtils.GetSomeModsFromType(howManyDrag, SelectionTypeEnum.DRAG);
            HashSet<IBaseSelectionMod> keyMods = SelectionManagerTestUtils.GetSomeModsFromType(howManyKey, SelectionTypeEnum.KEY);
            expectedMods.UnionWith(allMods);
            switch (selectionType)
            {
                case SelectionTypeEnum.CLICK:
                    expectedMods.UnionWith(clickMods);
                    break;
                case SelectionTypeEnum.DRAG:
                    expectedMods.UnionWith(dragMods);
                    break;
                case SelectionTypeEnum.KEY:
                    expectedMods.UnionWith(keyMods);
                    break;
                default:
                    break;
            }

            mods.UnionWith(allMods);
            mods.UnionWith(clickMods);
            mods.UnionWith(dragMods);
            mods.UnionWith(keyMods);
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

    }



}
