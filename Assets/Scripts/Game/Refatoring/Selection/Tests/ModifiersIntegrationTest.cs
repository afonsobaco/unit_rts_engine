// using System.Linq;
// using System.Collections.Generic;
// using NUnit.Framework;
// using UnityEngine;
// using RTSEngine.Manager;
// using Tests.Utils;
// using NSubstitute;

// namespace Tests
// {
//     [TestFixture]
//     public class ModifiersIntegrationTest
//     {
//         HashSet<ISelectableObject> mainList = TestUtils.GetSomeObjects(10);
//         private ISelectionManager selectionManager;
//         private ISelectionSettings settings;

//         [SetUp]
//         public void SetUp()
//         {
//             mainList = TestUtils.GetSomeObjects(10);
//             selectionManager = Substitute.For<ISelectionManager>();
//             settings = Substitute.For<ISelectionSettings>();
//             settings.Restricted.Returns(new ObjectTypeEnum[] { ObjectTypeEnum.UNIT, ObjectTypeEnum.BUILDING });
//             settings.Limit.Returns(10);
//             settings.Primary.Returns(new ObjectTypeEnum[] { ObjectTypeEnum.UNIT });
//             settings.Secondary.Returns(new ObjectTypeEnum[] { ObjectTypeEnum.BUILDING });
//             selectionManager.GetSettings().Returns(settings);

//             mainList.ToList().ForEach(x =>
//             {
//                 if (x.Index < 4)
//                 {
//                     x.SelectableObjectInfo.Type = ObjectTypeEnum.UNIT;
//                 }
//                 else if (x.Index < 7)
//                 {
//                     x.SelectableObjectInfo.Type = ObjectTypeEnum.BUILDING;
//                 }
//                 else
//                 {
//                     x.SelectableObjectInfo.Type = ObjectTypeEnum.CONSUMABLE;
//                 }
//                 x.LifeStatus = new ObjectStatus();
//             });
//         }

//         [TestCaseSource(nameof(Scenarios))]
//         public void ShouldRunAllIntegrationCases(SelectionTypeEnum type, bool isAdditive, bool isSubGroup, int[] oldSelectionIndexes, int[] newSelectionIndexes, int[] expectedResultIndexes)
//         {
//             HashSet<ISelectableObject> oldSelection = TestUtils.GetListByIndex(oldSelectionIndexes, mainList);
//             HashSet<ISelectableObject> newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);
//             HashSet<ISelectableObject> expected = TestUtils.GetListByIndex(expectedResultIndexes, mainList);
//             selectionManager.IsAdditive().Returns(isAdditive);
//             selectionManager.IsSubGroup().Returns(isSubGroup);

//             SelectionArguments args = new SelectionArguments(oldSelection, newSelection, mainList);

//             foreach (var item in GetModifiersBySelectionType(type, new HashSet<int[]> { new int[] { 0, 1 }, new int[] { 2, 3 }, new int[] { 4, 5, 6 } }))
//             {
//                 args = item.Apply(args);
//             }
//             CollectionAssert.AreEquivalent(expected, args.ToBeAdded);
//         }

//         private List<ISelectionModifier> GetModifiersBySelectionType(SelectionTypeEnum type, HashSet<int[]> sameType)
//         {
//             List<ISelectionModifier> modifiers = new List<ISelectionModifier>();
//             modifiers.Add(GetDefaultSubGroupMod(sameType));
//             modifiers.Add(GetDefaultTypePriorityInSelectionModifier());
//             modifiers.Add(GetDefaultAdditiveMod());
//             modifiers.Add(GetDefaultCanBeGroupedMod());
//             modifiers.Add(GetDefaultLimitMod());
//             modifiers.Add(GetDefaultGroupMod());
//             return modifiers.FindAll(x => x.Type.Equals(type) || x.Type.Equals(SelectionTypeEnum.ANY));
//         }

//         private ISelectionModifier GetDefaultCanBeGroupedMod()
//         {
//             CanBeGroupedSelectionModifier modifier = Substitute.ForPartsOf<CanBeGroupedSelectionModifier>(new object[] { selectionManager });
//             return modifier;
//         }

