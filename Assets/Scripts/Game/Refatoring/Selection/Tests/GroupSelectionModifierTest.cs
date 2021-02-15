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
    public class GroupSelectionModifierTest
    {
        private GroupSelectionModifier.Modifier modifier;

        [SetUp]
        public void SetUp()
        {
            modifier = Substitute.ForPartsOf<GroupSelectionModifier.Modifier>();
        }

        [Test]
        public void SelectionGroupModifierTestSimplePasses()
        {
            Assert.IsNotNull(modifier);
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldGroupSelectionToPassedValue(int amount, int[] oldSelectionIndexes, int[] newSelectionIndexes, int[] actualSelection)
        {

            ISelectable[] mainList = TestUtils.GetSomeObjects(amount);
            ISelectable[] oldSelection = TestUtils.GetListByIndex(oldSelectionIndexes, mainList);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);
            ISelectable[] expected = TestUtils.GetListByIndex(actualSelection, mainList);

            mainList.ToList().ForEach(x =>
            {
                x.CompareTo(Arg.Any<ISelectable>()).Returns(
                    args =>
                    {
                        var selectable = args[0] as ISelectable;
                        return (selectable.Index % 2) - (x.Index % 2);
                    }
                );
            });

            var result = modifier.Apply(oldSelection, newSelection, newSelection, SelectionType.ANY);

            CollectionAssert.AreEquivalent(expected, result);
        }

        private static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetDefaultCases())
                {
                    List<int> actualSelection = new List<int>();
                    foreach (var i in item.newSelection)
                    {
                        if (i % 2 == 0)
                        {
                            actualSelection.Add(i);
                        }
                    }
                    foreach (var i in item.newSelection)
                    {
                        if (i % 2 != 0)
                        {
                            actualSelection.Add(i);
                        }
                    }
                    yield return new TestCaseData(item.amount, item.oldSelection, item.newSelection, actualSelection.ToArray()).SetName(TestUtils.GetCaseName(item));
                }
            }
        }

    }
}
