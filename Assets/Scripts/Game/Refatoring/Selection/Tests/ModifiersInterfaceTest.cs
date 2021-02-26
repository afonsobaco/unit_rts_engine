using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using NSubstitute;

namespace Tests
{
    [TestFixture]
    public class ModifiersInterfaceTest
    {
        private ModifiersInterface _modifiersInterface;
        private IModifiersComponent _modifiersComponent;
        private ISelectionModifier[] _modifiers;

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
            ISelectionModifier modifier = Substitute.For<ISelectionModifier>();
            modifier.Type = SelectionType.ANY;
            modifier.Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>()).Returns(x => x[1]);
            list.Add(modifier);
            modifier = Substitute.For<ISelectionModifier>();
            modifier.Type = SelectionType.AREA;
            modifier.Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>()).Returns(x => x[1]);
            list.Add(modifier);
            modifier = Substitute.For<ISelectionModifier>();
            modifier.Type = SelectionType.PARTY;
            modifier.Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>()).Returns(x => x[1]);
            list.Add(modifier);
            modifier = Substitute.For<ISelectionModifier>();
            modifier.Type = SelectionType.INDIVIDUAL;
            modifier.Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>()).Returns(x => x[1]);
            list.Add(modifier);
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
            SelectionType type = SelectionType.AREA;
            ISelectable[] newSelection = SelectionTestUtils.GetSomeSelectable(Random.Range(1, 5));
            ISelectable[] oldSelection = new ISelectable[] { };
            var result = _modifiersInterface.ApplyAll(oldSelection, newSelection, type);

            CollectionAssert.AreEqual(newSelection, result);
            foreach (var m in _modifiers)
            {
                if (m.Type == type || m.Type == SelectionType.ANY)
                    m.Received().Apply(Arg.Is(oldSelection), Arg.Is(newSelection), Arg.Any<ISelectable[]>());
                else
                {
                    m.DidNotReceive().Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>());
                    m.DidNotReceive().Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>());
                }
            }
        }

        [Test]
        public void ShouldApplyAllModifiersOfType()
        {
            SelectionType type = SelectionType.PARTY;
            ISelectable[] newSelection = SelectionTestUtils.GetSomeSelectable(Random.Range(1, 5));
            ISelectable[] oldSelection = new ISelectable[] { };
            var result = _modifiersInterface.ApplyAll(oldSelection, newSelection, type);

            CollectionAssert.AreEqual(newSelection, result);
            foreach (var m in _modifiers)
            {
                if (m.Type == type || m.Type == SelectionType.ANY)
                    m.Received().Apply(Arg.Is(oldSelection), Arg.Is(newSelection), Arg.Any<ISelectable[]>());
                else
                {
                    m.DidNotReceive().Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>());
                    m.DidNotReceive().Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>());
                }
            }
        }

        [Test]
        public void ShouldApplyAllModifiersOfIndividualType()
        {
            SelectionType type = SelectionType.INDIVIDUAL;
            ISelectable[] newSelection = SelectionTestUtils.GetSomeSelectable(Random.Range(1, 5));
            ISelectable[] oldSelection = new ISelectable[] { };
            var result = _modifiersInterface.ApplyAll(oldSelection, newSelection, type);

            CollectionAssert.AreEqual(newSelection, result);
            foreach (var m in _modifiers)
            {
                if (m.Type == type || m.Type == SelectionType.ANY)
                    m.Received().Apply(Arg.Is(oldSelection), Arg.Is(newSelection), Arg.Any<ISelectable[]>());
                else
                {
                    m.DidNotReceive().Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>());
                    m.DidNotReceive().Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>());
                }
            }
        }


    }
}
