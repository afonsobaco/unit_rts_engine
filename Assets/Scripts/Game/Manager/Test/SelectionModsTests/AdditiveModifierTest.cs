using System.Linq;
using UnityEngine;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Manager;
using System.Collections.Generic;
using Tests.Utils;
using NSubstitute;

namespace Tests
{
    [TestFixture]
    public class AdditiveModifierTest
    {
        private AdditiveSelectionModifier modifier;
        private ISelectionManager selectionManager;

        [SetUp]
        public void SetUp()
        {
            selectionManager = Substitute.For<ISelectionManager>();
            modifier = Substitute.ForPartsOf<AdditiveSelectionModifier>(new object[] { selectionManager });

        }

        [Test]
        public void AdditiveModifierTestSimplePasses()
        {
            SelectionArguments args = new SelectionArguments(new HashSet<ISelectableObject>(), new HashSet<ISelectableObject>(), new HashSet<ISelectableObject>());
            var result = modifier.Apply(args);
            Assert.AreEqual(args, result);
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifierClick(SelectionStruct selectionStruct, ModifiersStruct modifiersStruct, ResultStruct resultStruct)
        {
            selectionManager.IsAdditive().Returns(modifiersStruct.isAdditive);

            HashSet<ISelectableObject> mainList = TestUtils.GetSomeObjects(selectionStruct.mainListAmount);
            HashSet<ISelectableObject> oldSelection = TestUtils.GetListByIndex(selectionStruct.oldSelection, mainList);
            HashSet<ISelectableObject> newSelection = TestUtils.GetListByIndex(selectionStruct.newSelection, mainList);

            SelectionArguments args = new SelectionArguments(oldSelection, newSelection, mainList);

            args = modifier.Apply(args);

            HashSet<ISelectableObject> expectedToBeAddedResult = TestUtils.GetListByIndex(resultStruct.expected, mainList);
            CollectionAssert.AreEquivalent(expectedToBeAddedResult, args.ToBeAdded);

        }

        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetCasesWithModifiers(new ModifiersStruct(true, false)))
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
                        expected = toBeAdded,
                    }).SetName(item.name);
                }
            }
        }

    }
}
