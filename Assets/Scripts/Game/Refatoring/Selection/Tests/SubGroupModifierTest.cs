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
    public class SubGroupModifierTest
    {
        private SubGroupSelectionModifier.Modifier modifier;

        [SetUp]
        public void SetUp()
        {
            modifier = Substitute.ForPartsOf<SubGroupSelectionModifier.Modifier>();
        }

        [Test]
        public void SubGroupModifierTestSimplePasses()
        {
            Assert.IsNotNull(modifier);
        }


        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifier(int amount, int[] newSelectionIndexes)
        {
            ISelectable[] mainList = SelectionTestUtils.GetSomeSelectable(amount, amount);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);

            ISelectable[] expected = null;
            if (newSelection.Length == 1)
            {
                expected = GetMockedSubGroup(mainList, newSelection.First());
            }
            else
            {
                expected = newSelection;
            }

            modifier.WhenForAnyArgs(x => x.GetAllGroupableOnScreen(default)).DoNotCallBase();
            modifier.GetAllGroupableOnScreen(default).ReturnsForAnyArgs(expected);

            var result = modifier.Apply(true, newSelection);

            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldGetAllGroupableOnScreen(int amount, int[] newSelectionIndexes)
        {
            ISelectable[] mainList = SelectionTestUtils.GetSomeSelectable(amount, amount);
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
            modifier.GetFromSubGroupOnScreen(default).ReturnsForAnyArgs(expected.ToList());

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
                    yield return new TestCaseData(item.amount, item.newSelection).SetName(TestUtils.GetCaseName(item));
                }
            }
        }
    }
}
