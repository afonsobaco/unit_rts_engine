using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Tests.Utils;
using RTSEngine.Core;
using NSubstitute;
using RTSEngine.RTSSelection;

namespace Tests
{
    [TestFixture]
    public class ModifiersInterfaceTest
    {
        private ModifiersInterface _modifiersInterface;
        private IModifiersComponent _modifiersComponent;
        private ISelectionModifier[] _modifiers;
        private ISelectionModifier multiple_modifier;
        private ISelectionModifier individual_modifier;

        [SetUp]
        public void SetUp()
        {
            _modifiers = GetModifiers();
            _modifiersComponent = Substitute.For<IModifiersComponent>();
            _modifiersComponent.GetModifiers().Returns(_modifiers);
            _modifiersInterface = Substitute.ForPartsOf<ModifiersInterface>(new object[] { _modifiersComponent });
        }

        private ISelectionModifier[] GetModifiers()
        {

            var list = new List<ISelectionModifier>();
            multiple_modifier = Substitute.For<ISelectionModifier>();
            multiple_modifier.RestrictedTypes = new SelectionType[] { SelectionType.MULTIPLE };
            multiple_modifier.Apply(Arg.Any<SelectionInfo>()).Returns(x => (x[0] as SelectionInfo).ActualSelection);
            list.Add(multiple_modifier);
            individual_modifier = Substitute.For<ISelectionModifier>();
            individual_modifier.RestrictedTypes = new SelectionType[] { SelectionType.INDIVIDUAL, SelectionType.UI_SELECTION };
            individual_modifier.Apply(Arg.Any<SelectionInfo>()).Returns(x => (x[0] as SelectionInfo).ActualSelection);
            list.Add(individual_modifier);
            return list.ToArray();
        }

        [Test]
        public void ModifierInterfaceTestSimplePasses()
        {
            Assert.NotNull(_modifiersInterface);
        }

        [Test]
        public void ShouldApplyAllModifiersOfAreaType()
        {
            SelectionType type = SelectionType.MULTIPLE;
            ISelectable[] newSelection = TestUtils.GetSomeObjects(Random.Range(1, 5));
            ISelectable[] oldSelection = new ISelectable[] { };
            var result = _modifiersInterface.ApplyAll(oldSelection, newSelection, type);

            CollectionAssert.AreEqual(newSelection, result);
            AssertModifiersReceived(type, newSelection, oldSelection);
        }

        [Test]
        public void ShouldApplyAllModifiersOfIndividualType()
        {
            SelectionType type = SelectionType.INDIVIDUAL;
            ISelectable[] newSelection = TestUtils.GetSomeObjects(Random.Range(1, 5));
            ISelectable[] oldSelection = new ISelectable[] { };
            var result = _modifiersInterface.ApplyAll(oldSelection, newSelection, type);

            CollectionAssert.AreEqual(newSelection, result);
            AssertModifiersReceived(type, newSelection, oldSelection);
        }

        [Test]
        public void ShouldApplyAllModifiersOfIndividualUISelectionType()
        {
            SelectionType type = SelectionType.UI_SELECTION;
            ISelectable[] newSelection = TestUtils.GetSomeObjects(Random.Range(1, 5));
            ISelectable[] oldSelection = new ISelectable[] { };
            var result = _modifiersInterface.ApplyAll(oldSelection, newSelection, type);

            CollectionAssert.AreEqual(newSelection, result);
            AssertModifiersReceived(type, newSelection, oldSelection);
        }

        private void AssertModifiersReceived(SelectionType type, ISelectable[] newSelection, ISelectable[] oldSelection)
        {
            foreach (var modifier in _modifiers)
            {
                if (modifier.RestrictedTypes == null || modifier.RestrictedTypes.Length == 0 || modifier.RestrictedTypes.Contains(type))
                    modifier.Received().Apply(Arg.Any<SelectionInfo>());
                else
                    modifier.DidNotReceiveWithAnyArgs().Apply(default);
            }
        }

    }
}
