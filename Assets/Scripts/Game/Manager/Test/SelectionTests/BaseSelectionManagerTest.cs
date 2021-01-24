using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Manager;
using System.Collections.Generic;
using UnityEngine;
using NSubstitute;

namespace Tests.Manager
{
    [TestFixture]
    public class BaseSelectionManagerTest
    {
        private BaseSelectionManager manager;

        public BaseSelectionManager Manager { get => manager; set => manager = value; }

        [SetUp]
        public void Init()
        {
            Manager = new DerivedClass();
        }

        [TearDown]
        public void Finish()
        {

        }


        [Test]
        public void ShouldReturnDefaultArgsWhenGetSelectionArgsWithNull()
        {
            List<SelectableObject> oldSelection = null;
            List<SelectableObject> newSelection = null;
            var args = Manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.DRAG);

            SelectionArgsXP expected = SelectionManagerTestUtils.GetDefaultArgs();
            expected.SelectionType = SelectionTypeEnum.DRAG;
            AssertArgs(expected, args);
        }

        [Test]
        public void ShouldReturnDefaultArgsWhenGetSelectionArgsWithEmpty()
        {
            List<SelectableObject> oldSelection = new List<SelectableObject>();
            List<SelectableObject> newSelection = new List<SelectableObject>();

            var args = Manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.DRAG);

            SelectionArgsXP expected = SelectionManagerTestUtils.GetDefaultArgs();
            expected.SelectionType = SelectionTypeEnum.DRAG;

