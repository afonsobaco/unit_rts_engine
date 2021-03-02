using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using System.Collections.Generic;
using NSubstitute;
using Tests.Utils;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class DoubleClickSelectionModifierTest
    {
        private DoubleClickSelectionModifier.Modifier modifier;

        [SetUp]
        public void SetUp()
        {
            modifier = Substitute.ForPartsOf<DoubleClickSelectionModifier.Modifier>();
        }

        [Test]
        public void DoubleClickModifierTestSimplePasses()
        {
            Assert.IsNotNull(modifier);
        }
        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifier(int amount, int[] oldSelectionIndexes, int[] newSelectionIndexes)
        {
            ISelectable[] mainList = TestUtils.GetSomeObjects(amount, amount);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);
            ISelectable[] oldSelection = TestUtils.GetListByIndex(oldSelectionIndexes, mainList);

            ISelectable[] expected = null;
            ISelectable selected = null;
            if (newSelection.Length == 1)
            {
                selected = newSelection.First();
                expected = GetMockedSubGroup(mainList, selected);
            }
            else
            {
                expected = newSelection;
            }

            modifier.WhenForAnyArgs(x => x.GetAllGroupableOnScreen(default)).DoNotCallBase();
            modifier.GetAllGroupableOnScreen(default).ReturnsForAnyArgs(expected);

            var result = modifier.Apply(oldSelection, newSelection, selected);

            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldGetAllGroupableOnScreen(int amount, int[] oldSelectionIndexes, int[] newSelectionIndexes)
        {
            ISelectable[] mainList = TestUtils.GetSomeObjects(amount, amount);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);

            ISelectable[] expected = null;
            ISelectable selected = newSelection.Length > 0 ? newSelection.First() : null;
            if (newSelection.Length == 1)
            {
                expected = GetMockedSubGroup(mainList, selected);
            }
            else
            {
                expected = newSelection;
            }

            modifier.WhenForAnyArgs(x => x.GetFromSubGroupOnScreen(default)).DoNotCallBase();
            modifier.GetFromSubGroupOnScreen(default).ReturnsForAnyArgs(expected);

            var result = modifier.GetAllGroupableOnScreen(selected);

            CollectionAssert.AreEquivalent(expected, result);
        }

        private ISelectable[] GetMockedSubGroup(ISelectable[] mainList, ISelectable selectable)
        {
            var evens = new List<ISelectable>();
            var odds = new List<ISelectable>();
            foreach (var item in mainList)
            {
                if (item.Index % 2 == 0) evens.Add(item); else odds.Add(item);
            }
            return evens.Contains(selectable) ? evens.ToArray() : odds.ToArray();
        }

        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetDefaultCases())
                {
                    yield return new TestCaseData(item.amount, item.oldSelection, item.newSelection).SetName(TestUtils.GetCaseName(item));
                }
            }
        }
    }
}
