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

        private ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> selectionManager;

        [SetUp]
        public void SetUp()
        {
            selectionManager = Substitute.For<ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum>>();
            modifier = Substitute.ForPartsOf<SameTypeSelectionModifier>(new object[] { selectionManager });
        }

        [Test]
        public void SameTypeModifierTestSimplePasses()
        {
            SelectionArgsXP args = new SelectionArgsXP(new HashSet<ISelectableObjectBehaviour>(), new HashSet<ISelectableObjectBehaviour>(), new HashSet<ISelectableObjectBehaviour>());

            var result = modifier.Apply(args);
            Assert.AreEqual(args, result);
        }


        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifier(SelectionStruct selectionStruct, ModifiersStruct modifierStruct, ResultStruct resultStruct)
        {
            selectionManager.IsSameType().Returns(modifierStruct.isSameType);

            HashSet<ISelectableObjectBehaviour> mainList = TestUtils.GetSomeObjects<ISelectableObjectBehaviour>(selectionStruct.mainListAmount);
            HashSet<ISelectableObjectBehaviour> oldSelection = TestUtils.GetListByIndex(selectionStruct.oldSelection, mainList);
            HashSet<ISelectableObjectBehaviour> newSelection = TestUtils.GetListByIndex(selectionStruct.newSelection, mainList);
            HashSet<ISelectableObjectBehaviour> expected = TestUtils.GetListByIndex(resultStruct.expected, mainList);

            HashSet<ISelectableObjectBehaviour> sameTypeList = new HashSet<ISelectableObjectBehaviour>();
            if (modifierStruct.isSameType && selectionStruct.newSelection.Length > 0)
            {
                if (selectionStruct.additionalInfo.group_evens.Contains(selectionStruct.newSelection[0])) //TODO should not work with newSelection
                {
                    sameTypeList = TestUtils.GetListByIndex(selectionStruct.additionalInfo.group_evens, mainList);
                }
                else
                {
                    sameTypeList = TestUtils.GetListByIndex(selectionStruct.additionalInfo.group_odds, mainList); ;
                }
            }
            SelectionArgsXP args = new SelectionArgsXP(oldSelection, newSelection, mainList);
            modifier.When(x => x.GetAllFromSameTypeThatCanGroup(Arg.Any<SelectionArgsXP>())).DoNotCallBase();
            modifier.GetAllFromSameTypeThatCanGroup(Arg.Any<SelectionArgsXP>()).Returns(sameTypeList);

            args = modifier.Apply(args);

            CollectionAssert.AreEquivalent(expected, args.ToBeAdded);

        }

        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetCustomCases(new ModifiersStruct(false, true), true))
                {
                    int[] toBeAdded = item.selection.newSelection;
                    if (item.modifiers.isSameType && toBeAdded.Length == 1) //click
                    {
                        if (item.selection.additionalInfo.group_evens.Contains(item.selection.newSelection[0]))
                        {
                            toBeAdded = item.selection.additionalInfo.group_evens;
                        }
                        else
                        {
                            toBeAdded = item.selection.additionalInfo.group_odds;
                        }
                    }
                    yield return new TestCaseData(item.selection, item.modifiers, new ResultStruct { expected = toBeAdded }).SetName(item.name);
                }
            }
        }
    }
}
