using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using RTSEngine.Manager;
using RTSEngine.Core;
using RTSEngine.Manager.SelectionMods;
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
            Manager = GetSelectionManager();
        }

        [Test]
        public void ShouldReturnDragSelectionType()
        {
            PrepareForDrag();
            var type = Manager.GetSelectionType();
            Assert.AreEqual(SelectionTypeEnum.DRAG, type);
        }

        [Test]
        public void ShouldReturnClickSelectionType()
        {
            PrepareForClick();
            var type = Manager.GetSelectionType();
            Assert.AreEqual(SelectionTypeEnum.CLICK, type);
        }

        [Test]
        public void ShouldReturnKeySelectionType()
        {
            PrepareForKey(1);
            var type = Manager.GetSelectionType();
            Assert.AreEqual(SelectionTypeEnum.KEY, type);
        }

        [Test]
        public void ShouldReturnTrueWhenKeyPressedIsGreaterThanZero()
        {
            Manager.KeyPressed = 1;
            var value = Manager.IsKey();
            Assert.True(value);
        }

        [Test]
        public void ShouldReturnFalseWhenKeyPressedIsLessOrEqualsZero()
        {
            Manager.KeyPressed = 0;
            var value = Manager.IsKey();
            Assert.False(value);
        }

        [Test]
        public void ShouldReturnFalseWhenNoObjectClicked()
        {
            Manager.GetObjectClicked().Returns(x => null);
            var value = Manager.IsClick();
            Assert.False(value);
        }

        [Test]
        public void ShouldReturnTrueWhenObjectClicked()
        {
            PrepareForClick();
            var value = Manager.IsClick();
            Assert.True(value);
        }


        [Test]
        public void ShouldReturnNewSelectionOnClick()
        {
            var expected = PrepareForClick();

            var selection = Manager.GetNewSelection();

            Assert.True(selection.Contains(expected));
            Assert.AreEqual(1, selection.Count);
        }



        [Test]
        public void ShouldAddToSpecificGroup()
        {
            int groupId = 1;
            Manager.CurrentSelection = new List<SelectableObject>();
            Manager.CurrentSelection.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            Manager.SetGroup(groupId);
            List<SelectableObject> collection = Manager.Groups[groupId];
            CollectionAssert.IsNotEmpty(collection);
            CollectionAssert.AreEquivalent(Manager.CurrentSelection, collection);
        }

        [Test]
        public void ShouldClearSpecificGroup()
        {
            int groupId = 1;
            Manager.CurrentSelection = new List<SelectableObject>();
            Manager.SetGroup(groupId);
            List<SelectableObject> collection = Manager.Groups[groupId];
            CollectionAssert.IsEmpty(collection);
            CollectionAssert.AreEquivalent(Manager.CurrentSelection, collection);
        }

        [Test]
        public void ShouldGetSpecificGroup()
        {
            int groupId = 1;
            var expected = new List<SelectableObject>();
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            Manager.Groups[1] = expected;

            var collection = Manager.GetGroup(groupId);

            CollectionAssert.IsNotEmpty(collection);
            CollectionAssert.AreEquivalent(expected, collection);
        }

        [Test]
        public void ShouldReturnEmptyWhenGroupKeyNotFound()
        {
            int groupId = 1;

            var collection = Manager.GetGroup(groupId);

            CollectionAssert.IsEmpty(collection);
        }

        [Test]
        public void ShouldReturnSelectionWhenDragOnScreenSpace()
        {
            PrepareForDrag();

            List<SelectableObject> expected = new List<SelectableObject>();
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            Manager.GetSelectionOnScreen().Returns(expected);

            var selection = Manager.GetNewSelection();

            CollectionAssert.AreEquivalent(expected, selection);
        }

        [Test]
        public void ShouldReturnEmptySelectionWhenDragEmptySpace()
        {
            PrepareForDrag();
            Manager.GetSelectionOnScreen().Returns(new List<SelectableObject>());
            var selection = Manager.GetNewSelection();
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
            Manager.Groups[1] = expected;

            var selection = Manager.GetNewSelection();

            CollectionAssert.AreEquivalent(expected, selection);

        }


        [Test]
        public void ShouldReturnEmptySelectionOnKeyNotFound()
        {
            int groupId = 1;
            PrepareForKey(groupId);
            var expected = new List<SelectableObject>();

            var selection = Manager.GetNewSelection();

            CollectionAssert.AreEquivalent(expected, selection);
        }

        [Test]
        public void ShouldUpdateSelectionStatusToTrue()
        {

            List<SelectableObject> original = new List<SelectableObject>();
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            original.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            var list = Manager.UpdateSelectionStatus(original, true);
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
            var list = Manager.UpdateSelectionStatus(original, false);
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
            var list = Manager.UpdatePreSelectionStatus(original, true);
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
            var list = Manager.UpdatePreSelectionStatus(original, false);
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

            Manager.CurrentSelection = currentSelection;

            var result = Manager.UpdateCurrentSelection(newSelection);

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

            Manager.CurrentSelection = currentSelection;

            var result = Manager.UpdateCurrentSelection(newSelection);

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

            Manager.CurrentSelection = currentSelection;

            var result = Manager.UpdateCurrentSelection(newSelection);

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

            Manager.PreSelection = preSelection;

            var result = Manager.UpdatePreSelection(newSelection);

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

            Manager.PreSelection = PreSelection;

            var result = Manager.UpdatePreSelection(newSelection);

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

            Manager.PreSelection = PreSelection;

            var result = Manager.UpdatePreSelection(newSelection);

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
            Manager.PreSelection = new List<SelectableObject>();
            var expected = new List<SelectableObject>();
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());
            expected.Add(SelectionManagerTestUtils.CreateGameObject<SelectableObject>());

            Manager.GetSelectionOnScreen().Returns(expected);
            Manager.PerformSelection(Arg.Any<List<SelectableObject>>(), Arg.Any<List<SelectableObject>>(), Arg.Is(SelectionTypeEnum.DRAG)).Returns(expected);

            Vector3 finalPos = new Vector3(0.5f, 0.5f, 0f);
            Manager.EndOfSelection(finalPos);

            Assert.AreEqual(finalPos, Manager.FinalScreenPosition);
            CollectionAssert.AreEquivalent(expected, Manager.CurrentSelection);
            foreach (var item in Manager.CurrentSelection)
            {
                Assert.True(item.IsSelected);
            }

            Assert.AreEqual(finalPos, Manager.FinalScreenPosition);
            Assert.AreEqual(0, Manager.KeyPressed);
            Assert.False(Manager.IsSelecting);

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

            Manager.GetSelectionOnScreen().Returns(expected);
            Manager.PerformSelection(Arg.Any<List<SelectableObject>>(), Arg.Any<List<SelectableObject>>(), Arg.Is(SelectionTypeEnum.DRAG)).Returns(expected);

            Vector3 finalPos = new Vector3(0.5f, 0.5f, 0f);
            Manager.DoPreSelection(finalPos);

            Assert.AreEqual(finalPos, Manager.FinalScreenPosition);
            CollectionAssert.AreEquivalent(expected, Manager.PreSelection);
            foreach (var item in Manager.PreSelection)
            {
                Assert.True(item.IsPreSelected);
            }
        }

        [Test]
        public void ShouldGetSelectionMainPoint()
        {
            var mainPoint = Manager.GetSelectionMainPoint();
            Assert.AreEqual(mainPoint, Vector3.zero);
        }

        // [TestCaseSource(nameof(Scenarios))]
        // public void ShouldGetModifiersToBeApplied(SelectionTypeEnum selectionType, int howManyAll, int howManyClick, int howManyDrag, int howManyKey, int howManyModsApplied)
        // {
        //     var mods = new List<ScriptableObject>();
        //     mods.AddRange(GetSomeModsFromType(howManyAll, SelectionTypeEnum.ALL));
        //     mods.AddRange(GetSomeModsFromType(howManyClick, SelectionTypeEnum.CLICK));
        //     mods.AddRange(GetSomeModsFromType(howManyDrag, SelectionTypeEnum.DRAG));
        //     mods.AddRange(GetSomeModsFromType(howManyKey, SelectionTypeEnum.KEY));
        //     manager.ScriptableObjectMods = mods.Select(x => x as ScriptableObject).ToList();

        //     var result = manager.GetModifiersToBeApplied(selectionType);

        // }



        // [TestCaseSource(nameof(Scenarios))]
        // public void ShouldApplyModifiers(SelectionTypeEnum selectionType, int howManyAll, int howManyClick, int howManyDrag, int howManyKey, int howManyModsApplied)
        // {
        //     var mods = new List<ISelectionMod<SelectableObject, SelectionTypeEnum>>();
        //     mods.AddRange(GetSomeModsFromType(howManyAll, SelectionTypeEnum.ALL));
        //     mods.AddRange(GetSomeModsFromType(howManyClick, SelectionTypeEnum.CLICK));
        //     mods.AddRange(GetSomeModsFromType(howManyDrag, SelectionTypeEnum.DRAG));
        //     mods.AddRange(GetSomeModsFromType(howManyKey, SelectionTypeEnum.KEY));


        //     var args = SelectionManagerTestUtils.GetDefaultArgs<SelectableObject, SelectionTypeEnum>();
        //     args.SelectionType = selectionType;
        //     args.SelectionModifiers = mods;

        //     var result = manager.ApplyModifiers(args);

        //     int expectedCount = 0;
        //     foreach (var mod in mods)
        //     {
        //         if (mod.Type == selectionType || mod.Type == SelectionTypeEnum.ALL)
        //         {
        //             mod.ReceivedWithAnyArgs().Apply(default);
        //             expectedCount++;
        //         }
        //         else
        //         {
        //             mod.DidNotReceiveWithAnyArgs().Apply(default);
        //         }
        //     }
        //     Assert.AreEqual(expectedCount, howManyModsApplied);

        // }

        #region methods
        private static SelectionManager GetSelectionManager()
        {

            var manager = Substitute.For<SelectionManager>();
            var so = Substitute.For<IRuntimeSet<SelectableObject>>();
            manager.SelectableList = so;

            return manager;
        }

        private void PrepareForDrag()
        {
            Manager.GetObjectClicked().Returns(x => null);
            Manager.KeyPressed = 0;
        }

        private void PrepareForKey(int v)
        {
            Manager.GetObjectClicked().Returns(x => null);
            Manager.KeyPressed = v;
        }

        private SelectableObject PrepareForClick()
        {
            SelectableObject so = SelectionManagerTestUtils.CreateGameObject<SelectableObject>();
            Manager.GetObjectClicked().Returns(so);
            Manager.KeyPressed = 0;
            return so;
        }

        private static List<ISelectionMod<SelectableObject, SelectionTypeEnum>> GetSomeModsFromType(int amount, SelectionTypeEnum type)
        {
            var mods = SelectionManagerTestUtils.GetSomeMods<SelectableObject, SelectionTypeEnum>(amount);
            mods.ForEach(x => x.Type = type);
            return mods;
        }

        private static List<AbstractSelectionMod<SelectableObject, SelectionTypeEnum>> GetSomeAbstractSelectionModifiers(int amount, SelectionTypeEnum type)
        {


            return null;
        }


        #endregion

        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                yield return new TestCaseData(SelectionTypeEnum.ALL, /*HowManyAll*/2, /*HowManyClick*/3, /*HowManyDrag*/2, /*HowManyKey*/1, /*HowManyModsApplied*/2);
                yield return new TestCaseData(SelectionTypeEnum.CLICK, /*HowManyAll*/2, /*HowManyClick*/3, /*HowManyDrag*/2, /*HowManyKey*/1, /*HowManyModsApplied*/5);
                yield return new TestCaseData(SelectionTypeEnum.DRAG, /*HowManyAll*/2, /*HowManyClick*/3, /*HowManyDrag*/2, /*HowManyKey*/1, /*HowManyModsApplied*/4);
                yield return new TestCaseData(SelectionTypeEnum.KEY, /*HowManyAll*/2, /*HowManyClick*/3, /*HowManyDrag*/2, /*HowManyKey*/1, /*HowManyModsApplied*/3);

                yield return new TestCaseData(SelectionTypeEnum.ALL, /*HowManyAll*/0, /*HowManyClick*/0, /*HowManyDrag*/0, /*HowManyKey*/0, /*HowManyModsApplied*/0);
                yield return new TestCaseData(SelectionTypeEnum.CLICK, /*HowManyAll*/0, /*HowManyClick*/0, /*HowManyDrag*/0, /*HowManyKey*/0, /*HowManyModsApplied*/0);
                yield return new TestCaseData(SelectionTypeEnum.DRAG, /*HowManyAll*/0, /*HowManyClick*/0, /*HowManyDrag*/0, /*HowManyKey*/0, /*HowManyModsApplied*/0);
                yield return new TestCaseData(SelectionTypeEnum.KEY, /*HowManyAll*/0, /*HowManyClick*/0, /*HowManyDrag*/0, /*HowManyKey*/0, /*HowManyModsApplied*/0);
            }
        }

        public SelectionManager Manager { get => manager; set => manager = value; }
    }


}
