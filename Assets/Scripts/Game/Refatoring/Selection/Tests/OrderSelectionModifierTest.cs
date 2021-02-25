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
    public class OrderSelectionModifierTest
    {
        private OrderSelectionModifier.Modifier modifier;

        public IEqualityComparer<ISelectable> equalityComparer;
        public IComparer<IGrouping<ISelectable, ISelectable>> groupingComparer;

        [SetUp]
        public void SetUp()
        {
            modifier = Substitute.ForPartsOf<OrderSelectionModifier.Modifier>();
            equalityComparer = Substitute.For<IEqualityComparer<ISelectable>>();
            groupingComparer = Substitute.For<IComparer<IGrouping<ISelectable, ISelectable>>>();
            modifier.EqualityComparer = equalityComparer;
            modifier.SubGroupComparer = groupingComparer;
        }

        [Test]
        public void SelectionGroupModifierTestSimplePasses()
        {
            Assert.IsNotNull(modifier);
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldOrderSelectionToPassedValue(int amount, int[] newSelectionIndexes, int[] expectedIndexes)
        {
            ISelectable[] mainList = TestUtils.GetSomeObjects(amount);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);
            ISelectable[] expected = TestUtils.GetListByIndex(expectedIndexes, mainList);

            modifier.WhenForAnyArgs(x => x.OrderSubGroups(default)).DoNotCallBase();
            modifier.OrderSubGroups(Arg.Any<ISelectable[]>()).Returns(expected);

            var result = modifier.Apply(newSelection);

            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldOrderSubGroups(int amount, int[] newSelectionIndexes, int[] expectedIndexes)
        {
            ISelectable[] mainList = TestUtils.GetSomeObjects(amount);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);
            ISelectable[] expected = TestUtils.GetListByIndex(expectedIndexes, mainList);

            equalityComparer.GetHashCode(default).ReturnsForAnyArgs(x =>
            {
                return ((x[0] as ISelectable).Index % 2).GetHashCode();
            });
            groupingComparer.Compare(default, default).ReturnsForAnyArgs(x =>
            {
                IGrouping<ISelectable, ISelectable> first = (x[0] as IGrouping<ISelectable, ISelectable>);
                IGrouping<ISelectable, ISelectable> second = (x[1] as IGrouping<ISelectable, ISelectable>);
                return (first.Key.Index % 2) - (second.Key.Index % 2);
            });

            var result = modifier.OrderSubGroups(newSelection);

            CollectionAssert.AreEqual(expected, result);
        }

        private static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetDefaultCases())
                {
                    List<int> expected = new List<int>();
                    foreach (var i in item.newSelection)
                    {
                        if (i % 2 == 0)
                        {
                            expected.Add(i);
                        }
                    }
                    foreach (var i in item.newSelection)
                    {
                        if (i % 2 != 0)
                        {
                            expected.Add(i);
                        }
                    }
                    yield return new TestCaseData(item.amount, item.newSelection, expected.ToArray()).SetName(TestUtils.GetCaseName(item));
                }
            }
        }

    }
}
