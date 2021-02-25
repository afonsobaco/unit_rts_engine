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
        private ISelectionManager selectionManager;
        private ISelectionSettings settings;

        [SetUp]
        public void SetUp()
        {
            selectionManager = Substitute.For<ISelectionManager>();
            settings = Substitute.For<ISelectionSettings>();
            settings.Primary.Returns(new ObjectTypeEnum[] { ObjectTypeEnum.UNIT });
            settings.Secondary.Returns(new ObjectTypeEnum[] { ObjectTypeEnum.BUILDING });
            selectionManager.GetSettings().Returns(settings);
            modifier = Substitute.ForPartsOf<OrderOfSelectionModifier>(new object[] { selectionManager });
        }

        [Test]
        public void OrderOfSelectionTestSimplePasses()
        {
            SelectionArguments args = new SelectionArguments(new HashSet<ISelectableObject>(), new HashSet<ISelectableObject>(), new HashSet<ISelectableObject>());
            var result = modifier.Apply(args);
            Assert.AreEqual(args, result);
        }


        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifier(SelectionStruct selectionStruct, ModifiersStruct modifiersStruct, ResultStruct resultStruct)
        {
            HashSet<ISelectableObject> mainList = TestUtils.GetSomeObjects(selectionStruct.mainListAmount);
            HashSet<ISelectableObject> oldSelection = TestUtils.GetListByIndex(selectionStruct.oldSelection, mainList);
            HashSet<ISelectableObject> newSelection = TestUtils.GetListByIndex(selectionStruct.newSelection, mainList);
            HashSet<ISelectableObject> expected = TestUtils.GetListByIndex(resultStruct.expected, mainList);

            SelectionArguments args = new SelectionArguments(oldSelection, newSelection, mainList);

            mainList.ToList().ForEach(x =>
            {
                if (x is ISelectableObject)
                {
                    var b = x as ISelectableObject;
                    if (b.Index < 4) b.SelectableObjectInfo.Type = ObjectTypeEnum.UNIT;
                    else if (b.Index < 7) b.SelectableObjectInfo.Type = ObjectTypeEnum.BUILDING;
                    else if (b.Index < 9) b.SelectableObjectInfo.Type = ObjectTypeEnum.CONSUMABLE;
                    else b.SelectableObjectInfo.Type = ObjectTypeEnum.ENVIRONMENT;
                }
            });

            args = modifier.Apply(args);
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
