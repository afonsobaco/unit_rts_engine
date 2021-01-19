using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using RTSEngine.Manager;
using RTSEngine.Selection.Mod;
using RTSEngine.Core;
using NSubstitute;

namespace RTSEngine.Selection.Tests
{

    public class SelectionManagerTest
    {

        [Test]
        public void ShouldReturnDragSelectionType()
        {
            var manager = GetSelectionManager();
            var type = manager.GetSelectionType();
            Assert.AreEqual(SelectionTypeEnum.DRAG, type);
            Assert.AreEqual(-1, manager.KeyPressed);
            Assert.Null(manager.Cliked);
        }


        [Test]
        public void ShouldReturnClickSelectionType()
        {
            var manager = GetSelectionManager();
            var obj = SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(0);
            manager.InitialScreenPosition = Camera.main.WorldToScreenPoint(obj.transform.position);
            manager.FinalScreenPosition = Camera.main.WorldToScreenPoint(obj.transform.position);

            var type = manager.GetSelectionType();

            Assert.AreEqual(SelectionTypeEnum.CLICK, type);
            Assert.AreEqual(-1, manager.KeyPressed);
        }

        [Test]
        public void ShouldReturnKeySelectionType()
        {
            var manager = GetSelectionManager();
            manager.KeyPressed = 2;
            var type = manager.GetSelectionType();

            Assert.AreEqual(SelectionTypeEnum.KEY, type);
            Assert.Null(manager.Cliked);
        }


        [Test]
        public void ShouldReturnNewSelectionOnClick()
        {
            var manager = GetSelectionManager();
            var expected = SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(0);
            manager.InitialScreenPosition = Camera.main.WorldToScreenPoint(expected.transform.position);
            manager.FinalScreenPosition = Camera.main.WorldToScreenPoint(expected.transform.position);

            var selection = manager.GetNewSelection();

            Assert.True(selection.Contains(expected));
            Assert.AreEqual(1, selection.Count);
        }

        [Test]
        public void ShouldReturnEmptySelectionWhenDragEmptySpace()
        {
            var manager = GetSelectionManager();
            manager.InitialScreenPosition = Camera.main.ViewportToScreenPoint(Vector3.zero);
            manager.FinalScreenPosition = Camera.main.ViewportToScreenPoint(Vector3.one);
            var selection = manager.GetNewSelection();
            CollectionAssert.IsEmpty(selection);
            foreach (var item in manager.SelectableList.GetList())
            {
                Assert.False(item.IsSelected);
            }
        }

        [Test]
        public void ShouldReturnAllObjectsWhenDragOnFullScreenSpace()
        {
            var manager = GetSelectionManager();
            manager.InitialScreenPosition = Camera.main.ViewportToScreenPoint(Vector3.zero);
            manager.FinalScreenPosition = Camera.main.ViewportToScreenPoint(Vector3.one);
            List<SelectableObject> expected = GetDefaultExpected(manager);

            var selection = manager.GetNewSelection();

            CollectionAssert.IsNotEmpty(selection);
            foreach (var item in selection)
            {
                Assert.True(item.IsSelected);
            }
        }

        [Test]
        public void ShouldReturnNewSelectionOnKey()
        {
            var manager = GetSelectionManager();
            List<SelectableObject> expected = GetDefaultExpected(manager);
            manager.CurrentSelection = expected;
            manager.SetGroup(1);
            manager.KeyPressed = 1;
            manager.CurrentSelection = null;

            var selection = manager.GetNewSelection();

            CollectionAssert.AreEquivalent(expected, selection);
            foreach (var item in expected)
            {
                Assert.True(item.IsSelected);
            }
        }

        [Test]
        public void ShouldReturnEmptySelectionOnKeyNotFound()
        {
            var manager = GetSelectionManager();
            List<SelectableObject> expected = GetDefaultExpected(manager);

            manager.CurrentSelection = expected;
            manager.SetGroup(1);
            manager.CurrentSelection = null;
            manager.KeyPressed = 2;

            var selection = manager.GetNewSelection();

            CollectionAssert.IsEmpty(selection);
            foreach (var item in manager.SelectableList.GetList())
            {
                Assert.False(item.IsSelected);
            }
        }

        [Test]
        public void ShouldUpdateCurrentToFullSelection()
        {
            //when
            var manager = GetSelectionManager();
            List<SelectableObject> expected = GetDefaultExpected(manager);

            //act
            manager.UpdateCurrentSelection(expected);

            //then
            CollectionAssert.IsNotEmpty(manager.CurrentSelection);
            CollectionAssert.AreEquivalent(expected, manager.CurrentSelection);
            foreach (var item in manager.SelectableList.GetList())
            {
                Assert.True(item.IsSelected);
            }
        }

        [Test]
        public void ShouldUpdateCurrentSelectionToNone()
        {
            //when
            var manager = GetSelectionManager();
            List<SelectableObject> expected = GetDefaultExpected(manager);

            //act
            manager.UpdateCurrentSelection(new List<SelectableObject>());

            //then
            CollectionAssert.IsEmpty(manager.CurrentSelection);
            Assert.AreEqual(0, manager.CurrentSelection.Count);
            foreach (var item in manager.SelectableList.GetList())
            {
                Assert.False(item.IsSelected);
            }
        }

        [Test]
        public void ShouldUpdateCurrentSelectionToTheFirstObjectOnList()
        {
            //when
            var manager = GetSelectionManager();
            List<SelectableObject> expected = GetDefaultExpected(manager);

            //act
            List<SelectableObject> oneObjectList = new List<SelectableObject>() { expected[0] };
            manager.UpdateCurrentSelection(oneObjectList);

            //then
            CollectionAssert.IsNotEmpty(manager.CurrentSelection);
            Assert.AreEqual(1, manager.CurrentSelection.Count);
            foreach (var item in manager.SelectableList.GetList())
            {
                if (oneObjectList.Contains(item))
                {
                    Assert.True(item.IsSelected);
                }
                else
                {
                    Assert.False(item.IsSelected);
                }
            }
        }

        private static List<SelectableObject> GetDefaultExpected(SelectionManager manager)
        {
            List<SelectableObject> expected = new List<SelectableObject>(){
                SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(0),
                SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(1),
                SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(2),
                SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(3),
                SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(4),
            };
            foreach (var item in expected)
            {
                manager.SelectableList.AddToList(item);
            }

            return expected;
        }

        #region methods
        private static SelectionManager GetSelectionManager()
        {

            var manager = new SelectionManager();
            var so = ScriptableObject.CreateInstance<GameRuntimeSet>();
            manager.SelectableList = so;
            return manager;
        }
        #endregion
    }


}
