using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using RTSEngine.Manager;
using RTSEngine.Core;
using Tests.Utils;
using System;
using NSubstitute;

namespace Tests.Manager
{

    [TestFixture]
    public class ModifiersIntegrationTest
    {

        List<ISelectable> mainList = TestUtils.GetSomeObjects(10);
        [SetUp]
        public void SetUp()
        {
            mainList = TestUtils.GetSomeObjects(10);
        }

        [TestCaseSource(nameof(Scenarios))]
        public void mainTest(SelectionTypeEnum type, bool isPreSelection, bool isAdditive, bool isSameType, int[] OLDSelectionIndexes, int[] newSelectionIndexes, int[] expectedResultIndexes)
        {
            List<ISelectable> OLDSelection = TestUtils.GetListByIndex(OLDSelectionIndexes, mainList);
            List<ISelectable> newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);
            List<ISelectable> expected = TestUtils.GetListByIndex(expectedResultIndexes, mainList);
            SelectionArgsXP args = new SelectionArgsXP(new SelectionArguments(type, isPreSelection, OLDSelection, newSelection, mainList), new SelectionModifierArguments(isSameType, isAdditive, new Vector2(0, 0), new Vector2(800, 600)));
            foreach (var item in GetModifiersBySelectionType(type))
            {
                args = item.Apply(args);
            }
            CollectionAssert.AreEquivalent(expected, args.Result.ToBeAdded);
        }

        private List<IBaseSelectionMod> GetModifiersBySelectionType(SelectionTypeEnum type)
        {
            List<IBaseSelectionMod> modifiers = new List<IBaseSelectionMod>();
            modifiers.Add(GetDefaultAdditiveMod());
            modifiers.Add(GetDefaultSameTypeMod());
            modifiers.Add(GetDefaultLimitMod());
            return modifiers.FindAll(x => x.Type.Equals(type) || x.Type.Equals(SelectionTypeEnum.ANY));
        }

        private static void DebugLog(SelectionArgsXP args)
        {
            Debug.Log("oldSelection");
            args.Arguments.OldSelection.ForEach(x => Debug.Log(x.Index));
            Debug.Log("NewSelection");
            args.Arguments.NewSelection.ForEach(x => Debug.Log(x.Index));
            Debug.Log("toBeAdded");
            args.Result.ToBeAdded.ForEach(x => Debug.Log(x.Index));
        }

        private IBaseSelectionMod GetDefaultAdditiveMod()
        {
            IBaseSelectionMod baseMode = Substitute.For<IBaseSelectionMod>();
            AdditiveSelectionModifier modifier = Substitute.ForPartsOf<AdditiveSelectionModifier>();
            baseMode.Apply(Arg.Any<SelectionArgsXP>()).Returns(x =>
            {
                return modifier.Apply(x[0] as SelectionArgsXP);
            });
            return baseMode;
        }

        private IBaseSelectionMod GetDefaultSameTypeMod()
        {
            IBaseSelectionMod baseMode = Substitute.For<IBaseSelectionMod>();

            SameTypeSelectionModifier modifier = Substitute.ForPartsOf<SameTypeSelectionModifier>();
            modifier.When(x => x.GetAllFromSameTypeOnScreen(Arg.Any<SelectionArgsXP>(), Arg.Any<SameTypeSelectionModeEnum>())).DoNotCallBase();
            modifier.GetAllFromSameTypeOnScreen(Arg.Any<SelectionArgsXP>(), Arg.Any<SameTypeSelectionModeEnum>()).Returns(args =>
                {
                    if ((args[0] as SelectionArgsXP).Arguments.NewSelection[0].Index % 2 == 0)
                    {
                        return mainList.FindAll(x => x.Index % 2 == 0);
                    }
                    else
                    {
                        return mainList.FindAll(x => x.Index % 2 != 0);
                    }
                }
            );
            baseMode.Type = SelectionTypeEnum.CLICK;
            baseMode.Apply(Arg.Any<SelectionArgsXP>()).Returns(x =>
            {
                return modifier.Apply(x[0] as SelectionArgsXP);
            });
            return baseMode;
        }

