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
            List<ISelectable> oldSelection = null;
            List<ISelectable> newSelection = null;
            var args = Manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.DRAG);

            SelectionArgsXP expected = SelectionManagerTestUtils.GetDefaultArgs();
            expected.SelectionType = SelectionTypeEnum.DRAG;
            AssertArgs(expected, args);
        }

        [Test]
        public void ShouldReturnDefaultArgsWhenGetSelectionArgsWithEmpty()
        {
            List<ISelectable> oldSelection = new List<ISelectable>();
            List<ISelectable> newSelection = new List<ISelectable>();

            var args = Manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.DRAG);

            SelectionArgsXP expected = SelectionManagerTestUtils.GetDefaultArgs();
            expected.SelectionType = SelectionTypeEnum.DRAG;

            AssertArgs(expected, args);
        }

        [Test]
        public void ShouldReturnCustomArgsWhenGetSelectionArgsWithCustom()
        {
            List<ISelectable> oldSelection = new List<ISelectable>() { SelectionManagerTestUtils.CreateATestableObject(0) };
            List<ISelectable> newSelection = new List<ISelectable>() { SelectionManagerTestUtils.CreateATestableObject(1) };
            SelectionArgsXP expected = SelectionManagerTestUtils.GetDefaultArgs();
            expected.SelectionType = SelectionTypeEnum.CLICK;
            expected.OldSelection.AddRange(oldSelection);
            expected.NewSelection.AddRange(newSelection);
            expected.ToBeAdded.AddRange(newSelection);
            expected.ToBeRemoved.AddRange(oldSelection);

            var args = Manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.CLICK);

            AssertArgs(expected, args);
        }


        [TestCaseSource(nameof(Scenarios))]
        public void ShouldFinalizeSelectionCorrectly(int mainListCount, int[] oldSelection, int[] newSelection, int[] expectedToBeAdded, int[] expectedToBeRemoved)
        {

            // var list = new List<ISelectable>();
            // for (var i = 0; i < mainListCount; i++)
            // {
            //     var selectable = Substitute.For<ISelectable>();
            //     list.Add(selectable);
            // }

            // var args = SelectionManagerTestUtils.GetDefaultArgs();

            // args.OldSelection = FindByIndex<ISelectable>((List<ISelectable>) list, oldSelection);
            // args.NewSelection = FindByIndex<ISelectable>(list, newSelection);
            // var ExpectedToBeAdded = FindByIndex<ISelectable>(list, expectedToBeAdded);
            // var ExpectedToBeRemoved = FindByIndex<ISelectable>(list, expectedToBeRemoved);

            // var result = manager.FinalizeSelection(args);

            // for (var i = 0; i < ExpectedToBeAdded.Count; i++)
            // {
            //     CollectionAssert.Contains(result, expectedToBeAdded[i]);
            // }
            // for (var i = 0; i < ExpectedToBeRemoved.Count; i++)
            // {
            //     CollectionAssert.DoesNotContain(result, ExpectedToBeRemoved[i]);
            // }

        }

        private List<T> FindByIndex<T>(List<T> mainList, int[] indexes) where T : ISelectable
        {
            List<T> list = new List<T>();
            for (var i = 0; i < indexes.Length; i++)
            {
                list.Add(mainList[indexes[i]]);
            }
            return list;
        }

        [Test]
        public void ShouldUpdateSelectionStatus()
        {
            List<ISelectable> expected = new List<ISelectable>();
            for (var i = 0; i < 4; i++)
            {
                ISelectable item = SelectionManagerTestUtils.CreateATestableObject(i);
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
            List<ISelectable> expected = new List<ISelectable>();
            for (var i = 0; i < 4; i++)
            {
                ISelectable item = SelectionManagerTestUtils.CreateATestableObject(i);
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

        private static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                // (mainListCount, oldSelection, newSelection, toBeAdded, toBeRemoved)
                //Empty new
                yield return new TestCaseData(5, new int[] { }, new int[] { }, new int[] { }, new int[] { });
                yield return new TestCaseData(5, new int[] { 1 }, new int[] { }, new int[] { }, new int[] { 1 });
                //Not empty new - Not In Old
                yield return new TestCaseData(5, new int[] { 1 }, new int[] { 2 }, new int[] { 2 }, new int[] { 1 });
                yield return new TestCaseData(5, new int[] { 1, 3 }, new int[] { 2 }, new int[] { 2 }, new int[] { 1, 3 });
                //Not empty new - Within Old
                yield return new TestCaseData(5, new int[] { 1 }, new int[] { 1 }, new int[] { 1 }, new int[] { });
                yield return new TestCaseData(5, new int[] { 1, 2 }, new int[] { 1 }, new int[] { 1 }, new int[] { 2 });
            }
        }
    }

    internal class DerivedClass : BaseSelectionManager
    {
        public override SelectionArgsXP ApplyModifiers(SelectionArgsXP args)
        {
            return args;
        }

    }
}
