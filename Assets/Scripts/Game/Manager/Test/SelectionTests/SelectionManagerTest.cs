using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using RTSEngine.Manager;
using RTSEngine.Core;
using NSubstitute;

namespace Tests.Manager
{

    [TestFixture]
    public class SelectionManagerTest
    {
        private SelectionManager manager;

        [SetUp]
        public void SetUp()
        {
            manager = GetSelectionManager();
        }

        [Test]
        public void ShouldReturnDragSelectionType()
        {
            PrepareForDrag();
            var type = manager.GetSelectionType();
            Assert.AreEqual(SelectionTypeEnum.DRAG, type);
        }

        [Test]
        public void ShouldReturnClickSelectionType()
        {
            PrepareForClick();
            var type = manager.GetSelectionType();
            Assert.AreEqual(SelectionTypeEnum.CLICK, type);
        }

        [Test]
        public void ShouldReturnKeySelectionType()
        {
            PrepareForKey(1);
            var type = manager.GetSelectionType();
            Assert.AreEqual(SelectionTypeEnum.KEY, type);
        }

        [Test]
        public void ShouldReturnTrueWhenKeyPressedIsGreaterThanZero()
        {
            manager.KeyPressed = 1;
            var value = manager.IsKey();
            Assert.True(value);
        }

        [Test]
        public void ShouldReturnFalseWhenKeyPressedIsLessOrEqualsZero()
        {
            manager.KeyPressed = 0;
            var value = manager.IsKey();
            Assert.False(value);
        }

        [Test]
        public void ShouldReturnFalseWhenNoObjectClicked()
        {
            manager.When(x => x.GetObjectClicked()).DoNotCallBase();
            manager.GetObjectClicked().Returns(x => null);
            var value = manager.IsClick();
            Assert.False(value);
        }

        [Test]
        public void ShouldReturnTrueWhenObjectClicked()
        {
            PrepareForClick();
            var value = manager.IsClick();
            Assert.True(value);
        }


        [Test]
        public void ShouldReturnNewSelectionOnClick()
        {
            var expected = PrepareForClick();

            var selection = manager.GetNewSelection();

            Assert.True(selection.Contains(expected));
            Assert.AreEqual(1, selection.Count);
        }



        [Test]
        public void ShouldAddToSpecificGroup()
        {
            int groupId = 1;
            manager.CurrentSelection = new List<ISelectable>();
            manager.CurrentSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            manager.SetGroup(groupId);
            List<ISelectable> collection = manager.Groups[groupId];
            CollectionAssert.IsNotEmpty(collection);
            CollectionAssert.AreEquivalent(manager.CurrentSelection, collection);
        }

        [Test]
        public void ShouldClearSpecificGroup()
        {
            int groupId = 1;
            manager.CurrentSelection = new List<ISelectable>();
            manager.SetGroup(groupId);
            List<ISelectable> collection = manager.Groups[groupId];
            CollectionAssert.IsEmpty(collection);
            CollectionAssert.AreEquivalent(manager.CurrentSelection, collection);
        }

        [Test]
        public void ShouldGetSpecificGroup()
        {
            int groupId = 1;
            var expected = new List<ISelectable>();
            expected.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            manager.Groups[1] = expected;

            var collection = manager.GetGroup(groupId);

            CollectionAssert.IsNotEmpty(collection);
            CollectionAssert.AreEquivalent(expected, collection);
        }

        [Test]
        public void ShouldReturnEmptyWhenGroupKeyNotFound()
        {
            int groupId = 1;

            var collection = manager.GetGroup(groupId);

            CollectionAssert.IsEmpty(collection);
        }

        [Test]
        public void ShouldReturnSelectionWhenDragOnScreenSpace()
        {
            PrepareForDrag();

            List<ISelectable> expected = new List<ISelectable>();
            expected.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            manager.When(x => x.GetDragSelection()).DoNotCallBase();
            manager.GetDragSelection().Returns(expected);

            var selection = manager.GetNewSelection();

            CollectionAssert.AreEquivalent(expected, selection);
        }

