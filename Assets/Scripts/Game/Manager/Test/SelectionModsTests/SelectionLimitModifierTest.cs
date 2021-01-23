using NUnit.Framework;
using RTSEngine.Core.Impls;
using UnityEngine;
using RTSEngine.Manager.Impls.SelectionMods.Impls;
using RTSEngine.Manager.Impls;
using RTSEngine.Manager.Enums;
using System.Collections.Generic;
using NSubstitute;
using System.Collections;

namespace Tests
{
    [TestFixture]
    public class SelectionLimitModifierTest
    {
        private SelectionLimitModifier.Modifier modifier;

        [SetUp]
        public void SetUp()
        {
            modifier = new SelectionLimitModifier.Modifier();
        }

        [Test]
        public void SelectionLimitModifierTestSimplePasses()
        {
            ISelectionArgsXP<SelectableObject, SelectionTypeEnum> args = ModifierTestUtils.GetDefaultArgs();
            var result = modifier.Apply(20, args);
            Assert.AreEqual(args, result);
        }


        [TestCaseSource(nameof(Scenarios))]
        public void ShouldLimitToBeRemovedToPassedValue(int mainListCount, int limit, int[] oldSelection, int[] newSelection, int[] toBeAdded, int[] toBeRemoved, int[] expectedToBeAddedResult)
        {
            ISelectionArgsXP<SelectableObject, SelectionTypeEnum> args = ModifierTestUtils.GetDefaultArgs();
            List<SelectableObject> mainList = ModifierTestUtils.GetSomeObjects(mainListCount);
            args.OldSelection.AddRange(ModifierTestUtils.GetListByIndex(oldSelection, mainList));
            args.NewSelection.AddRange(ModifierTestUtils.GetListByIndex(newSelection, mainList));
            args.ToBeAdded.AddRange(ModifierTestUtils.GetListByIndex(toBeAdded, mainList));
            args.ToBeRemoved.AddRange(ModifierTestUtils.GetListByIndex(toBeRemoved, mainList));
            var result = modifier.Apply(limit, args);
            CollectionAssert.AreEqual(ModifierTestUtils.GetListByIndex(expectedToBeAddedResult, mainList), result.ToBeAdded);
        }


        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                //----------- mainListCount, limit, -oldSelection, ------------newSelection,--------------- toBeAdded, --toBeRemoved, expectedToBeAddedResult
                yield return new TestCaseData(5, 2, new int[] { }, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3, 4 }, new int[] { }, new int[] { 1, 2 });
                yield return new TestCaseData(5, 2, new int[] { }, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2 }, new int[] { }, new int[] { 1, 2 });
                yield return new TestCaseData(5, 2, new int[] { }, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3 }, new int[] { 3, 4 }, new int[] { 1, 2 });
                yield return new TestCaseData(5, 2, new int[] { 1, 3 }, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2 }, new int[] { 3 }, new int[] { 1, 2 });
                yield return new TestCaseData(5, 2, new int[] { 1, 3 }, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2 }, new int[] { 3, 4 }, new int[] { 1, 2 });
                yield return new TestCaseData(5, 2, new int[] { }, new int[] { 1 }, new int[] { 1, 3, 2 }, new int[] { }, new int[] { 1, 3 });
                yield return new TestCaseData(5, 5, new int[] { }, new int[] { 1 }, new int[] { 1, 3, 2 }, new int[] { }, new int[] { 1, 3, 2 });

            }
        }
    }
}
