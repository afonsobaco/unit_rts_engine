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
    public class OrderOfSelectionModifierTest
    {
        private OrderOfSelectionModifier modifier;

        [SetUp]
        public void SetUp()
        {
            modifier = Substitute.ForPartsOf<OrderOfSelectionModifier>();
        }

        [Test]
        public void OrderOfSelectionTestSimplePasses()
        {
            SelectionArgsXP args = new SelectionArgsXP(new HashSet<ISelectableObjectBehaviour>(), new HashSet<ISelectableObjectBehaviour>(), new HashSet<ISelectableObjectBehaviour>());
            var result = modifier.Apply(args, SameTypeSelectionModeEnum.DISTANCE);
            Assert.AreEqual(args, result);
        }


        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifierOnClick(SelectionStruct selectionStruct, ModifiersStruct modifiersStruct, ResultStruct resultStruct)
        {
            HashSet<ISelectableObjectBehaviour> mainList = TestUtils.GetSomeObjects<ISelectableObjectBehaviour>(selectionStruct.mainListAmount);
            HashSet<ISelectableObjectBehaviour> oldSelection = TestUtils.GetListByIndex(selectionStruct.oldSelection, mainList);
            HashSet<ISelectableObjectBehaviour> newSelection = TestUtils.GetListByIndex(selectionStruct.newSelection, mainList);
            HashSet<ISelectableObjectBehaviour> expected = TestUtils.GetListByIndex(resultStruct.toBeAdded, mainList);

            SelectionArgsXP args = new SelectionArgsXP(oldSelection, newSelection, mainList);

            mainList.ToList().ForEach(x =>
            {
                if (x is ISelectableObjectBehaviour)
                {
                    var b = x as ISelectableObjectBehaviour;
                    if (b.Index % 2 == 0)
                    {
                        b.Type = ObjectTypeEnum.UNIT;
                    }
                    else
                    {
                        if (b.Index < 4) b.Type = ObjectTypeEnum.BUILDING;
                        else if (b.Index < 8) b.Type = ObjectTypeEnum.CONSUMABLE;
                        else b.Type = ObjectTypeEnum.ENVIRONMENT;
                    }
                }
            });

            args = modifier.Apply(args, new HashSet<ObjectTypeEnum>() { ObjectTypeEnum.UNIT }, new HashSet<ObjectTypeEnum>() { ObjectTypeEnum.BUILDING, ObjectTypeEnum.CONSUMABLE, ObjectTypeEnum.ENVIRONMENT });

            CollectionAssert.AreEquivalent(expected, args.ToBeAdded);
        }

        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetCustomCases(new ModifiersStruct(true, true), true))
                {
                    int[] toBeAdded = new int[] { };
                    if (item.selection.newSelection.Length > 0)
                    {
                        int[] intersection = item.selection.additionalInfo.group_evens.Intersect(item.selection.newSelection).ToArray();
                        if (intersection.Length > 0)
                        {
                            toBeAdded = intersection;
                        }
                        else
                        {
                            intersection = item.selection.additionalInfo.group_odds.Intersect(item.selection.newSelection).ToArray();
                            if (intersection.Length > 0)
                            {
                                toBeAdded = new int[] { intersection[0] };
                            }
                        }
                    }
                    yield return new TestCaseData(item.selection, item.modifiers, new ResultStruct { toBeAdded = toBeAdded }).SetName(item.name);
                }
                yield return new TestCaseData(new SelectionStruct(10, new int[] { }, new int[] { 1, 3, 5 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 1 })).SetName("NO MODIFIER, EMPTY OLD, MULTIPLE NEW, All on secondary list, first choice, get by secondary order");
                yield return new TestCaseData(new SelectionStruct(10, new int[] { }, new int[] { 7, 9, 1, 3, 5 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 1 })).SetName("NO MODIFIER, EMPTY OLD, MULTIPLE NEW, All on secondary list, shuffled, first choice, get by secondary order");
                yield return new TestCaseData(new SelectionStruct(10, new int[] { }, new int[] { 5, 7, 9 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 5 })).SetName("NO MODIFIER, EMPTY OLD, MULTIPLE NEW, All on secondary list, not first choice, get by secondary order");
                yield return new TestCaseData(new SelectionStruct(10, new int[] { }, new int[] { 9, 5, 7 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 5 })).SetName("NO MODIFIER, EMPTY OLD, MULTIPLE NEW, All on secondary list, not first choice, shuffled, get by secondary order");

                // yield return new TestCaseData(new SelectionStruct(10, new int[] { }, new int[] { 1, 3, 5 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 1 })).SetName("ADDITIVE, EMPTY OLD, MULTIPLE NEW, All on secondary list, first choice, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { }, new int[] { 7, 9, 1, 3, 5 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 1 })).SetName("ADDITIVE, EMPTY OLD, MULTIPLE NEW, All on secondary list, shuffled, first choice, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { }, new int[] { 5, 7, 9 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 5 })).SetName("ADDITIVE, EMPTY OLD, MULTIPLE NEW, All on secondary list, not first choice, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { }, new int[] { 9, 5, 7 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 5 })).SetName("ADDITIVE, EMPTY OLD, MULTIPLE NEW, All on secondary list, not first choice, shuffled, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { 0 }, new int[] { 1, 3, 5 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 1 })).SetName("ADDITIVE, SINGLE OLD, OLD IN PRIMARY LIST, MULTIPLE NEW, All on secondary list, first choice, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { 0 }, new int[] { 7, 9, 1, 3, 5 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 1 })).SetName("ADDITIVE, SINGLE OLD, OLD IN PRIMARY LIST, MULTIPLE NEW, All on secondary list, shuffled, first choice, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { 0 }, new int[] { 5, 7, 9 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 5 })).SetName("ADDITIVE, SINGLE OLD, OLD IN PRIMARY LIST, MULTIPLE NEW, All on secondary list, not first choice, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { 0 }, new int[] { 9, 5, 7 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 5 })).SetName("ADDITIVE, SINGLE OLD, OLD IN PRIMARY LIST, MULTIPLE NEW, All on secondary list, not first choice, shuffled, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { 1 }, new int[] { 1, 3, 5 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 1 })).SetName("ADDITIVE, SINGLE OLD, OLD IN SECONDARY LIST, MULTIPLE NEW, All on secondary list, first choice, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { 1 }, new int[] { 7, 9, 1, 3, 5 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 1 })).SetName("ADDITIVE, SINGLE OLD, OLD IN SECONDARY LIST, MULTIPLE NEW, All on secondary list, shuffled, first choice, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { 1 }, new int[] { 5, 7, 9 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 5 })).SetName("ADDITIVE, SINGLE OLD, OLD IN SECONDARY LIST, MULTIPLE NEW, All on secondary list, not first choice, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { 1 }, new int[] { 9, 5, 7 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 5 })).SetName("ADDITIVE, SINGLE OLD, OLD IN SECONDARY LIST, MULTIPLE NEW, All on secondary list, not first choice, shuffled, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { 0,1}, new int[] { 1, 3, 5 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 1 })).SetName("ADDITIVE, MULTIPLE OLD, MULTIPLE NEW, All on secondary list, first choice, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { 0,1}, new int[] { 7, 9, 1, 3, 5 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 1 })).SetName("ADDITIVE, MULTIPLE OLD, MULTIPLE NEW, All on secondary list, shuffled, first choice, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { 0,1}, new int[] { 5, 7, 9 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 5 })).SetName("ADDITIVE, MULTIPLE OLD, MULTIPLE NEW, All on secondary list, not first choice, get by secondary order");
                // yield return new TestCaseData(new SelectionStruct(10, new int[] { 0,1}, new int[] { 9, 5, 7 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 5 })).SetName("ADDITIVE, MULTIPLE OLD, MULTIPLE NEW, All on secondary list, not first choice, shuffled, get by secondary order");

            }
        }

    }
}