        [Test]
        public void ShouldReturnEmptySelectionWhenDragEmptySpace()
        {
            PrepareForDrag();
            manager.GetDragSelection().Returns(new List<ISelectable>());
            var selection = manager.GetNewSelection();
            CollectionAssert.IsEmpty(selection);
        }

        [Test]
        public void ShouldReturnSelectionOnKey()
        {
            int groupId = 1;
            PrepareForKey(groupId);

            var expected = new List<ISelectable>();
            expected.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            expected.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            manager.Groups[1] = expected;

            var selection = manager.GetNewSelection();

            CollectionAssert.AreEquivalent(expected, selection);

        }


        [Test]
        public void ShouldReturnEmptySelectionOnKeyNotFound()
        {
            int groupId = 1;
            PrepareForKey(groupId);
            var expected = new List<ISelectable>();

            var selection = manager.GetNewSelection();

            CollectionAssert.AreEquivalent(expected, selection);
        }

        [Test]
        public void ShouldUpdateSelectionStatusToTrue()
        {

            List<ISelectable> original = new List<ISelectable>();
            original.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            original.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            original.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            var list = manager.UpdateSelectionStatus(original, true);
            CollectionAssert.AreEquivalent(original, list);
            foreach (var item in list)
            {
                Assert.True(item.IsSelected);
            }
        }


        [Test]
        public void ShouldUpdateSelectionStatusToFalse()
        {

            List<ISelectable> original = new List<ISelectable>();
            original.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            original.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            original.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            var list = manager.UpdateSelectionStatus(original, false);
            CollectionAssert.AreEquivalent(original, list);
            foreach (var item in list)
            {
                Assert.False(item.IsSelected);
            }
        }

        [Test]
        public void ShouldUpdatePreSelectionStatusToTrue()
        {

            List<ISelectable> original = new List<ISelectable>();
            original.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            original.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            original.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            var list = manager.UpdatePreSelectionStatus(original, true);
            CollectionAssert.AreEquivalent(original, list);
            foreach (var item in list)
            {
                Assert.True(item.IsPreSelected);
            }
        }


        [Test]
        public void ShouldUpdatePreSelectionStatusToFalse()
        {

            List<ISelectable> original = new List<ISelectable>();
            original.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            original.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            original.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            var list = manager.UpdatePreSelectionStatus(original, false);
            CollectionAssert.AreEquivalent(original, list);
            foreach (var item in list)
            {
                Assert.False(item.IsPreSelected);
            }
        }

        [Test]
        public void ShouldUpdateCurrentToTotallyNewSelection()
        {
            //CurrentSelection
            var currentSelection = new List<ISelectable>();
            currentSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            currentSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            currentSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            foreach (var item in currentSelection)
            {
                item.IsSelected = true;
            }
            //NewSelection
            var newSelection = new List<ISelectable>();
            newSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            newSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));

            manager.CurrentSelection = currentSelection;

            var result = manager.UpdateCurrentSelection(newSelection);

            foreach (var item in currentSelection)
            {
                Assert.False(item.IsSelected);
            }

