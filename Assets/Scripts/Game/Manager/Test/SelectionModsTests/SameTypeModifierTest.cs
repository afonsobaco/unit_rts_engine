using UnityEngine;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Manager;
using System.Collections.Generic;
using NSubstitute;
using Tests.Utils;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class SameTypeModifierTest
    {
        private SameTypeSelectionModifier modifier;

        [SetUp]
        public void SetUp()
        {
            Modifier = Substitute.ForPartsOf<SameTypeSelectionModifier>();
        }

        [Test]
        public void SameTypeModifierTestSimplePasses()
        {
            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.ANY, false, new List<ISelectable>(), new List<ISelectable>(), new List<ISelectable>());
            SelectionModifierArguments modifierArguments = new SelectionModifierArguments();
            SelectionArgsXP args = new SelectionArgsXP(arguments, modifierArguments);

            var result = Modifier.Apply(args, SameTypeSelectionModeEnum.DISTANCE);
            Assert.AreEqual(args, result);
        }


        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifierOnClick(SelectionStruct selection, ModifiersStruct modifier, ResultStruct result)
        {

            List<ISelectable> mainList = TestUtils.GetSomeObjects(selection.mainListAmount);
            List<ISelectable> oldSelection = TestUtils.GetListByIndex(selection.oldSelection, mainList);
            List<ISelectable> newSelection = TestUtils.GetListByIndex(selection.newSelection, mainList);
            List<ISelectable> sameTypeList = new List<ISelectable>();
            List<ISelectable> expected = new List<ISelectable>();
            if (selection.newSelection.Length > 0)
            {
                if (selection.additionalInfo.group_evens.Contains(selection.newSelection[0]))
                {
                    sameTypeList = TestUtils.GetListByIndex(selection.additionalInfo.group_evens, mainList);
                }
                else
                {
                    sameTypeList = TestUtils.GetListByIndex(selection.additionalInfo.group_odds, mainList); ;
                }
            }
            if (modifier.isSameType)
            {
                expected = TestUtils.GetListByIndex(result.toBeAdded, mainList);
            }
            else
            {
                expected = TestUtils.GetListByIndex(selection.newSelection, mainList);
            }

            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.CLICK, false, oldSelection, newSelection, mainList);
            SelectionModifierArguments modifierArguments = new SelectionModifierArguments(modifier.isSameType, false, Vector2.zero, new Vector2(800, 600));
            SelectionArgsXP args = new SelectionArgsXP(arguments, modifierArguments);

            Modifier.When(x => x.GetAllFromSameTypeOnScreen(default, default)).DoNotCallBase();
            Modifier.GetAllFromSameTypeOnScreen(Arg.Any<SelectionArgsXP>(), Arg.Any<SameTypeSelectionModeEnum>()).Returns(sameTypeList);

            args = Modifier.Apply(args, SameTypeSelectionModeEnum.DISTANCE);

            CollectionAssert.AreEquivalent(expected, args.Result.ToBeAdded);

        }

        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetCustomCases(new ModifiersStruct(false, false, true), true))
                {
                    int[] toBeAdded = new int[] { };
                    if (item.selection.newSelection.Length == 1)
                    {
                        if (item.selection.oldSelection.Contains(item.selection.newSelection[0]) && item.selection.oldSelection.Length > 1)
                        {
                            toBeAdded = item.selection.oldSelection.ToList().FindAll(x => !item.selection.additionalInfo.group_evens.Contains(x)).ToArray();
                        }
                        else
                        {
                            // list.Add(new SelectionStruct(10, new int[] { 0 }, new int[] { 1 }, addInfo));
                            if (item.selection.additionalInfo.group_evens.Contains(item.selection.newSelection[0]))
                            {
                                toBeAdded = item.selection.additionalInfo.group_evens;
                            }
                            else
                            {
                                toBeAdded = item.selection.additionalInfo.group_odds;
                            }
                        }
                    }
                    else if (item.selection.newSelection.Length > 1)
                    {
                        toBeAdded = item.selection.newSelection;
                    }
                    yield return new TestCaseData(item.selection, item.modifiers, new ResultStruct { toBeAdded = toBeAdded }).SetName(item.name);
                }
            }
        }

        public SameTypeSelectionModifier Modifier { get => modifier; set => modifier = value; }
    }
}
