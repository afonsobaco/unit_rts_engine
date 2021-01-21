using NSubstitute;
using NUnit.Framework;
using RTSEngine.Core.Impls;
using RTSEngine.Core.Enums;
using RTSEngine.Manager.Abstracts;
using RTSEngine.Manager.Enums;
using RTSEngine.Manager.Impls;
using RTSEngine.Manager.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Manager
{
    [TestFixture]
    public class AbstractSelectionManagerTest
    {
        private AbstractSelectionManager<SelectableObject, SelectionTypeEnum, ObjectTypeEnum> manager;

        [SetUp]
        public void Init()
        {
            manager = new DerivedClass();
        }

        [TearDown]
        public void Finish()
        {

        }


        [Test]
        public void ShouldReturnDefaultArgsWhenGetSelectionArgsWithNull()
        {
            List<SelectableObject> oldSelection = null;
            List<SelectableObject> newSelection = null;
            var args = manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.DRAG);

            AssertArgs(SelectionManagerTestUtils.GetDefaultArgs<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>(), args);
        }

        [Test]
        public void ShouldReturnDefaultArgsWhenGetSelectionArgsWithEmpty()
        {
            List<SelectableObject> oldSelection = new List<SelectableObject>();
            List<SelectableObject> newSelection = new List<SelectableObject>();

            var args = manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.DRAG);

            AssertArgs(SelectionManagerTestUtils.GetDefaultArgs<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>(), args);
        }

        [Test]
        public void ShouldReturnCustomArgsWhenGetSelectionArgsWithCustom()
        {
            List<SelectableObject> oldSelection = new List<SelectableObject>() { SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(0) };
            List<SelectableObject> newSelection = new List<SelectableObject>() { SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(1) };
            SelectionArgsXP<SelectableObject, SelectionTypeEnum, ObjectTypeEnum> expected = SelectionManagerTestUtils.GetDefaultArgs<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>();
            expected.SelectionType = SelectionTypeEnum.CLICK;
            expected.OldSelection = oldSelection;
            expected.NewSelection = newSelection;

            var args = manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.CLICK);

            AssertArgs(expected, args);
        }

        [Test]
        public void ShouldGetModsBySelectionType()
        {
            List<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>> mods = GetModsToTest();
            SelectionTypeEnum type = SelectionTypeEnum.CLICK;
            mods[0].Type = type;
            mods[2].Type = type;

            var result = manager.GetModsBySelectionType(mods, type);

            Assert.AreEqual(2, result.Count);
            Assert.True(result.Contains(mods[0]));
            Assert.True(result.Contains(mods[2]));
        }


        [Test]
        public void ShouldGetEmptyWhenModsDoesNotContainsSelectionType()
        {
            List<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>> mods = GetModsToTest();
            SelectionTypeEnum type = SelectionTypeEnum.CLICK;

            var result = manager.GetModsBySelectionType(mods, type);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void ShouldApplyModsToArgs()
        {
            List<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>> mods = GetModsToTest();
            mods[0].Type = SelectionTypeEnum.CLICK;

            var args = SelectionManagerTestUtils.GetDefaultArgs<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>();
            args.SelectionType = SelectionTypeEnum.DRAG;

            var result = manager.ApplyModifiers(args);

        }


        [Test]
        public void ShouldFinalizeSelectionCorrectlyWhenHasSomethingToBeAdded()
        {
            var args = SelectionManagerTestUtils.GetDefaultArgs<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>();
            var amount = 4;
            List<SelectableObject> selection = new List<SelectableObject>();
            List<SelectableObject> expected = new List<SelectableObject>();
            for (var i = 0; i < amount; i++)
            {
                var obj = SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(i);
                selection.Add(obj);
                expected.Add(obj);
            }
            args.NewSelection = selection;
            args.ToBeAdded = selection;

            var result = manager.FinalizeSelection(args);

            Assert.AreEqual(expected, result);
            foreach (var item in result)
            {
                Assert.True(item.IsSelected);
            }
        }

        [Test]
        public void ShouldFinalizeSelectionCorrectlyWhenHasSomethingToBeRemoved()
        {
            var args = SelectionManagerTestUtils.GetDefaultArgs<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>();
            var amount = 4;
            List<SelectableObject> expected = new List<SelectableObject>();
            for (var i = 0; i < amount; i++)
            {
                var obj = SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(i);
                args.NewSelection.Add(obj);
                if (i > 1)
                {

                    args.ToBeAdded.Add(obj);
                    expected.Add(obj);
                }
                else
                {
                    args.ToBeRemoved.Add(obj);
                }
            }

            var result = manager.FinalizeSelection(args);

            CollectionAssert.AreEquivalent(expected, result);
            foreach (var item in args.ToBeAdded)
            {
                Assert.True(item.IsSelected);
            }
            foreach (var item in args.NewSelection.FindAll(a => !result.Contains(a)))
            {
                Assert.False(item.IsSelected);
            }
        }

        [Test]
        public void ShouldUpdateSelectionStatus()
        {
            List<SelectableObject> expected = new List<SelectableObject>();
            for (var i = 0; i < 4; i++)
            {
                SelectableObject item = SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(i);
                item.IsSelected = false;
                expected.Add(item);
            }

            //Act
            var actual = manager.UpdateSelectionStatus(expected, true);
            CollectionAssert.AreEquivalent(expected, actual);
            foreach (var item in expected)
            {
                Assert.True(item.IsSelected);
            }

            actual = manager.UpdateSelectionStatus(expected, false);
            CollectionAssert.AreEquivalent(expected, actual);
            foreach (var item in expected)
            {
                Assert.False(item.IsSelected);
            }
        }

        [Test]
        public void ShouldUpdatePreSelectionStatus()
        {
            List<SelectableObject> expected = new List<SelectableObject>();
            for (var i = 0; i < 4; i++)
            {
                SelectableObject item = SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(i);
                item.IsPreSelected = false;
                expected.Add(item);
            }

            //Act
            var actual = manager.UpdatePreSelectionStatus(expected, true);
            CollectionAssert.AreEquivalent(expected, actual);
            foreach (var item in expected)
            {
                Assert.True(item.IsPreSelected);
            }

            actual = manager.UpdatePreSelectionStatus(expected, false);
            CollectionAssert.AreEquivalent(expected, actual);
            foreach (var item in expected)
            {
                Assert.False(item.IsPreSelected);
            }
        }

        #region methods

        private static List<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>> GetModsToTest()
        {
            List<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>> mods = new List<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>>();
            mods.Add(Substitute.For<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>>());
            mods.Add(Substitute.For<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>>());
            mods.Add(Substitute.For<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>>());
            foreach (var mod in mods)
            {
                mod.Apply(Arg.Any<SelectionArgsXP<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>>()).Returns(x => x[0]);
            }

            return mods;
        }

        private void AssertArgs<T, E, O>(SelectionArgsXP<T, E, O> expected, SelectionArgsXP<T, E, O> actual)
        {
            CollectionAssert.AreEquivalent(expected.OldSelection, actual.OldSelection);
            CollectionAssert.AreEquivalent(expected.NewSelection, actual.NewSelection);
            CollectionAssert.AreEquivalent(expected.ToBeAdded, actual.ToBeAdded);
            CollectionAssert.AreEquivalent(expected.ToBeRemoved, actual.ToBeRemoved);
            Assert.AreEqual(expected.SelectionType, actual.SelectionType);
        }

        #endregion
    }

    internal class DerivedClass : AbstractSelectionManager<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>
    {
        public override ISelectionSettings<SelectableObject, SelectionTypeEnum, ObjectTypeEnum> GetSettings()
        {
            return Substitute.For<ISelectionSettings<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>>();
        }
    }
}
