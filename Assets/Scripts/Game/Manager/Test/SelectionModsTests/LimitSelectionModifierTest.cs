using System.Linq;
using UnityEngine;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Manager;
using System.Collections.Generic;
using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class LimitModifierTest
    {
        private LimitSelectionModifier modifier;

        [SetUp]
        public void SetUp()
        {
            Modifier = new LimitSelectionModifier();
        }

        [Test]
        public void SelectionLimitModifierTestSimplePasses()
        {
            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.ANY, false, new List<ISelectable>(), new List<ISelectable>(), new List<ISelectable>());
            SelectionModifierArguments modifierArguments = new SelectionModifierArguments();
            SelectionArgsXP args = new SelectionArgsXP(arguments, modifierArguments);

            var result = Modifier.Apply(args, 20);
            Assert.AreEqual(args, result);
        }


        [TestCaseSource(nameof(Scenarios))]
        public void ShouldLimitSelectionToPassedValue(SelectionStruct selectionStruct, ModifiersStruct modifiersStruct, ResultStruct resultStruct, int limit)
        {
            List<ISelectable> mainList = TestUtils.GetSomeObjects(selectionStruct.mainListAmount);

            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.ANY, false, new List<ISelectable>(), TestUtils.GetListByIndex(selectionStruct.newSelection, mainList), mainList);
            SelectionModifierArguments modifierArguments = new SelectionModifierArguments();
            SelectionArgsXP args = new SelectionArgsXP(arguments, modifierArguments);

            args = Modifier.Apply(args, limit);
            List<ISelectable> expected = TestUtils.GetListByIndex(resultStruct.toBeAdded, mainList);
            CollectionAssert.AreEquivalent(expected, args.Result.ToBeAdded);
        }



        private static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetDefaultCases())
                {
                    const int limit = 3;
                    yield return new TestCaseData(item.selection, item.modifiers, new ResultStruct()
                    {
                        toBeAdded = item.selection.newSelection.Take(limit).ToArray(),
                    }, limit).SetName(item.name);
                }
            }
        }

        public LimitSelectionModifier Modifier { get => modifier; set => modifier = value; }
    }
}
