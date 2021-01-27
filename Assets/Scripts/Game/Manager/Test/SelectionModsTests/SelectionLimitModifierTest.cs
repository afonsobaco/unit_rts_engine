using System.Linq;
using UnityEngine;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Manager;
using RTSEngine.Manager.SelectionMods.Impls;
using System.Collections.Generic;
using Tests.Utils;

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
            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.ANY, false, new List<ISelectable>(), new List<ISelectable>(), new List<ISelectable>());
            SelectionModifierArguments modifierArguments = new SelectionModifierArguments(false, false, Vector2.zero, Vector2.zero);
            SelectionArgsXP args = new SelectionArgsXP(arguments, modifierArguments);

            var result = Modifier.Apply(20, args);
            Assert.AreEqual(args, result);
        }


        [TestCaseSource(nameof(Scenarios))]
        public void ShouldLimitSelectionToPassedValue(SelectionStruct selectionStruct, ModifiersStruct modifiersStruct, ResultStruct resultStruct, int limit)
        {
            List<ISelectable> mainList = TestUtils.GetSomeObjects(selectionStruct.mainListAmount);

            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.ANY, false, new List<ISelectable>(), TestUtils.GetListByIndex(selectionStruct.newSelection, mainList), mainList);
            SelectionModifierArguments modifierArguments = new SelectionModifierArguments(false, false, Vector2.zero, Vector2.zero);
            SelectionArgsXP args = new SelectionArgsXP(arguments, modifierArguments);

            args = Modifier.Apply(limit, args);

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

        public SelectionLimitModifier.SelectionModifier Modifier { get => modifier; set => modifier = value; }
    }
}
