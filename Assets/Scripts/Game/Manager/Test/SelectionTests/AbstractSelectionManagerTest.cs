using NUnit.Framework;
using RTSEngine.Manager;
using System.Collections.Generic;

namespace Tests.Manager
{
    [TestFixture]
    public class AbstractSelectionManagerTest
    {
        private AbstractSelectionManager<SelectableObject, SelectionTypeEnum> manager;

        public AbstractSelectionManager<SelectableObject, SelectionTypeEnum> Manager { get => manager; set => manager = value; }

        [SetUp]
        public void Init()
        {
            Manager = new DerivedClass();
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
            var args = Manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.DRAG);

            ISelectionArgsXP<SelectableObject, SelectionTypeEnum> expected = SelectionManagerTestUtils.GetDefaultArgs<SelectableObject, SelectionTypeEnum>();
            expected.SelectionType = SelectionTypeEnum.DRAG;
            AssertArgs(expected, args);
        }

        [Test]
        public void ShouldReturnDefaultArgsWhenGetSelectionArgsWithEmpty()
        {
            List<SelectableObject> oldSelection = new List<SelectableObject>();
            List<SelectableObject> newSelection = new List<SelectableObject>();

            var args = Manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.DRAG);

            ISelectionArgsXP<SelectableObject, SelectionTypeEnum> expected = SelectionManagerTestUtils.GetDefaultArgs<SelectableObject, SelectionTypeEnum>();
            expected.SelectionType = SelectionTypeEnum.DRAG;

            AssertArgs(expected, args);
        }

        [Test]
        public void ShouldReturnCustomArgsWhenGetSelectionArgsWithCustom()
        {
            List<SelectableObject> oldSelection = new List<SelectableObject>() { SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(0) };
            List<SelectableObject> newSelection = new List<SelectableObject>() { SelectionManagerTestUtils.CreateATestableObject<SelectableObject>(1) };
            ISelectionArgsXP<SelectableObject, SelectionTypeEnum> expected = SelectionManagerTestUtils.GetDefaultArgs<SelectableObject, SelectionTypeEnum>();
            expected.SelectionType = SelectionTypeEnum.CLICK;
            expected.OldSelection = oldSelection;
            expected.NewSelection = newSelection;

            var args = Manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.CLICK);

            AssertArgs(expected, args);
        }

        [Test]
        public void ShouldFinalizeSelectionCorrectlyWhenHasSomethingToBeAdded()
        {
            var args = SelectionManagerTestUtils.GetDefaultArgs<SelectableObject, SelectionTypeEnum>();
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

            var result = Manager.FinalizeSelection(args);

            Assert.AreEqual(expected, result);
            foreach (var item in result)
            {
                Assert.True(item.IsSelected);
            }
        }

        [Test]
        public void ShouldFinalizeSelectionCorrectlyWhenHasSomethingToBeRemoved()
        {
            var args = SelectionManagerTestUtils.GetDefaultArgs<SelectableObject, SelectionTypeEnum>();
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

            var result = Manager.FinalizeSelection(args);

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
            var actual = Manager.UpdateSelectionStatus(expected, true);
            CollectionAssert.AreEquivalent(expected, actual);
            foreach (var item in expected)
            {
                Assert.True(item.IsSelected);
            }

            actual = Manager.UpdateSelectionStatus(expected, false);
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
            var actual = Manager.UpdatePreSelectionStatus(expected, true);
            CollectionAssert.AreEquivalent(expected, actual);
            foreach (var item in expected)
            {
                Assert.True(item.IsPreSelected);
            }

            actual = Manager.UpdatePreSelectionStatus(expected, false);
            CollectionAssert.AreEquivalent(expected, actual);
            foreach (var item in expected)
            {
                Assert.False(item.IsPreSelected);
            }
        }

        [Test]
        public void ShouldGetModsBySelectionType()
        {
            var mods = SelectionManagerTestUtils.GetSomeMods<SelectableObject, SelectionTypeEnum>(4);
            SelectionTypeEnum type = SelectionTypeEnum.CLICK;
            mods[0].Type = type;
            mods[2].Type = type;

            var result = Manager.GetModsBySelectionType(mods, type);

            Assert.AreEqual(2, result.Count);
            Assert.True(result.Contains(mods[0]));
            Assert.True(result.Contains(mods[2]));
        }


        [Test]
        public void ShouldGetEmptyWhenModsDoesNotContainsSelectionType()
        {
            var mods = SelectionManagerTestUtils.GetSomeMods<SelectableObject, SelectionTypeEnum>(4);
            SelectionTypeEnum type = SelectionTypeEnum.CLICK;

            var result = Manager.GetModsBySelectionType(mods, type);

            Assert.AreEqual(0, result.Count);
        }

        #region methods

        private void AssertArgs<T, ST>(ISelectionArgsXP<T, ST> expected, ISelectionArgsXP<T, ST> actual)
        {
            CollectionAssert.AreEquivalent(expected.OldSelection, actual.OldSelection);
            CollectionAssert.AreEquivalent(expected.NewSelection, actual.NewSelection);
            CollectionAssert.AreEquivalent(expected.ToBeAdded, actual.ToBeAdded);
            CollectionAssert.AreEquivalent(expected.ToBeRemoved, actual.ToBeRemoved);
            Assert.AreEqual(expected.SelectionType, actual.SelectionType);
        }

        #endregion
    }

    internal class DerivedClass : AbstractSelectionManager<SelectableObject, SelectionTypeEnum>
    {
        public override ISelectionArgsXP<SelectableObject, SelectionTypeEnum> ApplyModifiers(ISelectionArgsXP<SelectableObject, SelectionTypeEnum> args)
        {
            return args;
        }
        public override List<ISelectionMod<SelectableObject, SelectionTypeEnum>> GetModifiersToBeApplied(SelectionTypeEnum selectionType)
        {
            return new List<ISelectionMod<SelectableObject, SelectionTypeEnum>>();
        }
    }
}