//         private ISelectionModifier GetDefaultAdditiveMod()
//         {
//             AdditiveSelectionModifier modifier = Substitute.ForPartsOf<AdditiveSelectionModifier>(new object[] { selectionManager });
//             return modifier;
//         }

//         private ISelectionModifier GetDefaultSubGroupMod(HashSet<int[]> sameType)
//         {
//             SubGroupSelectionModifier modifier = Substitute.ForPartsOf<SubGroupSelectionModifier>(new object[] { selectionManager });

//             modifier.When(x => x.GetAllFromSubGroupThatCanGroup(Arg.Any<SelectionArguments>())).DoNotCallBase();
//             modifier.GetAllFromSubGroupThatCanGroup(Arg.Any<SelectionArguments>()).Returns(args =>
//                 {
//                     var index = (args[0] as SelectionArguments).NewSelection.First().Index;
//                     foreach (var item in sameType)
//                     {
//                         if (item.Contains(index))
//                         {
//                             return new HashSet<ISelectableObject>(mainList.ToList().FindAll(x => item.Contains(x.Index)));
//                         }
//                     }
//                     return new HashSet<ISelectableObject>();
//                 }
//             );
//             return modifier;
//         }


//         private ISelectionModifier GetDefaultTypePriorityInSelectionModifier()
//         {
//             TypePriorityInSelectionModifier modifier = Substitute.ForPartsOf<TypePriorityInSelectionModifier>(new object[] { selectionManager });
//             return modifier;
//         }

//         private ISelectionModifier GetDefaultLimitMod()
//         {
//             LimitSelectionModifier modifier = Substitute.ForPartsOf<LimitSelectionModifier>(new object[] { selectionManager });
//             return modifier;
//         }

//         private ISelectionModifier GetDefaultGroupMod()
//         {
//             GroupSelectionModifier modifier = Substitute.ForPartsOf<GroupSelectionModifier>(new object[] { selectionManager });
//             return modifier;
//         }

//         private static IEnumerable<TestCaseData> Scenarios
//         {
//             get
//             {
//                 foreach (var item in Cases)
//                 {
//                     var name = TestUtils.GetCaseName(item.selection, item.modifiers);
//                     SelectionTypeEnum type = item.selection.additionalInfo.type;
//                     yield return new TestCaseData(
//                         type, item.modifiers.isAdditive, item.modifiers.isSubGroup,
//                         item.selection.oldSelection, item.selection.newSelection, item.result).SetName(type + " | " + name);
//                 }
//             }
//         }

//         private static HashSet<CaseStruct> Cases
//         {
//             get
//             {
//                 return new HashSet<CaseStruct>()
//                 {
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, false), new int[] { 4 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0, 4 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, false), new int[] { 0, 4 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0, 4 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, false), new int[] { 4 }, ""),

//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, true), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, true), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, true), new int[] { 0, 1 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, true), new int[] { 0, 1 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, true), new int[] { 4, 5, 6 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0, 4 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( false, true), new int[] { 0, 1 }, ""),

//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, true), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, true), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, true), new int[] { 0, 1 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, true), new int[] { 0, 1 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, true), new int[] { 0, 4, 5, 6 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0, 4 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.CLICK, default, default)),  new ModifiersStruct( true, true), new int[] { 4 }, ""),

//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] {  }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 4 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 4 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0, 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0, 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 4 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( false, false), new int[] { 4 }, ""),

//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 4 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0, 4 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0, 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0, 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 4 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4, 7 }, new AdditionalInfo(SelectionTypeEnum.DRAG, default, default)),  new ModifiersStruct( true, false), new int[] { 0, 4 }, ""),

//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 4 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 4 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 7 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 7 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 7 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 7 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { 0, 4, 7 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 0, 4}, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0, 4, 7 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( false, false), new int[] { 0, 4}, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 0 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 4 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 0, 4 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0 }, new int[] { 0, 4 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 0, 4 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0, 4 }, new int[] { 0 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 4 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0, 4 }, new int[] { 0, 4 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 0, 4 }, ""),
//                     new CaseStruct(new SelectionStruct(10, new int[] { 0, 4 }, new int[] { 0, 4, 7 }, new AdditionalInfo(SelectionTypeEnum.KEY, default, default)),  new ModifiersStruct( true, false), new int[] { 0, 4}, ""),
//                 };

//             }
//         }
//     }
// }
