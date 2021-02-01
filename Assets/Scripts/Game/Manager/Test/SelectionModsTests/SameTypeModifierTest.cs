using System.Security.Cryptography.X509Certificates;
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

        [TestCase(new int[] { 0 }, new int[] { 0, 1, 2, 3 }, TestName = "Should Get All Units")]
        [TestCase(new int[] { 5 }, new int[] { 4, 5, 6 }, TestName = "Should Get All Buildings")]
        [TestCase(new int[] { 9 }, new int[] { 9 }, TestName = "Should Get Empty")]
        public void ShouldGetAllFromSameTypeThatCanGroup(int[] selectedIndex, int[] expectedResult)
        {
            HashSet<ISelectableObjectBehaviour> mainList = TestUtils.GetSomeObjects<ISelectableObjectBehaviour>(10);
            HashSet<ISelectableObjectBehaviour> newSelection = TestUtils.GetListByIndex(selectedIndex, mainList);
            HashSet<ISelectableObjectBehaviour> expected = TestUtils.GetListByIndex(expectedResult, mainList);
            SetObjectSelectableTypes(mainList);

            modifier.WhenForAnyArgs(x => x.GetAllFromSameType(default, default, default, default, default)).DoNotCallBase();
            modifier.GetAllFromSameType(default, default, default, default, default).ReturnsForAnyArgs(expected);

            SelectionArgsXP args = new SelectionArgsXP(default, newSelection, mainList);
            var result = modifier.GetAllFromSameTypeThatCanGroup(args);

            CollectionAssert.AreEquivalent(expected, result);
        }

        [Test]
        public void ShouldGetAllFromSameType()
        {
            HashSet<ISelectableObjectBehaviour> mainList = TestUtils.GetSomeObjects<ISelectableObjectBehaviour>(10);
            HashSet<ISelectableObjectBehaviour> expected = TestUtils.GetListByIndex(new int[] { 0, 1, 2, 3 }, mainList);
            ISelectableObjectBehaviour selected = TestUtils.GetListByIndex(new int[] { 0 }, mainList).First();

            SetObjectSelectableTypes(mainList);

            modifier.WhenForAnyArgs(x => x.GetFromSameTypeInScreen(default, default, default, default)).DoNotCallBase();
            modifier.GetFromSameTypeInScreen(default, default, default, default).ReturnsForAnyArgs(expected);

            var result = modifier.GetAllFromSameType(selected, mainList, default, default, default);

            CollectionAssert.AreEquivalent(expected, result);
        }

        private static void SetObjectSelectableTypes(HashSet<ISelectableObjectBehaviour> mainList)
        {
            mainList.ToList().ForEach(x =>
            {
                if (x.Index < 4)
                {
                    x.Type = ObjectTypeEnum.UNIT;
                }
                else if (x.Index < 7)
                {
                    x.Type = ObjectTypeEnum.BUILDING;
                }
                else
                {
                    x.Type = ObjectTypeEnum.ENVIRONMENT;
                }
            }); ;
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
