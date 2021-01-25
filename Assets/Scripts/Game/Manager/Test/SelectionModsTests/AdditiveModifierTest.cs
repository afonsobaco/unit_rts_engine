using UnityEngine;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Manager;
using RTSEngine.Manager.SelectionMods.Impls;
using System.Collections.Generic;

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
            SelectionArgsXP args = ModifierTestUtils.GetDefaultArgs();
            var result = Modifier.Apply(args);
            Assert.AreEqual(args, result);
        }


        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifier(SelectionTypeEnum selectionType, int mainListCount, bool additive, int[] oldSelection, int[] newSelection, int[] expectedToBeAdded, int[] expectedToBeRemoved)
        {
            SelectionArgsXP args = ModifierTestUtils.GetDefaultArgs();
            args.SelectionType = selectionType;
            args.IsAdditive = additive;

            List<ISelectable> mainList = ModifierTestUtils.GetSomeObjects(mainListCount);
            args.OldSelection.AddRange(ModifierTestUtils.GetListByIndex(oldSelection, mainList));
            args.NewSelection.AddRange(ModifierTestUtils.GetListByIndex(newSelection, mainList));
            args.ToBeRemoved.AddRange(args.OldSelection);
            args.ToBeAdded.AddRange(args.NewSelection);

            args = Modifier.Apply(args);

            List<ISelectable> expectedToBeAddedResult = ModifierTestUtils.GetListByIndex(expectedToBeAdded, mainList);
            CollectionAssert.AreEqual(expectedToBeAddedResult, args.ToBeAdded);

            List<ISelectable> expectedToBeRemovedResult = ModifierTestUtils.GetListByIndex(expectedToBeRemoved, mainList);
            CollectionAssert.AreEqual(expectedToBeRemovedResult, args.ToBeRemoved);
        }


        public static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                //Click
                yield return new TestCaseData(SelectionTypeEnum.CLICK, 1, true, new int[] { }, new int[] { 0 }, new int[] { 0 }, new int[] { }).SetName("Click - Additive, Empty Old");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, 1, true, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }, new int[] { }).SetName("Click - Additive, Single Element in Old, Clicked is in Old");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, 2, true, new int[] { 0 }, new int[] { 1 }, new int[] { 0, 1 }, new int[] { }).SetName("Click - Additive, Single Element in Old, Clicked is NOT in Old");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, 2, true, new int[] { 0, 1 }, new int[] { 0 }, new int[] { 1 }, new int[] { 0 }).SetName("Click - Additive, Multiple Element in Old, Clicked is in Old");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, 3, true, new int[] { 0, 1 }, new int[] { 2 }, new int[] { 0, 1, 2 }, new int[] { }).SetName("Click - Additive, Multiple Element in Old, Clicked is NOT in Old");

                yield return new TestCaseData(SelectionTypeEnum.CLICK, 1, false, new int[] { }, new int[] { 0 }, new int[] { 0 }, new int[] { }).SetName("Click - NOT Additive, Empty Old");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, 1, false, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }).SetName("Click - NOT Additive, Single Element in Old, Clicked is in Old");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, 2, false, new int[] { 0 }, new int[] { 1 }, new int[] { 1 }, new int[] { 0 }).SetName("Click - NOT Additive, Single Element in Old, Clicked is NOT in Old");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, 2, false, new int[] { 0, 1 }, new int[] { 0 }, new int[] { 0 }, new int[] { 0, 1 }).SetName("Click - NOT Additive, Multiple Element in Old, Clicked is in Old");
                yield return new TestCaseData(SelectionTypeEnum.CLICK, 3, false, new int[] { 0, 1 }, new int[] { 2 }, new int[] { 2 }, new int[] { 0, 1 }).SetName("Click - NOT Additive, Multiple Element in Old, Clicked is NOT in Old");
                //Drag
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 2, true, new int[] { }, new int[] { 0, 1 }, new int[] { 0, 1 }, new int[] { }).SetName("Drag - Additive, Empty Old, -");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 2, true, new int[] { 0 }, new int[] { 1 }, new int[] { 0, 1 }, new int[] { }).SetName("Drag - Additive, Single Element in Old, Single Element in new, Old does NOT contains new Element");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 1, true, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }, new int[] { }).SetName("Drag - Additive, Single Element in Old, Single Element in new, Old contains new Element");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 3, true, new int[] { 0 }, new int[] { 1, 2 }, new int[] { 0, 1, 2 }, new int[] { }).SetName("Drag - Additive, Single Element in Old, Multiple Element in new, Does NOT contains Old");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 2, true, new int[] { 0 }, new int[] { 0, 1 }, new int[] { 0, 1 }, new int[] { }).SetName("Drag - Additive, Single Element in Old, Multiple Element in new, Does Contains Old");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 3, true, new int[] { 0, 1 }, new int[] { 2 }, new int[] { 0, 1, 2 }, new int[] { }).SetName("Drag - Additive, Multiple Element in Old, Single Element in new, Old does NOT contains new Element");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 2, true, new int[] { 0, 1 }, new int[] { 0 }, new int[] { 0, 1 }, new int[] { }).SetName("Drag - Additive, Multiple Element in Old, Single Element in new, Old contains new Element");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 4, true, new int[] { 0, 1 }, new int[] { 2, 3 }, new int[] { 0, 1, 2, 3 }, new int[] { }).SetName("Drag - Additive, Multiple Element in Old, Multiple Element in new, New does NOT contains any from Old");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 3, true, new int[] { 0, 1 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { }).SetName("Drag - Additive, Multiple Element in Old, Multiple Element in new, New Contains All from Old");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 3, true, new int[] { 0, 1 }, new int[] { 0, 2 }, new int[] { 0, 1, 2 }, new int[] { }).SetName("Drag - Additive, Multiple Element in Old, Multiple Element in new, New does Contains some from Old");

                yield return new TestCaseData(SelectionTypeEnum.DRAG, 2, false, new int[] { }, new int[] { 1, 2 }, new int[] { 1, 2 }, new int[] { }).SetName("Drag - NOT Additive, Empty Old, -");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 2, false, new int[] { 0 }, new int[] { 1 }, new int[] { 1 }, new int[] { 0 }).SetName("Drag - NOT Additive, Single Element in Old, Single Element in new, Old does NOT contains new Element");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 1, false, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }).SetName("Drag - NOT Additive, Single Element in Old, Single Element in new, Old contains new Element");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 3, false, new int[] { 0 }, new int[] { 1, 2 }, new int[] { 1, 2 }, new int[] { 0 }).SetName("Drag - NOT Additive, Single Element in Old, Multiple Element in new, Does NOT contains Old");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 2, false, new int[] { 0 }, new int[] { 0, 1 }, new int[] { 0, 1 }, new int[] { 0 }).SetName("Drag - NOT Additive, Single Element in Old, Multiple Element in new, Does Contains Old");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 3, false, new int[] { 0, 1 }, new int[] { 2 }, new int[] { 2 }, new int[] { 0, 1 }).SetName("Drag - NOT Additive, Multiple Element in Old, Single Element in new, Old does NOT contains new Element");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 2, false, new int[] { 0, 1 }, new int[] { 0 }, new int[] { 0 }, new int[] { 0, 1 }).SetName("Drag - NOT Additive, Multiple Element in Old, Single Element in new, Old contains new Element");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 4, false, new int[] { 0, 1 }, new int[] { 2, 3 }, new int[] { 2, 3 }, new int[] { 0, 1 }).SetName("Drag - NOT Additive, Multiple Element in Old, Multiple Element in new, New does NOT contains any from Old");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 3, false, new int[] { 0, 1 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { 0, 1 }).SetName("Drag - NOT Additive, Multiple Element in Old, Multiple Element in new, New Contains All from Old");
                yield return new TestCaseData(SelectionTypeEnum.DRAG, 3, false, new int[] { 0, 1 }, new int[] { 0, 2 }, new int[] { 0, 2 }, new int[] { 0, 1 }).SetName("Drag - NOT Additive, Multiple Element in Old, Multiple Element in new, New does Contains some from Old");

                //Key
                yield return new TestCaseData(SelectionTypeEnum.KEY, 3, true, new int[] { }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { }).SetName("Key - Additive, Empty Old, -");
                yield return new TestCaseData(SelectionTypeEnum.KEY, 6, true, new int[] { 0, 1, 2 }, new int[] { 3, 4, 5 }, new int[] { 0, 1, 2, 3, 4, 5 }, new int[] { }).SetName("Key - Additive, Old Contains none of New");
                yield return new TestCaseData(SelectionTypeEnum.KEY, 3, true, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { }).SetName("Key - Additive, New is equals to Old");
                yield return new TestCaseData(SelectionTypeEnum.KEY, 5, true, new int[] { 0, 1, 2 }, new int[] { 0, 1, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }, new int[] { }).SetName("Key - Additive, Old Contains some of New");
                yield return new TestCaseData(SelectionTypeEnum.KEY, 6, true, new int[] { 0, 1, 2, 3, 4, 5 }, new int[] { 3, 4, 5 }, new int[] { 0, 1, 2 }, new int[] { 3, 4, 5 }).SetName("Key - Additive, Old Contains All of New");

                yield return new TestCaseData(SelectionTypeEnum.KEY, 3, false, new int[] { }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { }).SetName("Key - NOT Additive, Empty Old, -");
                yield return new TestCaseData(SelectionTypeEnum.KEY, 6, false, new int[] { 0, 1, 2 }, new int[] { 3, 4, 5 }, new int[] { 3, 4, 5 }, new int[] { 0, 1, 2 }).SetName("Key - NOT Additive, Old Contains none of New");
                yield return new TestCaseData(SelectionTypeEnum.KEY, 3, false, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }).SetName("Key - NOT Additive, New is equals to Old");
                yield return new TestCaseData(SelectionTypeEnum.KEY, 5, false, new int[] { 0, 1, 2 }, new int[] { 0, 1, 3, 4 }, new int[] { 0, 1, 3, 4 }, new int[] { 0, 1, 2 }).SetName("Key - NOT Additive, Old Contains some of New");
                yield return new TestCaseData(SelectionTypeEnum.KEY, 6, false, new int[] { 0, 1, 2, 3, 4, 5 }, new int[] { 3, 4, 5 }, new int[] { 3, 4, 5 }, new int[] { 0, 1, 2, 3, 4, 5 }).SetName("Key - NOT Additive, Old Contains All of New");

            }
        }

        public AdditiveModifier.SelectionModifier Modifier { get => modifier; set => modifier = value; }
    }
}