            AssertArgs(expected, args);
        }

        [Test]
        public void ShouldReturnCustomArgsWhenGetSelectionArgsWithCustom()
        {
            List<SelectableObject> oldSelection = new List<SelectableObject>() { SelectionManagerTestUtils.CreateATestableObject(0) };
            List<SelectableObject> newSelection = new List<SelectableObject>() { SelectionManagerTestUtils.CreateATestableObject(1) };
            SelectionArgsXP expected = SelectionManagerTestUtils.GetDefaultArgs();
            expected.SelectionType = SelectionTypeEnum.CLICK;
            expected.OldSelection = oldSelection;
            expected.NewSelection = newSelection;
            expected.ToBeAdded = newSelection;
            expected.ToBeRemoved = oldSelection;

            var args = Manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.CLICK);

            AssertArgs(expected, args);
        }

        [Test]
        public void ShouldFinalizeSelectionCorrectlyWhenHasSomethingToBeAdded()
        {
            var args = SelectionManagerTestUtils.GetDefaultArgs();
            var amount = 4;
            List<SelectableObject> selection = new List<SelectableObject>();
            List<SelectableObject> expected = new List<SelectableObject>();
            for (var i = 0; i < amount; i++)
            {
                var obj = SelectionManagerTestUtils.CreateATestableObject(i);
                selection.Add(obj);
                expected.Add(obj);
            }
            args.NewSelection = selection;
            args.ToBeAdded = selection;

            var result = Manager.FinalizeSelection(args);

            CollectionAssert.AreEquivalent(expected, result);
            foreach (var item in result)
            {
                Assert.True(item.IsSelected);
            }
        }

        [Test]
        public void ShouldFinalizeSelectionCorrectlyWhenHasSomethingToBeRemoved()
        {
            var args = SelectionManagerTestUtils.GetDefaultArgs();
            var amount = 4;
            List<SelectableObject> expected = new List<SelectableObject>();
            for (var i = 0; i < amount; i++)
            {
                var obj = SelectionManagerTestUtils.CreateATestableObject(i);
                args.NewSelection.Add(obj);
                if (i > 1)
                {

                    args.ToBeAdded.Add(obj);
                    expected.Add(obj);
                }
                else
                {
                    args.ToBeRemoved.Add(obj);
                }
            }

            var result = Manager.FinalizeSelection(args);

            CollectionAssert.AreEquivalent(expected, result);
            foreach (var item in args.ToBeAdded)
            {
                Assert.True(item.IsSelected);
            }
            foreach (var item in args.NewSelection.FindAll(a => !result.Contains(a)))
            {
                Assert.False(item.IsSelected);
            }
        }

        [Test]
        public void ShouldUpdateSelectionStatus()
        {
            List<SelectableObject> expected = new List<SelectableObject>();
            for (var i = 0; i < 4; i++)
            {
                SelectableObject item = SelectionManagerTestUtils.CreateATestableObject(i);
                item.IsSelected = false;
                expected.Add(item);
            }

            //Act
            var actual = Manager.UpdateSelectionStatus(expected, true);
            CollectionAssert.AreEquivalent(expected, actual);
            foreach (var item in expected)
            {
                Assert.True(item.IsSelected);
            }

            actual = Manager.UpdateSelectionStatus(expected, false);
            CollectionAssert.AreEquivalent(expected, actual);
            foreach (var item in expected)
            {
                Assert.False(item.IsSelected);
            }
        }

        [Test]
        public void ShouldUpdatePreSelectionStatus()
        {
            List<SelectableObject> expected = new List<SelectableObject>();
            for (var i = 0; i < 4; i++)
            {
                SelectableObject item = SelectionManagerTestUtils.CreateATestableObject(i);
                item.IsPreSelected = false;
                expected.Add(item);
            }

            //Act
            var actual = Manager.UpdatePreSelectionStatus(expected, true);
            CollectionAssert.AreEquivalent(expected, actual);
            foreach (var item in expected)
            {
                Assert.True(item.IsPreSelected);
            }

            actual = Manager.UpdatePreSelectionStatus(expected, false);
            CollectionAssert.AreEquivalent(expected, actual);
            foreach (var item in expected)
            {
                Assert.False(item.IsPreSelected);
            }
        }

        [TestCase(SelectionTypeEnum.ALL, 1)]
        [TestCase(SelectionTypeEnum.CLICK, 3)]
        [TestCase(SelectionTypeEnum.DRAG, 2)]
        [TestCase(SelectionTypeEnum.KEY, 1)]
        public void ShouldGetModsBySelectionType(SelectionTypeEnum type, int expectedCount)
        {
            List<IBaseSelectionMod> mods = new List<IBaseSelectionMod>();
            mods.AddRange(SelectionManagerTestUtils.GetSomeModsFromType(1, SelectionTypeEnum.ALL));
            mods.AddRange(SelectionManagerTestUtils.GetSomeModsFromType(3, SelectionTypeEnum.CLICK));
            mods.AddRange(SelectionManagerTestUtils.GetSomeModsFromType(2, SelectionTypeEnum.DRAG));
            mods.AddRange(SelectionManagerTestUtils.GetSomeModsFromType(1, SelectionTypeEnum.KEY));
            var result = manager.GetModsBySelectionType(mods, type);

            Assert.AreEqual(expectedCount, result.Count);

        }



        [Test]
        public void ShouldGetEmptyWhenModsDoesNotContainsSelectionType()
        {
            List<IBaseSelectionMod> mods = new List<IBaseSelectionMod>();
            mods.AddRange(SelectionManagerTestUtils.GetSomeModsFromType(1, SelectionTypeEnum.ALL));
            mods.AddRange(SelectionManagerTestUtils.GetSomeModsFromType(2, SelectionTypeEnum.DRAG));
            mods.AddRange(SelectionManagerTestUtils.GetSomeModsFromType(1, SelectionTypeEnum.KEY));
            var result = manager.GetModsBySelectionType(mods, SelectionTypeEnum.CLICK);
            CollectionAssert.IsEmpty(result);
        }

        #region methods

        private void AssertArgs(SelectionArgsXP expected, SelectionArgsXP actual)
        {
            CollectionAssert.AreEquivalent(expected.OldSelection, actual.OldSelection);
            CollectionAssert.AreEquivalent(expected.NewSelection, actual.NewSelection);
            CollectionAssert.AreEquivalent(expected.ToBeAdded, actual.ToBeAdded);
            CollectionAssert.AreEquivalent(expected.ToBeRemoved, actual.ToBeRemoved);
            Assert.AreEqual(expected.SelectionType, actual.SelectionType);
        }

        #endregion
    }

    internal class DerivedClass : BaseSelectionManager
    {
        public override SelectionArgsXP ApplyModifiers(SelectionArgsXP args)
        {
            return args;
        }

    }
}
