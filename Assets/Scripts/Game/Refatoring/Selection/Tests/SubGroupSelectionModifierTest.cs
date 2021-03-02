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
    public class SubGroupSelectionModifierTest
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
        public void ShouldApplyModifierIndividual(int amount, int[] oldSelectionIndexes, int[] newSelectionIndexes)
        {
            ISelectable[] mainList = TestUtils.GetSomeObjects(amount, amount);
            ISelectable[] oldSelection = TestUtils.GetListByIndex(oldSelectionIndexes, mainList);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);

            ISelectable[] expected = GetMockedExpected(mainList, newSelection);

            modifier.WhenForAnyArgs(x => x.FilterBySubGroup(default, default)).DoNotCallBase();
            modifier.FilterBySubGroup(default, default).ReturnsForAnyArgs(expected);

            modifier.WhenForAnyArgs(x => x.GetAllOnScreenArea()).DoNotCallBase();
            modifier.GetAllOnScreenArea().ReturnsForAnyArgs(mainList);

            var result = modifier.Apply(true, oldSelection, newSelection, SelectionType.INDIVIDUAL);

            CollectionAssert.AreEquivalent(expected, result);
            if (newSelection.Length == 1)
                modifier.Received().FilterBySubGroup(Arg.Is(mainList), Arg.Is(newSelection.First()));
            else
                modifier.DidNotReceiveWithAnyArgs().FilterBySubGroup(default, default);
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifierIndividualOnSelection(int amount, int[] oldSelectionIndexes, int[] newSelectionIndexes)
        {
            ISelectable[] mainList = TestUtils.GetSomeObjects(amount, amount);
            ISelectable[] oldSelection = TestUtils.GetListByIndex(oldSelectionIndexes, mainList);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);

            ISelectable[] expected = GetMockedExpected(mainList, newSelection);

            modifier.WhenForAnyArgs(x => x.FilterBySubGroup(default, default)).DoNotCallBase();
            modifier.FilterBySubGroup(default, default).ReturnsForAnyArgs(expected);

            modifier.WhenForAnyArgs(x => x.GetAllOnScreenArea()).DoNotCallBase();
            modifier.GetAllOnScreenArea().ReturnsForAnyArgs(mainList);

            var result = modifier.Apply(true, oldSelection, newSelection, SelectionType.INDIVIDUAL_ON_SELECTION);

            CollectionAssert.AreEquivalent(expected, result);
            if (newSelection.Length == 1)
                modifier.Received().FilterBySubGroup(Arg.Is(oldSelection), Arg.Is(newSelection.First()));
            else
                modifier.DidNotReceiveWithAnyArgs().FilterBySubGroup(default, default);
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

        private ISelectable[] GetMockedExpected(ISelectable[] mainList, ISelectable[] newSelection)
        {
            ISelectable[] expected = null;
            if (newSelection.Length == 1)
            {
                expected = GetMockedSubGroup(mainList, newSelection.First());
            }
            else
            {
                expected = newSelection;
            }
            return expected;
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
