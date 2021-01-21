using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using RTSEngine.Selection.Mod;
using RTSEngine.Core;
using RTSEngine.Selection;
using RTSEngine.Manager;
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
            manager.CurrentSelection = new List<SelectableObject>();
            manager.CurrentSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            manager.SetGroup(groupId);
            List<SelectableObject> collection = manager.Groups[groupId];
            CollectionAssert.IsNotEmpty(collection);
            CollectionAssert.AreEquivalent(manager.CurrentSelection, collection);
        }

        [Test]
        public void ShouldClearSpecificGroup()
        {
            int groupId = 1;
            manager.CurrentSelection = new List<SelectableObject>();
            manager.SetGroup(groupId);
            List<SelectableObject> collection = manager.Groups[groupId];
            CollectionAssert.IsEmpty(collection);
            CollectionAssert.AreEquivalent(manager.CurrentSelection, collection);
        }

        [Test]
        public void ShouldGetSpecificGroup()
        {
            int groupId = 1;
            var expected = new List<SelectableObject>();
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
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

            List<SelectableObject> expected = new List<SelectableObject>();
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            manager.GetSelectionOnScreen().Returns(expected);

            var selection = manager.GetNewSelection();

            CollectionAssert.AreEquivalent(expected, selection);
        }

        [Test]
        public void ShouldReturnEmptySelectionWhenDragEmptySpace()
        {
            PrepareForDrag();
            manager.GetSelectionOnScreen().Returns(new List<SelectableObject>());
            var selection = manager.GetNewSelection();
            CollectionAssert.IsEmpty(selection);
        }

        [Test]
        public void ShouldReturnSelectionOnKey()
        {
            int groupId = 1;
            PrepareForKey(groupId);

            var expected = new List<SelectableObject>();
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            manager.Groups[1] = expected;

            var selection = manager.GetNewSelection();

            CollectionAssert.AreEquivalent(expected, selection);

        }


        [Test]
        public void ShouldReturnEmptySelectionOnKeyNotFound()
        {
            int groupId = 1;
            PrepareForKey(groupId);
            var expected = new List<SelectableObject>();

            var selection = manager.GetNewSelection();

            CollectionAssert.AreEquivalent(expected, selection);
        }

        [Test]
        public void ShouldUpdateSelectionStatusToTrue()
        {

            List<SelectableObject> original = new List<SelectableObject>();
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
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

            List<SelectableObject> original = new List<SelectableObject>();
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
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

            List<SelectableObject> original = new List<SelectableObject>();
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
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

            List<SelectableObject> original = new List<SelectableObject>();
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
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
            var currentSelection = new List<SelectableObject>();
            currentSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            currentSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            currentSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            foreach (var item in currentSelection)
            {
                item.IsSelected = true;
            }
            //NewSelection
            var newSelection = new List<SelectableObject>();
            newSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            newSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());

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
            var currentSelection = new List<SelectableObject>();
            currentSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            currentSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            currentSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            foreach (var item in currentSelection)
            {
                item.IsSelected = true;
            }
            //NewSelection
            var newSelection = new List<SelectableObject>();

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
            var currentSelection = new List<SelectableObject>();
            currentSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            currentSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            SelectableObject mixedItem = SelectionManagerTestUtils.CreateGameObject<SelectableObject>();
            currentSelection.Add(mixedItem);
            foreach (var item in currentSelection)
            {
                item.IsSelected = true;
            }
            //NewSelection
            var newSelection = new List<SelectableObject>();
            newSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
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
            var preSelection = new List<SelectableObject>();
            preSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            preSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            preSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            foreach (var item in preSelection)
            {
                item.IsPreSelected = true;
            }
            //NewSelection
            var newSelection = new List<SelectableObject>();
            newSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            newSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());

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
            var PreSelection = new List<SelectableObject>();
            PreSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            PreSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            PreSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            foreach (var item in PreSelection)
            {
                item.IsPreSelected = true;
            }
            //NewSelection
            var newSelection = new List<SelectableObject>();

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
            var PreSelection = new List<SelectableObject>();
            PreSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            PreSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            SelectableObject mixedItem = SelectionManagerTestUtils.CreateGameObject<SelectableObject>();
            PreSelection.Add(mixedItem);
            foreach (var item in PreSelection)
            {
                item.IsPreSelected = true;
            }
            //NewSelection
            var newSelection = new List<SelectableObject>();
            newSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
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
            var expected = new List<SelectableObject>();
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());

            manager.GetSelectionOnScreen().Returns(expected);
            manager.PerformSelection(Arg.Any<List<SelectableObject>>(), Arg.Any<List<SelectableObject>>(), Arg.Is(SelectionTypeEnum.DRAG)).Returns(expected);

            Vector3 finalPos = new Vector3(0.5f, 0.5f, 0f);
            manager.EndOfSelection(finalPos);

            Assert.AreEqual(finalPos, manager.FinalScreenPosition);
            CollectionAssert.AreEquivalent(expected, manager.CurrentSelection);
            foreach (var item in manager.CurrentSelection)
            {
                Assert.True(item.IsSelected);
            }

        }



        [Test]
        public void ShouldDoPreSelection()
        {
            //PreSelection
            PrepareForDrag();
            var expected = new List<SelectableObject>();
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());

            manager.GetSelectionOnScreen().Returns(expected);
            manager.PerformSelection(Arg.Any<List<SelectableObject>>(), Arg.Any<List<SelectableObject>>(), Arg.Is(SelectionTypeEnum.DRAG)).Returns(expected);

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

        #region methods
        private static SelectionManager GetSelectionManager()
        {

            var manager = Substitute.For<SelectionManager>();
            var so = Substitute.For<IRuntimeSet<SelectableObject>>();
            manager.SelectableList = so;

            return manager;
        }

        private static SelectionArgsXP<T, E> GetDefaultArgs<T, E>()
        {
            var settings = Substitute.ForPartsOf<ISelectionSettings<T, E>>();
            SelectionArgsXP<T, E> args = new SelectionArgsXP<T, E>();
            args.Settings = settings;
            args.Settings.Mods = new List<IAbstractSelectionMod<T, E>>();
            return args;
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

        private SelectableObject PrepareForClick()
        {
            SelectableObject so = SelectionManagerTestUtils.CreateGameObject<SelectableObject>();
            manager.GetObjectClicked().Returns(so);
            manager.KeyPressed = 0;
            return so;
        }


        #endregion
    }


}
