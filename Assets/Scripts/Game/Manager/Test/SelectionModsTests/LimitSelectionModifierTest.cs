using System.Linq;
using UnityEngine;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Manager;
using System.Collections.Generic;
using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class LimitModifierTest
    {
        private LimitSelectionModifier modifier;

        [SetUp]
        public void SetUp()
        {
            modifier = new LimitSelectionModifier();
        }

        [Test]
        public void SelectionLimitModifierTestSimplePasses()
        {
            SelectionArgsXP args = new SelectionArgsXP(new HashSet<ISelectableObjectBehaviour>(), new HashSet<ISelectableObjectBehaviour>(), new HashSet<ISelectableObjectBehaviour>());
            var result = modifier.Apply(args);
            Assert.AreEqual(args, result);
        }


        [TestCaseSource(nameof(Scenarios))]
        public void ShouldLimitSelectionToPassedValue(SelectionStruct selectionStruct, ModifiersStruct modifiersStruct, ResultStruct resultStruct, int limit)
        {
            HashSet<ISelectableObjectBehaviour> mainList = TestUtils.GetSomeObjects<ISelectableObjectBehaviour>(selectionStruct.mainListAmount);
            HashSet<ISelectableObjectBehaviour> oldSelection = TestUtils.GetListByIndex(selectionStruct.oldSelection, mainList);
            HashSet<ISelectableObjectBehaviour> newSelection = TestUtils.GetListByIndex(selectionStruct.newSelection, mainList);

            SelectionArgsXP args = new SelectionArgsXP(oldSelection, newSelection, mainList);
            modifier.MaxLimit = limit;

            args = modifier.Apply(args);
            HashSet<ISelectableObjectBehaviour> expected = TestUtils.GetListByIndex(resultStruct.expected, mainList);
            CollectionAssert.AreEquivalent(expected, args.ToBeAdded);
        }



        private static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetDefaultCases())
                {
                    const int limit = 3;
                    yield return new TestCaseData(item.selection, item.modifiers, new ResultStruct()
                    {
                        expected = item.selection.newSelection.Take(limit).ToArray(),
                    }, limit).SetName(item.name);
                }
            }
        }

    }
}
