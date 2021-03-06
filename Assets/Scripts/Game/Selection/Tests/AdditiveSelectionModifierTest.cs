using System.Linq;
using NUnit.Framework;
using RTSEngine.Core;
using System.Collections.Generic;
using Tests.Utils;
using NSubstitute;
using RTSEngine.RTSSelection;

namespace Tests
{
    [TestFixture]
    public class AdditiveSelectionModifierTest
    {
        private AdditiveSelectionModifier.Modifier modifier;

        [SetUp]
        public void SetUp()
        {
            modifier = Substitute.ForPartsOf<AdditiveSelectionModifier.Modifier>();
        }

        [Test]
        public void AdditiveModifierTestSimplePasses()
        {
            Assert.IsNotNull(modifier);
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifierClick(int amount, int[] oldSelectionIndexes, int[] newSelectionIndexes, int[] actualSelection)
        {
            ISelectable[] mainList = TestUtils.GetSomeObjects(amount);
            ISelectable[] oldSelection = TestUtils.GetListByIndex(oldSelectionIndexes, mainList);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);
            ISelectable[] expected = TestUtils.GetListByIndex(actualSelection, mainList);

            var result = modifier.Apply(true, oldSelection, newSelection, newSelection);

            CollectionAssert.AreEquivalent(expected, result);

        }

        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetDefaultCases())
                {
                    int[] toBeAdded = item.newSelection;

                    bool containsAll = toBeAdded.Length > 0 && toBeAdded.ToList().TrueForAll(x => item.oldSelection.ToList().Contains(x));
                    bool differentCounts = item.oldSelection.Length != toBeAdded.Length;

                    var newToBeAdded = item.oldSelection.Union(toBeAdded).ToList();

                    if (containsAll && differentCounts)
                    {
                        newToBeAdded.RemoveAll(x => toBeAdded.Contains(x));
                    }

                    toBeAdded = newToBeAdded.ToArray();
                    yield return new TestCaseData(item.amount, item.oldSelection, item.newSelection, toBeAdded).SetName(TestUtils.GetCaseName(item));

                }
            }
        }

    }
}
