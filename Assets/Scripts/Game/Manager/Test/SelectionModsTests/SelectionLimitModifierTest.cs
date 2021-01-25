using UnityEngine;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Manager;
using RTSEngine.Manager.SelectionMods.Impls;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class SelectionLimitModifierTest
    {
        private SelectionLimitModifier.SelectionModifier modifier;

        [SetUp]
        public void SetUp()
        {
            Modifier = new SelectionLimitModifier.SelectionModifier();
        }

        [Test]
        public void SelectionLimitModifierTestSimplePasses()
        {
            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.ALL, false, new List<ISelectable>(), new List<ISelectable>(), new List<ISelectable>());
            SelectionModifierArguments modifierArguments = new SelectionModifierArguments(false, false, Vector2.zero, Vector2.zero);
            SelectionArgsXP args = new SelectionArgsXP(arguments, modifierArguments);

            var result = Modifier.Apply(20, args);
            Assert.AreEqual(args, result);
        }


        [TestCaseSource(nameof(Scenarios))]
        public void ShouldLimitToBeRemovedToPassedValue(int mainListCount, int limit, int[] newSelection, int[] expectedToBeAddedResult)
        {
            //TODO redo that
            List<ISelectable> mainList = ModifierTestUtils.GetSomeObjects(mainListCount);

            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.ALL, false, new List<ISelectable>(), ModifierTestUtils.GetListByIndex(newSelection, mainList), mainList);
            SelectionModifierArguments modifierArguments = new SelectionModifierArguments(false, false, Vector2.zero, Vector2.zero);
            SelectionArgsXP args = new SelectionArgsXP(arguments, modifierArguments);

            args = Modifier.Apply(limit, args);

            List<ISelectable> expected = ModifierTestUtils.GetListByIndex(expectedToBeAddedResult, mainList);
            CollectionAssert.AreEquivalent(expected, args.Result.ToBeAdded);
        }


        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                yield return new TestCaseData(5, 2, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2 });
                yield return new TestCaseData(5, 4, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3, 4 });
                yield return new TestCaseData(5, 5, new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 });
                yield return new TestCaseData(5, 3, new int[] { }, new int[] { });
            }
        }

        public SelectionLimitModifier.SelectionModifier Modifier { get => modifier; set => modifier = value; }
    }
}
