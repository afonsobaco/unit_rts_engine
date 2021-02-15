using UnityEngine;
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
    public class SameTypeModifierTest
    {
        private SameTypeSelectionModifier.Modifier modifier;

        [SetUp]
        public void SetUp()
        {
            modifier = Substitute.ForPartsOf<SameTypeSelectionModifier.Modifier>();
            modifier.Active = true;
        }

        [Test]
        public void SameTypeModifierTestSimplePasses()
        {
            Assert.IsNotNull(modifier);
        }


        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifier(int amount, int[] oldSelectionIndexes, int[] newSelectionIndexes)
        {
            ISelectable[] mainList = TestUtils.GetSomeObjects(amount, amount);
            ISelectable[] oldSelection = TestUtils.GetListByIndex(oldSelectionIndexes, mainList);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);

            List<ISelectable> expected = new List<ISelectable>(newSelection);
            if (newSelection.Length == 1)
            {
                expected = GetMockedSameType(mainList, newSelection.First());
            }
            modifier.WhenForAnyArgs(x => x.GetAllFromSameType(default)).DoNotCallBase();
            modifier.GetAllFromSameType(default).ReturnsForAnyArgs(expected.ToList());

            var result = modifier.Apply(oldSelection, newSelection, newSelection, SelectionType.ANY);

            CollectionAssert.AreEquivalent(expected.ToArray(), result);
        }

        private static List<ISelectable> GetMockedSameType(ISelectable[] mainList, ISelectable selectable)
        {
            var evens = new List<ISelectable>();
            var odds = new List<ISelectable>();
            foreach (var item in mainList)
            {
                if (item.Index % 2 == 0) evens.Add(item); else odds.Add(item);
            }
            return evens.Contains(selectable) ? evens : odds;
        }

        [TestCase(10, new int[] { 0 }, new int[] { 0, 1, 2, 3 }, TestName = "Should Get All Units")]
        [TestCase(10, new int[] { 5 }, new int[] { 4, 5, 6 }, TestName = "Should Get All Buildings")]
        [TestCase(10, new int[] { 9 }, new int[] { }, TestName = "Should Get Empty")]
        public void ShouldGetFromSameTypeInViewport(int amount, int[] newSelectionIndexes, int[] expectedIndexes)
        {
            ISelectable[] mainList = TestUtils.GetSomeObjects(amount, amount - 3);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);
            ISelectable[] expected = TestUtils.GetListByIndex(expectedIndexes, mainList);

            modifier.InitialViewportPoint = Vector2.zero;
            modifier.FinalViewportPoint = Vector2.one;

            modifier.WhenForAnyArgs(x => x.GetMainList()).DoNotCallBase();
            modifier.GetMainList().ReturnsForAnyArgs(new HashSet<ISelectable>(mainList));
            modifier.WhenForAnyArgs(x => x.GetAllInsideViewportPoints(default)).DoNotCallBase();
            modifier.GetAllInsideViewportPoints(Arg.Any<ISelectable>()).Returns(mainList.ToList());

            foreach (var item in mainList)
            {
                if (item is IGroupable)
                {
                    IGroupable groupable = (item as IGroupable);
                    groupable.IsCompatible(Arg.Any<ISelectable>()).Returns(x =>
                    {
                        ISelectable selectable = x[0] as ISelectable;
                        if (selectable.Index < 4 && item.Index < 4)
                        {
                            return true;
                        }
                        else if (selectable.Index >= 4 && item.Index >= 4 && selectable.Index < 7 && item.Index < 7)
                        {
                            return true;
                        }
                        return false;
                    }
                    );
                }
            }

            var result = modifier.GetFromSameTypeInViewport(newSelection.First());

            CollectionAssert.AreEquivalent(expected, result);
        }

        [Test]
        public void ShouldGetAllFromSameType()
        {
            ISelectable[] mainList = TestUtils.GetSomeObjects(10, 10);
            ISelectable[] expected = TestUtils.GetListByIndex(new int[] { 0, 1, 2, 3 }, mainList);
            ISelectable selected = TestUtils.GetListByIndex(new int[] { 0 }, mainList).First();

            modifier.WhenForAnyArgs(x => x.GetFromSameTypeInViewport(default)).DoNotCallBase();
            modifier.GetFromSameTypeInViewport(default).ReturnsForAnyArgs(expected.ToList());

            var result = modifier.GetAllFromSameType(selected);

            CollectionAssert.AreEquivalent(expected, result);
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
