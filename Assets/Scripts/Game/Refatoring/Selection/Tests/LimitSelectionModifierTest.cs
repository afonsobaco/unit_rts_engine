using System.Linq;
using UnityEngine;
using NUnit.Framework;
using RTSEngine.Core;
using NSubstitute;
using RTSEngine.Refactoring;
using System.Collections.Generic;
using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class LimitSelectionModifierTest
    {
        private LimitSelectionModifier.Modifier modifier;

        [SetUp]
        public void SetUp()
        {
            modifier = Substitute.ForPartsOf<LimitSelectionModifier.Modifier>();
        }

        [Test]
        public void SelectionLimitModifierTestSimplePasses()
        {
            Assert.IsNotNull(modifier);
        }
        [TestCaseSource(nameof(Scenarios))]
        public void ShouldLimitSelectionToPassedValue(int amount, int[] oldSelectionIndexes, int[] newSelectionIndexes, int[] actualSelection, int limit)
        {

            ISelectable[] mainList = TestUtils.GetSomeObjects(amount);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);
            ISelectable[] expected = TestUtils.GetListByIndex(actualSelection, mainList);
            modifier.Limit = limit;

            var result = modifier.Apply(newSelection);

            CollectionAssert.AreEquivalent(expected, result);
        }
        private static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetDefaultCases())
                {
                    int limit = Random.Range(1, 5);
                    yield return new TestCaseData(item.amount, item.oldSelection, item.newSelection, item.newSelection.Take(limit).ToArray(), limit).SetName(TestUtils.GetCaseName(item));
                }
            }
        }

    }
}
