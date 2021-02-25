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
    public class CanBeGroupedModifierTest
    {

        private CanBeGroupedSelectionModifier.Modifier modifier;
        private IModifierHelper canBeGroupedHelper;
        [SetUp]
        public void SetUp()
        {
            modifier = Substitute.ForPartsOf<CanBeGroupedSelectionModifier.Modifier>();
            canBeGroupedHelper = Substitute.For<IModifierHelper>();
            modifier.CanBeGroupedHelper = canBeGroupedHelper;
            canBeGroupedHelper.Apply(Arg.Any<ISelectable[]>()).Returns(args =>
            {
                List<ISelectable> selectables = new List<ISelectable>(args[0] as ISelectable[]);
                selectables.RemoveAll(x => x.Index >= 7);
                return selectables.ToArray();
            });
        }

        [Test]
        public void SelectionLimitModifierTestSimplePasses()
        {
            Assert.IsNotNull(modifier);
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifier(int amount, int[] newSelectionIndexes, int[] actualSelection)
        {
            ISelectable[] mainList = TestUtils.GetSomeObjects(amount);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);
            ISelectable[] expected = TestUtils.GetListByIndex(actualSelection, mainList);

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
                    foreach (var a in item.newSelection)
                    {
                        if (a < 7)
                            expected.Add(a);
                    }
                    yield return new TestCaseData(item.amount, item.newSelection, expected.ToArray()).SetName(TestUtils.GetCaseName(item));
                }
            }
        }
    }
}