        private IBaseSelectionMod GetDefaultLimitMod()
        {
            IBaseSelectionMod baseMode = Substitute.For<IBaseSelectionMod>();
            LimitSelectionModifier modifier = Substitute.ForPartsOf<LimitSelectionModifier>();
            baseMode.Apply(Arg.Any<SelectionArgsXP>()).Returns(x =>
            {
                return modifier.Apply(x[0] as SelectionArgsXP);
            });
            return baseMode;
        }


        private static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                // mainListAmount, selectionType , isPreselection, isAdditive, isSameType, OLDSelection, newSelection, expectedResult

                //CLICK
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, false, new int[] { }, new int[] { }, new int[] { }).SetName("ON CLICK: NO MODIFIERS, EMPTY OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, false, new int[] { }, new int[] { 0 }, new int[] { 0 }).SetName("ON CLICK: NO MODIFIERS, EMPTY OLD, SINGLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, false, new int[] { 0 }, new int[] { }, new int[] { }).SetName("ON CLICK: NO MODIFIERS, SINGLE OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, false, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }).SetName("ON CLICK: NO MODIFIERS, SINGLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, false, new int[] { 0 }, new int[] { 1 }, new int[] { 1 }).SetName("ON CLICK: NO MODIFIERS, SINGLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { }, new int[] { }).SetName("ON CLICK: NO MODIFIERS, MULTIPLE OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }, new int[] { 0 }).SetName("ON CLICK: NO MODIFIERS, MULTIPLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 }).SetName("ON CLICK: NO MODIFIERS, MULTIPLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => NEW");
                //Click + aditive
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, false, new int[] { }, new int[] { }, new int[] { }).SetName("ON CLICK: ADDITIVE, EMPTY OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, false, new int[] { }, new int[] { 0 }, new int[] { 0 }).SetName("ON CLICK: ADDITIVE, EMPTY OLD, SINGLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, false, new int[] { 0 }, new int[] { }, new int[] { 0 }).SetName("ON CLICK: ADDITIVE, SINGLE OLD, EMPTY NEW, RESULT => OLD");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, false, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }).SetName("ON CLICK: ADDITIVE, SINGLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => OLD ");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, false, new int[] { 0 }, new int[] { 1 }, new int[] { 0, 1 }).SetName("ON CLICK: ADDITIVE, SINGLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON CLICK: ADDITIVE, MULTIPLE OLD, EMPTY NEW, RESULT => OLD");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }, new int[] { 1, 2, 3, 4 }).SetName("ON CLICK: ADDITIVE, MULTIPLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => OLD - NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 0, 1, 2, 3, 4, 5 }).SetName("ON CLICK: ADDITIVE, MULTIPLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => OLD + NEW");
                //Click + SameType
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, true, new int[] { }, new int[] { }, new int[] { }).SetName("ON CLICK: SAMETYPE, EMPTY OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, true, new int[] { }, new int[] { 0 }, new int[] { 0, 2, 4, 6, 8 }).SetName("ON CLICK: SAMETYPE, EMPTY OLD, SINGLE NEW, RESULT => NEW + SAMETYPE FROM NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, true, new int[] { 0 }, new int[] { }, new int[] { }).SetName("ON CLICK: SAMETYPE, SINGLE OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, true, new int[] { 0 }, new int[] { 0 }, new int[] { 0, 2, 4, 6, 8 }).SetName("ON CLICK: SAMETYPE, SINGLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW + SAMETYPE FROM NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, true, new int[] { 0 }, new int[] { 1 }, new int[] { 1, 3, 5, 7, 9 }).SetName("ON CLICK: SAMETYPE, SINGLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => NEW + SAMETYPE FROM NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { }, new int[] { }).SetName("ON CLICK: SAMETYPE, MULTIPLE OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }, new int[] { 1, 3 }).SetName("ON CLICK: SAMETYPE, MULTIPLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => OLD - SAMETYPE FROM NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 1, 3, 5, 7, 9 }).SetName("ON CLICK: SAMETYPE, MULTIPLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => NEW + SAMETYPE FROM NEW");
                //Click + additive + SameType
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, true, new int[] { }, new int[] { }, new int[] { }).SetName("ON CLICK: ADDITIVE | SAMETYPE, EMPTY OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, true, new int[] { }, new int[] { 0 }, new int[] { 0, 2, 4, 6, 8 }).SetName("ON CLICK: ADDITIVE | SAMETYPE, EMPTY OLD, SINGLE NEW, RESULT => NEW + SAMETYPE FROM NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, true, new int[] { 0 }, new int[] { }, new int[] { 0 }).SetName("ON CLICK: ADDITIVE | SAMETYPE, SINGLE OLD, EMPTY NEW, RESULT => OLD");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, true, new int[] { 0 }, new int[] { 0 }, new int[] { 0, 2, 4, 6, 8 }).SetName("ON CLICK: ADDITIVE | SAMETYPE, SINGLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW + SAMETYPE FROM NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, true, new int[] { 0 }, new int[] { 1 }, new int[] { 0, 1, 3, 5, 7, 9 }).SetName("ON CLICK: ADDITIVE | SAMETYPE, SINGLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => NEW + SAMETYPE FROM NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON CLICK: ADDITIVE | SAMETYPE, MULTIPLE OLD, EMPTY NEW, RESULT => OLD");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }, new int[] { 1, 3 }).SetName("ON CLICK: ADDITIVE | SAMETYPE, MULTIPLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => OLD - SAMETYPE FROM NEW");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 0, 1, 2, 3, 4, 5, 7, 9 }).SetName("ON CLICK: ADDITIVE | SAMETYPE, MULTIPLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => NEW + SAMETYPE FROM NEW");


                // //DRAG
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { }, new int[] { }, new int[] { }).SetName("ON DRAG: NO MODIFIERS, EMPTY OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { }, new int[] { 0 }, new int[] { 0 }).SetName("ON DRAG: NO MODIFIERS, EMPTY OLD, SINGLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: NO MODIFIERS, EMPTY OLD, MULTIPLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { 0 }, new int[] { }, new int[] { }).SetName("ON DRAG: NO MODIFIERS, SINGLE OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }).SetName("ON DRAG: NO MODIFIERS, SINGLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { 0 }, new int[] { 1 }, new int[] { 1 }).SetName("ON DRAG: NO MODIFIERS, SINGLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { 0 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: NO MODIFIERS, SINGLE OLD, MULTIPLE NEW, NEW CONTAINS OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { 0 }, new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }).SetName("ON DRAG: NO MODIFIERS, SINGLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { }, new int[] { }).SetName("ON DRAG: NO MODIFIERS, MULTIPLE OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }, new int[] { 0 }).SetName("ON DRAG: NO MODIFIERS, MULTIPLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 }).SetName("ON DRAG: NO MODIFIERS, MULTIPLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }).SetName("ON DRAG: NO MODIFIERS, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS SOME OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: NO MODIFIERS, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS ALL OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 5, 6, 7 }, new int[] { 0, 1, 2, 5, 6, 7 }).SetName("ON DRAG: NO MODIFIERS, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS MIXED, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8, 9 }, new int[] { 5, 6, 7, 8, 9 }).SetName("ON DRAG: NO MODIFIERS, MULTIPLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS ANY OLD, RESULT => NEW");
                //DRAG + aditive
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { }, new int[] { }, new int[] { }).SetName("ON DRAG: ADDITIVE, EMPTY OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { }, new int[] { 0 }, new int[] { 0 }).SetName("ON DRAG: ADDITIVE, EMPTY OLD, SINGLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: ADDITIVE, EMPTY OLD, MULTIPLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { 0 }, new int[] { }, new int[] { 0 }).SetName("ON DRAG: ADDITIVE, SINGLE OLD, EMPTY NEW, RESULT => OLD");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }).SetName("ON DRAG: ADDITIVE, SINGLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { 0 }, new int[] { 1 }, new int[] { 0, 1 }).SetName("ON DRAG: ADDITIVE, SINGLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT =>OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { 0 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: ADDITIVE, SINGLE OLD, MULTIPLE NEW, NEW CONTAINS OLD, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { 0 }, new int[] { 1, 2, 3, 4, 5 }, new int[] { 0, 1, 2, 3, 4, 5 }).SetName("ON DRAG: ADDITIVE, SINGLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS OLD, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: ADDITIVE, MULTIPLE OLD, EMPTY NEW, RESULT => OLD");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: ADDITIVE, MULTIPLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 0, 1, 2, 3, 4, 5 }).SetName("ON DRAG: ADDITIVE, MULTIPLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: ADDITIVE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS SOME OLD, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: ADDITIVE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS ALL OLD, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 5, 6, 7 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }).SetName("ON DRAG: ADDITIVE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS MIXED, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8, 9 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }).SetName("ON DRAG: ADDITIVE, MULTIPLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS ANY OLD, RESULT => OLD + NEW");
                //DRAG + SameType
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { }, new int[] { }, new int[] { }).SetName("ON DRAG: SAMETYPE, EMPTY OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { }, new int[] { 0 }, new int[] { 0 }).SetName("ON DRAG: SAMETYPE, EMPTY OLD, SINGLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: SAMETYPE, EMPTY OLD, MULTIPLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { 0 }, new int[] { }, new int[] { }).SetName("ON DRAG: SAMETYPE, SINGLE OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }).SetName("ON DRAG: SAMETYPE, SINGLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { 0 }, new int[] { 1 }, new int[] { 1 }).SetName("ON DRAG: SAMETYPE, SINGLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { 0 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: SAMETYPE, SINGLE OLD, MULTIPLE NEW, NEW CONTAINS OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { 0 }, new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }).SetName("ON DRAG: SAMETYPE, SINGLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { }, new int[] { }).SetName("ON DRAG: SAMETYPE, MULTIPLE OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }, new int[] { 0 }).SetName("ON DRAG: SAMETYPE, MULTIPLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 }).SetName("ON DRAG: SAMETYPE, MULTIPLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }).SetName("ON DRAG: SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS SOME OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS ALL OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 5, 6, 7 }, new int[] { 0, 1, 2, 5, 6, 7 }).SetName("ON DRAG: SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS MIXED, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8, 9 }, new int[] { 5, 6, 7, 8, 9 }).SetName("ON DRAG: SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS ANY OLD, RESULT => NEW");
                //DRAG + Additive + SameType
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { }, new int[] { }, new int[] { }).SetName("ON DRAG: ADDITIVE | SAMETYPE, EMPTY OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { }, new int[] { 0 }, new int[] { 0 }).SetName("ON DRAG: ADDITIVE | SAMETYPE, EMPTY OLD, SINGLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: ADDITIVE | SAMETYPE, EMPTY OLD, MULTIPLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { 0 }, new int[] { }, new int[] { 0 }).SetName("ON DRAG: ADDITIVE | SAMETYPE, SINGLE OLD, EMPTY NEW, RESULT => OLD");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }).SetName("ON DRAG: ADDITIVE | SAMETYPE, SINGLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { 0 }, new int[] { 1 }, new int[] { 0, 1 }).SetName("ON DRAG: ADDITIVE | SAMETYPE, SINGLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT =>OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { 0 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: ADDITIVE | SAMETYPE, SINGLE OLD, MULTIPLE NEW, NEW CONTAINS OLD, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { 0 }, new int[] { 1, 2, 3, 4, 5 }, new int[] { 0, 1, 2, 3, 4, 5 }).SetName("ON DRAG: ADDITIVE | SAMETYPE, SINGLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS OLD, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: ADDITIVE | SAMETYPE, MULTIPLE OLD, EMPTY NEW, RESULT => OLD");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: ADDITIVE | SAMETYPE, MULTIPLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 0, 1, 2, 3, 4, 5 }).SetName("ON DRAG: ADDITIVE | SAMETYPE, MULTIPLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: ADDITIVE | SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS SOME OLD, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON DRAG: ADDITIVE | SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS ALL OLD, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 5, 6, 7 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }).SetName("ON DRAG: ADDITIVE | SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS MIXED, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8, 9 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }).SetName("ON DRAG: ADDITIVE | SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS ANY OLD, RESULT => OLD + NEW");

                // //KEY
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { }, new int[] { }, new int[] { }).SetName("ON KEY: NO MODIFIERS, EMPTY OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { }, new int[] { 0 }, new int[] { 0 }).SetName("ON KEY: NO MODIFIERS, EMPTY OLD, SINGLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON KEY: NO MODIFIERS, EMPTY OLD, MULTIPLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { 0 }, new int[] { }, new int[] { }).SetName("ON KEY: NO MODIFIERS, SINGLE OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }).SetName("ON KEY: NO MODIFIERS, SINGLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { 0 }, new int[] { 1 }, new int[] { 1 }).SetName("ON KEY: NO MODIFIERS, SINGLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { 0 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON KEY: NO MODIFIERS, SINGLE OLD, MULTIPLE NEW, NEW CONTAINS OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { 0 }, new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }).SetName("ON KEY: NO MODIFIERS, SINGLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { }, new int[] { }).SetName("ON KEY: NO MODIFIERS, MULTIPLE OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }, new int[] { 0 }).SetName("ON KEY: NO MODIFIERS, MULTIPLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 }).SetName("ON KEY: NO MODIFIERS, MULTIPLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }).SetName("ON KEY: NO MODIFIERS, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS SOME OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON KEY: NO MODIFIERS, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS ALL OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 5, 6, 7 }, new int[] { 0, 1, 2, 5, 6, 7 }).SetName("ON KEY: NO MODIFIERS, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS MIXED, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8, 9 }, new int[] { 5, 6, 7, 8, 9 }).SetName("ON KEY: NO MODIFIERS, MULTIPLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS ANY OLD, RESULT => NEW");
                //KEY + aditive
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { }, new int[] { }, new int[] { }).SetName("ON KEY: ADDITIVE, EMPTY OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { }, new int[] { 0 }, new int[] { 0 }).SetName("ON KEY: ADDITIVE, EMPTY OLD, SINGLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON KEY: ADDITIVE, EMPTY OLD, MULTIPLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { 0 }, new int[] { }, new int[] { 0 }).SetName("ON KEY: ADDITIVE, SINGLE OLD, EMPTY NEW, RESULT => OLD");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }).SetName("ON KEY: ADDITIVE, SINGLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { 0 }, new int[] { 1 }, new int[] { 0, 1 }).SetName("ON KEY: ADDITIVE, SINGLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT =>OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { 0 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON KEY: ADDITIVE, SINGLE OLD, MULTIPLE NEW, NEW CONTAINS OLD, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { 0 }, new int[] { 1, 2, 3, 4, 5 }, new int[] { 0, 1, 2, 3, 4, 5 }).SetName("ON KEY: ADDITIVE, SINGLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS OLD, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON KEY: ADDITIVE, MULTIPLE OLD, EMPTY NEW, RESULT => OLD");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }, new int[] { 1, 2, 3, 4 }).SetName("ON KEY: ADDITIVE, MULTIPLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => OLD - NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 0, 1, 2, 3, 4, 5 }).SetName("ON KEY: ADDITIVE, MULTIPLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2 }, new int[] { 3, 4 }).SetName("ON KEY: ADDITIVE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS SOME OLD, RESULT => OLD - NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON KEY: ADDITIVE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS ALL OLD, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 5, 6, 7 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }).SetName("ON KEY: ADDITIVE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS MIXED, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, false, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8, 9 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }).SetName("ON KEY: ADDITIVE, MULTIPLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS ANY OLD, RESULT => OLD + NEW");
                //KEY + SameType
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { }, new int[] { }, new int[] { }).SetName("ON KEY: SAMETYPE, EMPTY OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { }, new int[] { 0 }, new int[] { 0 }).SetName("ON KEY: SAMETYPE, EMPTY OLD, SINGLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON KEY: SAMETYPE, EMPTY OLD, MULTIPLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { 0 }, new int[] { }, new int[] { }).SetName("ON KEY: SAMETYPE, SINGLE OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }).SetName("ON KEY: SAMETYPE, SINGLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { 0 }, new int[] { 1 }, new int[] { 1 }).SetName("ON KEY: SAMETYPE, SINGLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { 0 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON KEY: SAMETYPE, SINGLE OLD, MULTIPLE NEW, NEW CONTAINS OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { 0 }, new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }).SetName("ON KEY: SAMETYPE, SINGLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { }, new int[] { }).SetName("ON KEY: SAMETYPE, MULTIPLE OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }, new int[] { 0 }).SetName("ON KEY: SAMETYPE, MULTIPLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 }).SetName("ON KEY: SAMETYPE, MULTIPLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }).SetName("ON KEY: SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS SOME OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON KEY: SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS ALL OLD, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 5, 6, 7 }, new int[] { 0, 1, 2, 5, 6, 7 }).SetName("ON KEY: SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS MIXED, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, false, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8, 9 }, new int[] { 5, 6, 7, 8, 9 }).SetName("ON KEY: SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS ANY OLD, RESULT => NEW");
                //KEY + Additive + SameType
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { }, new int[] { }, new int[] { }).SetName("ON KEY: ADDITIVE | SAMETYPE, EMPTY OLD, EMPTY NEW, RESULT => EMPTY");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { }, new int[] { 0 }, new int[] { 0 }).SetName("ON KEY: ADDITIVE | SAMETYPE, EMPTY OLD, SINGLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON KEY: ADDITIVE | SAMETYPE, EMPTY OLD, MULTIPLE NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { 0 }, new int[] { }, new int[] { 0 }).SetName("ON KEY: ADDITIVE | SAMETYPE, SINGLE OLD, EMPTY NEW, RESULT => OLD");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }).SetName("ON KEY: ADDITIVE | SAMETYPE, SINGLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { 0 }, new int[] { 1 }, new int[] { 0, 1 }).SetName("ON KEY: ADDITIVE | SAMETYPE, SINGLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT =>OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { 0 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON KEY: ADDITIVE | SAMETYPE, SINGLE OLD, MULTIPLE NEW, NEW CONTAINS OLD, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { 0 }, new int[] { 1, 2, 3, 4, 5 }, new int[] { 0, 1, 2, 3, 4, 5 }).SetName("ON KEY: ADDITIVE | SAMETYPE, SINGLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS OLD, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON KEY: ADDITIVE | SAMETYPE, MULTIPLE OLD, EMPTY NEW, RESULT => OLD");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0 }, new int[] { 1, 2, 3, 4 }).SetName("ON KEY: ADDITIVE | SAMETYPE, MULTIPLE OLD, SINGLE NEW, OLD CONTAINS NEW, RESULT => OLD - NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 0, 1, 2, 3, 4, 5 }).SetName("ON KEY: ADDITIVE | SAMETYPE, MULTIPLE OLD, SINGLE NEW, OLD DOES NOT CONTAINS NEW, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2 }, new int[] { 3, 4 }).SetName("ON KEY: ADDITIVE | SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS SOME OLD, RESULT => OLD - NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }).SetName("ON KEY: ADDITIVE | SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS ALL OLD, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 1, 2, 5, 6, 7 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }).SetName("ON KEY: ADDITIVE | SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW CONTAINS MIXED, RESULT => OLD + NEW");
                yield return new TestCaseData(SelectionTypeEnum.KEY, false, true, true, new int[] { 0, 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8, 9 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }).SetName("ON KEY: ADDITIVE | SAMETYPE, MULTIPLE OLD, MULTIPLE NEW, NEW DOES NOT CONTAINS ANY OLD, RESULT => OLD + NEW");

            }
        }
    }
}