            foreach (var item in newSelection)
            {
                Assert.True(item.IsSelected);
            }
        }

        [Test]
        public void ShouldUpdateCurrentToEmptyNewSelection()
        {
            //CurrentSelection
            var currentSelection = new List<ISelectable>();
            currentSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            currentSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            currentSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            foreach (var item in currentSelection)
            {
                item.IsSelected = true;
            }
            //NewSelection
            var newSelection = new List<ISelectable>();

            manager.CurrentSelection = currentSelection;

            var result = manager.UpdateCurrentSelection(newSelection);

            foreach (var item in currentSelection)
            {
                Assert.False(item.IsSelected);
            }
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void ShouldUpdateCurrentToMixedNewSelection()
        {
            //CurrentSelection
            var currentSelection = new List<ISelectable>();
            currentSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            currentSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            ISelectable mixedItem = SelectionManagerTestUtils.CreateATestableObject(0);
            currentSelection.Add(mixedItem);
            foreach (var item in currentSelection)
            {
                item.IsSelected = true;
            }
            //NewSelection
            var newSelection = new List<ISelectable>();
            newSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            newSelection.Add(mixedItem);

            manager.CurrentSelection = currentSelection;

            var result = manager.UpdateCurrentSelection(newSelection);

            foreach (var item in currentSelection)
            {
                if (!newSelection.Contains(item))
                    Assert.False(item.IsSelected);
            }

            foreach (var item in newSelection)
            {
                Assert.True(item.IsSelected);
            }
        }

        [Test]
        public void ShouldUpdatePreToTotallyNewSelection()
        {
            //PreSelection
            var preSelection = new List<ISelectable>();
            preSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            preSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            preSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            foreach (var item in preSelection)
            {
                item.IsPreSelected = true;
            }
            //NewSelection
            var newSelection = new List<ISelectable>();
            newSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            newSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));

            manager.PreSelection = preSelection;

            var result = manager.UpdatePreSelection(newSelection);

            foreach (var item in preSelection)
            {
                Assert.False(item.IsPreSelected);
            }

            foreach (var item in newSelection)
            {
                Assert.True(item.IsPreSelected);
            }
        }

        [Test]
        public void ShouldUpdatePreToEmptyNewSelection()
        {
            //PreSelection
            var PreSelection = new List<ISelectable>();
            PreSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            PreSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            PreSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            foreach (var item in PreSelection)
            {
                item.IsPreSelected = true;
            }
            //NewSelection
            var newSelection = new List<ISelectable>();

            manager.PreSelection = PreSelection;

            var result = manager.UpdatePreSelection(newSelection);

            foreach (var item in PreSelection)
            {
                Assert.False(item.IsPreSelected);
            }
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void ShouldUpdatePreToMixedNewSelection()
        {
            //PreSelection
            var PreSelection = new List<ISelectable>();
            PreSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            PreSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            ISelectable mixedItem = SelectionManagerTestUtils.CreateATestableObject(0);
            PreSelection.Add(mixedItem);
            foreach (var item in PreSelection)
            {
                item.IsPreSelected = true;
            }
            //NewSelection
            var newSelection = new List<ISelectable>();
            newSelection.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            newSelection.Add(mixedItem);

            manager.PreSelection = PreSelection;

            var result = manager.UpdatePreSelection(newSelection);

            foreach (var item in PreSelection)
            {
                if (!newSelection.Contains(item))
                    Assert.False(item.IsPreSelected);
            }

            foreach (var item in newSelection)
            {
                Assert.True(item.IsPreSelected);
            }
        }


        [Test]
        public void ShouldStartSelection()
        {
            //when
            var manager = Substitute.For<SelectionManager>();
            Vector3 initialPos = new Vector3(0.5f, 0.5f, 0f);
            manager.StartOfSelection(initialPos);

            Assert.AreEqual(initialPos, manager.InitialScreenPosition);
        }

        [Test]
        public void ShouldEndSelection()
        {

            //CurrentSelection
            PrepareForDrag();
            manager.PreSelection = new List<ISelectable>();
            var expected = new List<ISelectable>();
            expected.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            expected.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            expected.Add(SelectionManagerTestUtils.CreateATestableObject(0));

            manager.GetDragSelection().Returns(expected);
            manager.PerformSelection(Arg.Any<List<ISelectable>>(), Arg.Any<List<ISelectable>>(), Arg.Is(SelectionTypeEnum.DRAG)).Returns(expected);

            Vector3 finalPos = new Vector3(0.5f, 0.5f, 0f);
            manager.EndOfSelection(finalPos);

            Assert.AreEqual(finalPos, manager.FinalScreenPosition);
            CollectionAssert.AreEquivalent(expected, manager.CurrentSelection);
            foreach (var item in manager.CurrentSelection)
            {
                Assert.True(item.IsSelected);
            }

            Assert.AreEqual(finalPos, manager.FinalScreenPosition);
            Assert.AreEqual(0, manager.KeyPressed);
            Assert.False(manager.IsSelecting);

        }



        [Test]
        public void ShouldDoPreSelection()
        {
            //PreSelection
            PrepareForDrag();
            var expected = new List<ISelectable>();
            expected.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            expected.Add(SelectionManagerTestUtils.CreateATestableObject(0));
            expected.Add(SelectionManagerTestUtils.CreateATestableObject(0));

            manager.Mods = new List<IBaseSelectionMod>();

            manager.When(x => x.GetDragSelection()).DoNotCallBase();
            manager.GetDragSelection().Returns(expected);

            manager.When(x => x.PerformSelection(default, default, default)).DoNotCallBase();
            manager.PerformSelection(default, default, default).ReturnsForAnyArgs(expected);

            Vector3 finalPos = new Vector3(0.5f, 0.5f, 0f);
            manager.DoPreSelection(finalPos);

            Assert.AreEqual(finalPos, manager.FinalScreenPosition);
            CollectionAssert.AreEquivalent(expected, manager.PreSelection);
            foreach (var item in manager.PreSelection)
            {
                Assert.True(item.IsPreSelected);
            }
        }

        [Test]
        public void ShouldGetSelectionMainPoint()
        {
            var mainPoint = manager.GetSelectionMainPoint();
            Assert.AreEqual(mainPoint, Vector3.zero);
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldGetModifiersToBeApplied(SelectionTypeEnum selectionType, int howManyAll, int howManyClick, int howManyDrag, int howManyKey)
        {
            List<IBaseSelectionMod> mods = new List<IBaseSelectionMod>();
            List<IBaseSelectionMod> expectedMods = new List<IBaseSelectionMod>();
            GetModsToTest(selectionType, howManyAll, howManyClick, howManyDrag, howManyKey, mods, expectedMods);

            manager.Mods = mods;

            var result = manager.GetModifiersToBeApplied(selectionType);

            CollectionAssert.AreEquivalent(expectedMods, result);

        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifiers(SelectionTypeEnum selectionType, int howManyAll, int howManyClick, int howManyDrag, int howManyKey)
        {
            List<IBaseSelectionMod> mods = new List<IBaseSelectionMod>();
            List<IBaseSelectionMod> expectedMods = new List<IBaseSelectionMod>();
            GetModsToTest(selectionType, howManyAll, howManyClick, howManyDrag, howManyKey, mods, expectedMods);

            manager.When(x => x.GetModifiersToBeApplied(Arg.Any<SelectionTypeEnum>())).DoNotCallBase();
            manager.GetModifiersToBeApplied(Arg.Any<SelectionTypeEnum>()).Returns(expectedMods);

            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.DRAG, false, new List<ISelectable>(), new List<ISelectable>(), new List<ISelectable>());
            SelectionArgsXP args = SelectionManagerTestUtils.GetDefaultArgs(arguments);

            var result = manager.ApplyModifiers(args);

            foreach (var mod in mods)
            {
                if (mod.Type == selectionType || mod.Type == SelectionTypeEnum.ALL)
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
        public void ShouldReturnDefaultArgsWhenGetSelectionArgsWithNull()
        {
            List<ISelectable> oldSelection = null;
            List<ISelectable> newSelection = null;
            var args = manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.DRAG, false);

            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.DRAG, args.Arguments.IsPreSelection, args.Arguments.OldSelection, args.Arguments.NewSelection, args.Arguments.MainList);
            SelectionArgsXP expected = SelectionManagerTestUtils.GetDefaultArgs(arguments);

            AssertArgs(expected, args);
        }

        [Test]
        public void ShouldReturnDefaultArgsWhenGetSelectionArgsWithEmpty()
        {
            List<ISelectable> oldSelection = new List<ISelectable>();
            List<ISelectable> newSelection = new List<ISelectable>();

            var args = manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.DRAG, false);

            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.DRAG, args.Arguments.IsPreSelection, args.Arguments.OldSelection, args.Arguments.NewSelection, args.Arguments.MainList);
            SelectionArgsXP expected = SelectionManagerTestUtils.GetDefaultArgs(arguments);

            AssertArgs(expected, args);
        }

        [Test]
        public void ShouldReturnCustomArgsWhenGetSelectionArgsWithCustom()
        {
            List<ISelectable> oldSelection = new List<ISelectable>() { SelectionManagerTestUtils.CreateATestableObject(0) };
            List<ISelectable> newSelection = new List<ISelectable>() { SelectionManagerTestUtils.CreateATestableObject(1) };
            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.CLICK, false, oldSelection, newSelection, new List<ISelectable>());
            SelectionArgsXP expected = SelectionManagerTestUtils.GetDefaultArgs(arguments);

            var args = manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.CLICK, false);

            AssertArgs(expected, args);
        }

        #region methods

        private static void GetModsToTest(SelectionTypeEnum selectionType, int howManyAll, int howManyClick, int howManyDrag, int howManyKey, List<IBaseSelectionMod> mods, List<IBaseSelectionMod> expectedMods)
        {
            List<IBaseSelectionMod> allMods = SelectionManagerTestUtils.GetSomeModsFromType(howManyAll, SelectionTypeEnum.ALL);
            List<IBaseSelectionMod> clickMods = SelectionManagerTestUtils.GetSomeModsFromType(howManyClick, SelectionTypeEnum.CLICK);
            List<IBaseSelectionMod> dragMods = SelectionManagerTestUtils.GetSomeModsFromType(howManyDrag, SelectionTypeEnum.DRAG);
            List<IBaseSelectionMod> keyMods = SelectionManagerTestUtils.GetSomeModsFromType(howManyKey, SelectionTypeEnum.KEY);
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

        private static SelectionManager GetSelectionManager()
        {

            var manager = Substitute.ForPartsOf<SelectionManager>();
            var so = Substitute.For<IRuntimeSet<ISelectable>>();
            manager.SelectableList = so;

            return manager;
        }

        private void PrepareForDrag()
        {
            manager.GetObjectClicked().Returns(x => null);
            manager.KeyPressed = 0;
        }

        private void PrepareForKey(int v)
        {
            manager.GetObjectClicked().Returns(x => null);
            manager.KeyPressed = v;
        }

        private ISelectable PrepareForClick()
        {
            ISelectable so = SelectionManagerTestUtils.CreateATestableObject(0);
            manager.GetObjectClicked().Returns(so);
            manager.KeyPressed = 0;
            return so;
        }


        private void AssertArgs(SelectionArgsXP expected, SelectionArgsXP actual)
        {
            CollectionAssert.AreEquivalent(expected.Arguments.OldSelection, actual.Arguments.OldSelection);
            CollectionAssert.AreEquivalent(expected.Arguments.NewSelection, actual.Arguments.NewSelection);
            CollectionAssert.AreEquivalent(expected.Result.ToBeAdded, actual.Result.ToBeAdded);
            CollectionAssert.AreEquivalent(expected.Result.ToBeRemoved, actual.Result.ToBeRemoved);
            Assert.AreEqual(expected.Arguments.SelectionType, actual.Arguments.SelectionType);
        }

        #endregion

        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                yield return new TestCaseData(SelectionTypeEnum.ALL, /*HowManyAll*/2, /*HowManyClick*/3, /*HowManyDrag*/2, /*HowManyKey*/1);
                yield return new TestCaseData(SelectionTypeEnum.CLICK, /*HowManyAll*/2, /*HowManyClick*/3, /*HowManyDrag*/2, /*HowManyKey*/1);
                yield return new TestCaseData(SelectionTypeEnum.DRAG, /*HowManyAll*/2, /*HowManyClick*/3, /*HowManyDrag*/2, /*HowManyKey*/1);
                yield return new TestCaseData(SelectionTypeEnum.KEY, /*HowManyAll*/2, /*HowManyClick*/3, /*HowManyDrag*/2, /*HowManyKey*/1);

                yield return new TestCaseData(SelectionTypeEnum.ALL, /*HowManyAll*/0, /*HowManyClick*/0, /*HowManyDrag*/0, /*HowManyKey*/0);
                yield return new TestCaseData(SelectionTypeEnum.CLICK, /*HowManyAll*/0, /*HowManyClick*/0, /*HowManyDrag*/0, /*HowManyKey*/0);
                yield return new TestCaseData(SelectionTypeEnum.DRAG, /*HowManyAll*/0, /*HowManyClick*/0, /*HowManyDrag*/0, /*HowManyKey*/0);
                yield return new TestCaseData(SelectionTypeEnum.KEY, /*HowManyAll*/0, /*HowManyClick*/0, /*HowManyDrag*/0, /*HowManyKey*/0);
            }
        }

    }


}
