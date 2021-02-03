using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using RTSEngine.Manager;
using Tests.Utils;
using NSubstitute;

namespace Tests
{

    [TestFixture]
    public class ModifiersIntegrationTest
    {

        HashSet<ISelectableObjectBehaviour> mainList = TestUtils.GetSomeObjects<ISelectableObjectBehaviour>(10);
        private ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> selectionManager;

        [SetUp]
        public void SetUp()
        {
            mainList = TestUtils.GetSomeObjects<ISelectableObjectBehaviour>(10);
            selectionManager = Substitute.For<ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum>>();

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
                    x.Type = ObjectTypeEnum.CONSUMABLE;
                }
            });
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldRunAllIntegrationCases(SelectionTypeEnum type, bool isAdditive, bool isSameType, int[] oldSelectionIndexes, int[] newSelectionIndexes, int[] expectedResultIndexes)
        {
            HashSet<ISelectableObjectBehaviour> oldSelection = TestUtils.GetListByIndex(oldSelectionIndexes, mainList);
            HashSet<ISelectableObjectBehaviour> newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);
            HashSet<ISelectableObjectBehaviour> expected = TestUtils.GetListByIndex(expectedResultIndexes, mainList);
            selectionManager.IsAdditive().Returns(isAdditive);
            selectionManager.IsSameType().Returns(isSameType);

            SelectionArgsXP args = new SelectionArgsXP(oldSelection, newSelection, mainList);

            foreach (var item in GetModifiersBySelectionType(type, new HashSet<int[]> { new int[] { 0, 1 }, new int[] { 2, 3 }, new int[] { 4, 5, 6 } }))
            {
                args = item.Apply(args);
            }
            CollectionAssert.AreEquivalent(expected, args.ToBeAdded);
        }

        private List<ISelectionModifier> GetModifiersBySelectionType(SelectionTypeEnum type, HashSet<int[]> sameType)
        {
            List<ISelectionModifier> modifiers = new List<ISelectionModifier>();
            modifiers.Add(GetDefaultSameTypeMod(sameType));
            modifiers.Add(GetDefaultOrderOfSelectionModifier());
            modifiers.Add(GetDefaultAdditiveMod());
            modifiers.Add(GetDefaultLimitMod());
            return modifiers.FindAll(x => x.Type.Equals(type) || x.Type.Equals(SelectionTypeEnum.ANY));
        }

        private ISelectionModifier GetDefaultAdditiveMod()
        {
            AdditiveSelectionModifier modifier = Substitute.ForPartsOf<AdditiveSelectionModifier>(new object[] { selectionManager });
            return modifier;
        }

        private ISelectionModifier GetDefaultSameTypeMod(HashSet<int[]> sameType)
        {
            SameTypeSelectionModifier modifier = Substitute.ForPartsOf<SameTypeSelectionModifier>(new object[] { selectionManager });

            modifier.When(x => x.GetAllFromSameTypeThatCanGroup(Arg.Any<SelectionArgsXP>())).DoNotCallBase();
            modifier.GetAllFromSameTypeThatCanGroup(Arg.Any<SelectionArgsXP>()).Returns(args =>
                {
                    var index = (args[0] as SelectionArgsXP).NewSelection.First().Index;
                    foreach (var item in sameType)
                    {
                        if (item.Contains(index))
                        {
                            return new HashSet<ISelectableObjectBehaviour>(mainList.ToList().FindAll(x => item.Contains(x.Index)));
                        }
                    }
                    return new HashSet<ISelectableObjectBehaviour>();
                }
            );
            return modifier;
        }


        private ISelectionModifier GetDefaultOrderOfSelectionModifier()
        {
            OrderOfSelectionModifier modifier = Substitute.ForPartsOf<OrderOfSelectionModifier>();
            return modifier;
        }

        private ISelectionModifier GetDefaultLimitMod()
        {
            LimitSelectionModifier modifier = Substitute.ForPartsOf<LimitSelectionModifier>();
            return modifier;
        }

        private static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in Cases)
                {
                    var name = TestUtils.GetCaseName(item.selection, item.modifiers);
                    SelectionTypeEnum type = item.selection.additionalInfo.type;
                    yield return new TestCaseData(
                        type, item.modifiers.isAdditive, item.modifiers.isSameType,
                        item.selection.oldSelection, item.selection.newSelection, item.result).SetName(type + " | " + name);
                }
            }
        }

        private static HashSet<CaseStruct> Cases
        {
            get
            {
                return new HashSet<CaseStruct>()
                {
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, false), new int[] { 4 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0, 4 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, false), new int[] { 0, 4 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0, 4 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, false), new int[] { 4 }, ""),

                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, true), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, true), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, true), new int[] { 0, 1 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, true), new int[] { 0, 1 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, true), new int[] { 4, 5, 6 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0, 4 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, true), new int[] { 0, 1 }, ""),

                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, true), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, true), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, true), new int[] { 0, 1 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, true), new int[] { 0, 1 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, true), new int[] { 0, 4, 5, 6 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0, 4 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, true), new int[] { 4 }, ""),

                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] {  }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 4 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 4 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0, 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0, 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 4 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 4 }, ""),

                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 4 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0, 4 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0, 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0, 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 4 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0, 4 }, ""),

                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 4 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 4 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 7 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 7 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 7 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 7 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0, 4, 7 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 0, 4, 7 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0, 4, 7 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 0, 4, 7 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 0, 4 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0, 4 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 0, 4 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0, 4 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 4 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0, 4 }, new int[] { 0, 4 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 0, 4 }, ""),
                    new CaseStruct(new SelectionStruct(10, new int[] { 0, 4 }, new int[] { 0, 4, 7 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 0, 4, 7 }, ""),
                };

            }
        }
    }
}
