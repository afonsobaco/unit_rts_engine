using UnityEngine;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Manager;
using System.Collections.Generic;
using Tests.Utils;

namespace Tests.Manager
{
    [TestFixture]
    public class BaseSelectionManagerTest
    {
        private BaseSelectionManager manager;

        [SetUp]
        public void Init()
        {
            manager = new DerivedClass();
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
            var actual = manager.UpdateSelectionStatus(expected, true);
            CollectionAssert.AreEquivalent(expected, actual);
            foreach (var item in expected)
            {
                Assert.True(item.IsSelected);
            }

            actual = manager.UpdateSelectionStatus(expected, false);
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
            var actual = manager.UpdatePreSelectionStatus(expected, true);
            CollectionAssert.AreEquivalent(expected, actual);
            foreach (var item in expected)
            {
                Assert.True(item.IsPreSelected);
            }

            actual = manager.UpdatePreSelectionStatus(expected, false);
            CollectionAssert.AreEquivalent(expected, actual);
            foreach (var item in expected)
            {
                Assert.False(item.IsPreSelected);
            }
        }

        [TestCase(SelectionTypeEnum.ANY, 1)]
        [TestCase(SelectionTypeEnum.CLICK, 3)]
        [TestCase(SelectionTypeEnum.DRAG, 2)]
        [TestCase(SelectionTypeEnum.KEY, 1)]
        public void ShouldGetModsBySelectionType(SelectionTypeEnum type, int expectedCount)
        {
            List<IBaseSelectionMod> mods = new List<IBaseSelectionMod>();
            mods.AddRange(SelectionManagerTestUtils.GetSomeModsFromType(1, SelectionTypeEnum.ANY));
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
            mods.AddRange(SelectionManagerTestUtils.GetSomeModsFromType(1, SelectionTypeEnum.ANY));
            mods.AddRange(SelectionManagerTestUtils.GetSomeModsFromType(2, SelectionTypeEnum.DRAG));
            mods.AddRange(SelectionManagerTestUtils.GetSomeModsFromType(1, SelectionTypeEnum.KEY));
            var result = manager.GetModsBySelectionType(mods, SelectionTypeEnum.CLICK);
            CollectionAssert.IsEmpty(result);
        }

        [TestCaseSource(nameof(OtherScenarios))]
        public void ShouldFinalizeSelectionCorrectly(SelectionStruct selectionStruct, ModifiersStruct modifiersStruct, ResultStruct resultStruct)
        {
            var mainList = TestUtils.GetSomeObjects(selectionStruct.mainListAmount);
            var arguments = new SelectionArguments(default, modifiersStruct.isPreSelection, TestUtils.GetListByIndex(selectionStruct.oldSelection, mainList), TestUtils.GetListByIndex(selectionStruct.newSelection, mainList), mainList);
            var args = SelectionManagerTestUtils.GetDefaultArgs(arguments);

            var expectedToBeAdded = TestUtils.GetListByIndex(resultStruct.toBeAdded, mainList);

            var result = manager.FinalizeSelection(args);

            for (var i = 0; i < expectedToBeAdded.Count; i++)
            {
                CollectionAssert.Contains(result, expectedToBeAdded[i]);
                Assert.True(expectedToBeAdded[i].IsSelected);
            }
        }


        private static IEnumerable<TestCaseData> OtherScenarios
        {
            get
            {
                foreach (var item in TestUtils.GetCases())
                {
                    yield return new TestCaseData(item.selection, item.modifiers, new ResultStruct()
                    {
                        toBeAdded = item.selection.newSelection,
                        toBeRemoved = item.selection.oldSelection,
                    }).SetName(item.name);
                }
            }
        }
    }

    internal class DerivedClass : BaseSelectionManager
    {
        public override SelectionArgsXP ApplyModifiers(SelectionArgsXP args)
        {
            return args;
        }

        public override SelectionArgsXP GetSelectionArgs(List<ISelectable> currentSelection, List<ISelectable> newSelection, SelectionTypeEnum selectionType, bool isPreSelection)
        {
            return SelectionManagerTestUtils.GetDefaultArgs();
        }
    }
}
