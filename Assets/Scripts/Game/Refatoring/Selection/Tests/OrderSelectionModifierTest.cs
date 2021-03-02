using System.Linq;
using UnityEngine;
using NUnit.Framework;
using RTSEngine.Core;
using NSubstitute;
using RTSEngine.Refactoring;
using System.Collections.Generic;
using Tests.Utils;
using Zenject;

namespace Tests
{
    [TestFixture]
    public class OrderSelectionModifierTest : ZenjectUnitTestFixture
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
            Container.BindInstance(equalityComparer).AsSingle();
            Container.BindInstance(groupingComparer).AsSingle();
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
