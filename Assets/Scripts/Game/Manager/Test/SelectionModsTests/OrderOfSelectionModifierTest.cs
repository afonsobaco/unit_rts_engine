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
            var result = modifier.Apply(args);
            Assert.AreEqual(args, result);
        }


        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifier(SelectionStruct selectionStruct, ModifiersStruct modifiersStruct, ResultStruct resultStruct)
        {
            HashSet<ISelectableObjectBehaviour> mainList = TestUtils.GetSomeObjects<ISelectableObjectBehaviour>(selectionStruct.mainListAmount);
            HashSet<ISelectableObjectBehaviour> oldSelection = TestUtils.GetListByIndex(selectionStruct.oldSelection, mainList);
            HashSet<ISelectableObjectBehaviour> newSelection = TestUtils.GetListByIndex(selectionStruct.newSelection, mainList);
            HashSet<ISelectableObjectBehaviour> expected = TestUtils.GetListByIndex(resultStruct.expected, mainList);

            SelectionArgsXP args = new SelectionArgsXP(oldSelection, newSelection, mainList);

            mainList.ToList().ForEach(x =>
            {
                if (x is ISelectableObjectBehaviour)
                {
                    var b = x as ISelectableObjectBehaviour;
                    if (b.Index < 4) b.Type = ObjectTypeEnum.UNIT;
                    else if (b.Index < 7) b.Type = ObjectTypeEnum.BUILDING;
                    else if (b.Index < 9) b.Type = ObjectTypeEnum.CONSUMABLE;
                    else b.Type = ObjectTypeEnum.ENVIRONMENT;
                }
            });

            args = modifier.Apply(args);
            Debug.Log("expected");
            expected.ToList().ForEach(x => Debug.Log(x.Index));
            Debug.Log("ToBeAdded");
            args.ToBeAdded.ToList().ForEach(x => Debug.Log(x.Index));

            CollectionAssert.AreEquivalent(expected, args.ToBeAdded);
        }

        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetCustomCases(new ModifiersStruct(true, true), true))
                {
                    List<int> expected = new List<int>();
                    List<int> aux = new List<int>();
                    foreach (var a in item.selection.newSelection)
                    {
                        if (a < 4)
                        {
                            aux.Add(a);
                        }
                    }
                    if (aux.Count == 0)
                    {
                        foreach (var a in item.selection.newSelection)
                        {
                            if (a < 7)
                            {
                                aux.Add(a);
                            }
                        }
                        if (aux.Count > 0)
                        {
                            expected.Add(aux[0]);
                        }
                    }
                    else
                    {
                        expected = aux;
                    }
                    yield return new TestCaseData(item.selection, item.modifiers, new ResultStruct { expected = expected.ToArray() }).SetName(item.name);
                }
                yield return new TestCaseData(new SelectionStruct(10, new int[] { }, new int[] { 4, 5, 6 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 4 })).SetName("NO MODIFIER, EMPTY OLD, MULTIPLE NEW, All on secondary list, first choice, get by secondary order");
                yield return new TestCaseData(new SelectionStruct(10, new int[] { }, new int[] { 7, 9, 4, 5, 6 }, default), new ModifiersStruct(), new ResultStruct(new int[] { 4 })).SetName("NO MODIFIER, EMPTY OLD, MULTIPLE NEW, All on secondary list, shuffled, first choice, get by secondary order");


            }
        }

    }
}
