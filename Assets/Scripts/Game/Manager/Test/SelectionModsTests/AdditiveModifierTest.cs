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
    public class AdditiveModifierTest
    {
        private AdditiveSelectionModifier modifier;

        [SetUp]
        public void SetUp()
        {
            Modifier = new AdditiveSelectionModifier();
        }

        [Test]
        public void AdditiveModifierTestSimplePasses()
        {
            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.ANY, false, new List<ISelectable>(), new List<ISelectable>(), new List<ISelectable>());
            SelectionModifierArguments modifierArguments = new SelectionModifierArguments(false, false, Vector2.zero, Vector2.zero);
            SelectionArgsXP args = new SelectionArgsXP(arguments, modifierArguments);

            var result = Modifier.Apply(args);
            Assert.AreEqual(args, result);
        }

        [Test]
        public void ShouldApplyModifierAny()
        {
            SelectionModifierArguments modifierArgs = new SelectionModifierArguments(default, true, default, default);
            SelectionArgsXP args = new SelectionArgsXP(new SelectionArguments(SelectionTypeEnum.ANY, false, new List<ISelectable>(), new List<ISelectable>(), new List<ISelectable>()), modifierArgs);

            args = Modifier.Apply(args);

            List<ISelectable> expectedToBeAddedResult = new List<ISelectable>();
            CollectionAssert.AreEquivalent(expectedToBeAddedResult, args.Result.ToBeAdded);

        }



        // [TestCaseSource(nameof(Scenarios))]
        // public void ShouldApplyModifierDrag(SelectionStruct selectionStruct, ModifiersStruct modifiersStruct, ResultStruct resultStruct)
        // {
        //     List<ISelectable> mainList = TestUtils.GetSomeObjects(selectionStruct.mainListAmount);

        //     SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.DRAG, modifiersStruct.isPreSelection, TestUtils.GetListByIndex(selectionStruct.oldSelection, mainList), TestUtils.GetListByIndex(selectionStruct.newSelection, mainList), mainList);
        //     SelectionModifierArguments modifierArguments = new SelectionModifierArguments(modifiersStruct.isSameType, modifiersStruct.isAdditive, Vector2.zero, new Vector2(800, 600));
        //     SelectionArgsXP args = new SelectionArgsXP(arguments, modifierArguments);

        //     args = Modifier.Apply(args);

        //     List<ISelectable> expectedToBeAddedResult = TestUtils.GetListByIndex(resultStruct.toBeAdded, mainList);
        //     CollectionAssert.AreEquivalent(expectedToBeAddedResult, args.Result.ToBeAdded);

        // }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifierClick(SelectionStruct selectionStruct, ModifiersStruct modifiersStruct, ResultStruct resultStruct)
        {
            List<ISelectable> mainList = TestUtils.GetSomeObjects(selectionStruct.mainListAmount);

            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.CLICK, modifiersStruct.isPreSelection, TestUtils.GetListByIndex(selectionStruct.oldSelection, mainList), TestUtils.GetListByIndex(selectionStruct.newSelection, mainList), mainList);
            SelectionModifierArguments modifierArguments = new SelectionModifierArguments(modifiersStruct.isSameType, modifiersStruct.isAdditive, Vector2.zero, new Vector2(800, 600));
            SelectionArgsXP args = new SelectionArgsXP(arguments, modifierArguments);

            args = Modifier.Apply(args);

            List<ISelectable> expectedToBeAddedResult = TestUtils.GetListByIndex(resultStruct.toBeAdded, mainList);
            CollectionAssert.AreEquivalent(expectedToBeAddedResult, args.Result.ToBeAdded);

        }

        // [TestCaseSource(nameof(Scenarios))]
        // public void ShouldApplyModifierKey(SelectionStruct selectionStruct, ModifiersStruct modifiersStruct, ResultStruct resultStruct)
        // {
        //     List<ISelectable> mainList = TestUtils.GetSomeObjects(selectionStruct.mainListAmount);

        //     SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.KEY, modifiersStruct.isPreSelection, TestUtils.GetListByIndex(selectionStruct.oldSelection, mainList), TestUtils.GetListByIndex(selectionStruct.newSelection, mainList), mainList);
        //     SelectionModifierArguments modifierArguments = new SelectionModifierArguments(modifiersStruct.isSameType, modifiersStruct.isAdditive, Vector2.zero, new Vector2(800, 600));
        //     SelectionArgsXP args = new SelectionArgsXP(arguments, modifierArguments);

        //     args = Modifier.Apply(args);

        //     List<ISelectable> expectedToBeAddedResult = TestUtils.GetListByIndex(resultStruct.toBeAdded, mainList);
        //     CollectionAssert.AreEquivalent(expectedToBeAddedResult, args.Result.ToBeAdded);
        // }

        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetCasesWithModifiers(new ModifiersStruct(false, true, false)))
                {
                    int[] toBeAdded = item.selection.newSelection;
                    if (item.modifiers.isAdditive)
                    {
                        bool containsAll = toBeAdded.Length > 0 && toBeAdded.ToList().TrueForAll(x => item.selection.oldSelection.ToList().Contains(x));
                        bool differentCounts = item.selection.oldSelection.Length != toBeAdded.Length;

                        var newToBeAdded = item.selection.oldSelection.Union(toBeAdded).ToList();

                        if (containsAll && differentCounts)
                        {
                            newToBeAdded.RemoveAll(x => toBeAdded.Contains(x));
                        }

                        toBeAdded = newToBeAdded.ToArray();
                    }
                    yield return new TestCaseData(item.selection, item.modifiers, new ResultStruct()
                    {
                        toBeAdded = toBeAdded,
                    }).SetName(item.name);
                }
            }
        }

        public AdditiveSelectionModifier Modifier { get => modifier; set => modifier = value; }
    }
}
