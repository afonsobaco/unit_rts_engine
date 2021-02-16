using UnityEngine;
using NUnit.Framework;
using RTSEngine.Refactoring;
using RTSEngine.Core;
using Tests.Utils;
using System.Collections.Generic;
using NSubstitute;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class GroupRestrictionModifierTest
    {

        private GroupRestrictionSelectionModifier.Modifier modifier;
        private IModifier groupRestriction;
        [SetUp]
        public void SetUp()
        {
            modifier = Substitute.ForPartsOf<GroupRestrictionSelectionModifier.Modifier>();
            groupRestriction = Substitute.For<IModifier>();
            modifier.GroupRestriction = groupRestriction;
            groupRestriction.Apply(Arg.Any<ISelectable[]>()).Returns(args =>
            {
                List<ISelectable> selectables = new List<ISelectable>(args[0] as ISelectable[]);
                selectables.RemoveAll(x => x.Index >= 7);
                selectables.ForEach(x => Debug.Log(x.Index));
                return selectables.ToArray();
            });
        }

        [Test]
        public void SelectionLimitModifierTestSimplePasses()
        {
            Assert.IsNotNull(modifier);
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifier(int amount, int[] oldSelectionIndexes, int[] newSelectionIndexes, int[] actualSelection)
        {
            ISelectable[] mainList = TestUtils.GetSomeObjects(amount);
            ISelectable[] oldSelection = TestUtils.GetListByIndex(oldSelectionIndexes, mainList);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);
            ISelectable[] expected = TestUtils.GetListByIndex(actualSelection, mainList);

            var result = modifier.Apply(oldSelection, newSelection, newSelection, SelectionType.ANY);
            Debug.Log(expected.GetType().Name);
            Debug.Log(">" + result.GetType().Name);
            CollectionAssert.AreEquivalent(expected, result);
        }

        private static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetDefaultCases())
                {
                    List<int> expected = new List<int>();
                    foreach (var a in item.newSelection)
                    {
                        if (a < 7)
                            expected.Add(a);
                    }
                    yield return new TestCaseData(item.amount, item.oldSelection, item.newSelection, expected.ToArray()).SetName(TestUtils.GetCaseName(item));
                }
            }
        }
    }
}
