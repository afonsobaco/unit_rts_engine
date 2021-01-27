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
    public class AdditiveModifierTest
    {
        private AdditiveModifier.SelectionModifier modifier;

        [SetUp]
        public void SetUp()
        {
            Modifier = new AdditiveModifier.SelectionModifier();
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
            SelectionArgsXP args = new SelectionArgsXP(new SelectionArguments(SelectionTypeEnum.ANY, false, new List<ISelectable>(), new List<ISelectable>(), new List<ISelectable>()), new SelectionModifierArguments(default, true, default, default));

            args = Modifier.Apply(args);

            List<ISelectable> expectedToBeAddedResult = new List<ISelectable>();
            CollectionAssert.AreEquivalent(expectedToBeAddedResult, args.Result.ToBeAdded);

            List<ISelectable> expectedToBeRemovedResult = new List<ISelectable>();
            CollectionAssert.AreEquivalent(expectedToBeRemovedResult, args.Result.ToBeRemoved);
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

        //     List<ISelectable> expectedToBeRemovedResult = TestUtils.GetListByIndex(resultStruct.toBeRemoved, mainList);
        //     CollectionAssert.AreEquivalent(expectedToBeRemovedResult, args.Result.ToBeRemoved);
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

            List<ISelectable> expectedToBeRemovedResult = TestUtils.GetListByIndex(resultStruct.toBeRemoved, mainList);
            CollectionAssert.AreEquivalent(expectedToBeRemovedResult, args.Result.ToBeRemoved);
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

        //     List<ISelectable> expectedToBeRemovedResult = TestUtils.GetListByIndex(resultStruct.toBeRemoved, mainList);
        //     CollectionAssert.AreEquivalent(expectedToBeRemovedResult, args.Result.ToBeRemoved);
        // }

        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetCases(new ModifiersStruct(false, true, false)))
                {
                    int[] toBeAdded;
                    if (item.modifiers.isAdditive && item.selection.newSelection.Length > 0)
                    {

                        bool containsAll = item.selection.newSelection.ToList().TrueForAll(x => item.selection.oldSelection.ToList().Contains(x));
                        bool differentCounts = item.selection.oldSelection.Length != item.selection.newSelection.Length;
                        if (!(containsAll && differentCounts))
                        {
                            toBeAdded = item.selection.oldSelection.Union(item.selection.newSelection).ToArray();
                        }
                        else
                        {
                            toBeAdded = new int[] { };
                        }
                    }
                    else
                    {
                        toBeAdded = item.selection.newSelection;
                    }
                    yield return new TestCaseData(item.selection, item.modifiers, new ResultStruct()
                    {
                        toBeAdded = toBeAdded,
                        toBeRemoved = item.selection.oldSelection,
                    }).SetName(item.name);
                }
            }
        }

        public AdditiveModifier.SelectionModifier Modifier { get => modifier; set => modifier = value; }
    }
}
