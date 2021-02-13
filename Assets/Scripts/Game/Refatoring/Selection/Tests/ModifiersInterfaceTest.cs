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
        private ISelectionModifier[] modifiers;

        [SetUp]
        public void SetUp()
        {
            modifiers = GetModifiers();
            _modifiersInterface = Substitute.ForPartsOf<ModifiersInterface>(new object[] { modifiers });
        }

        private ISelectionModifier[] GetModifiers()
        {

            var list = new List<ISelectionModifier>();
            ISelectionModifier modifier = Substitute.For<ISelectionModifier>();
            modifier.Type = SelectionType.ANY;
            modifier.Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<SelectionType>()).Returns(x => x[1]);
            list.Add(modifier);
            modifier = Substitute.For<ISelectionModifier>();
            modifier.Type = SelectionType.AREA;
            modifier.Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<SelectionType>()).Returns(x => x[1]);
            list.Add(modifier);
            modifier = Substitute.For<ISelectionModifier>();
            modifier.Type = SelectionType.GROUP;
            modifier.Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<SelectionType>()).Returns(x => x[1]);
            list.Add(modifier);
            modifier = Substitute.For<ISelectionModifier>();
            modifier.Type = SelectionType.INDIVIDUAL;
            modifier.Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<SelectionType>()).Returns(x => x[1]);
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
            foreach (var m in modifiers)
            {
                if (m.Type == type || m.Type == SelectionType.ANY)
                    m.Received().Apply(Arg.Is(oldSelection), Arg.Is(newSelection), Arg.Any<ISelectable[]>(), type);
                else
                {
                    m.DidNotReceive().Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Is(SelectionType.GROUP));
                    m.DidNotReceive().Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Is(SelectionType.INDIVIDUAL));
                }
            }
        }

        [Test]
        public void ShouldApplyAllModifiersOfGroupType()
        {
            SelectionType type = SelectionType.GROUP;
            ISelectable[] newSelection = SelectionTestUtils.GetSomeSelectable(Random.Range(1, 5));
            ISelectable[] oldSelection = new ISelectable[] { };
            var result = _modifiersInterface.ApplyAll(oldSelection, newSelection, type);

            CollectionAssert.AreEqual(newSelection, result);
            foreach (var m in modifiers)
            {
                if (m.Type == type || m.Type == SelectionType.ANY)
                    m.Received().Apply(Arg.Is(oldSelection), Arg.Is(newSelection), Arg.Any<ISelectable[]>(), type);
                else
                {
                    m.DidNotReceive().Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Is(SelectionType.AREA));
                    m.DidNotReceive().Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Is(SelectionType.INDIVIDUAL));
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
            foreach (var m in modifiers)
            {
                if (m.Type == type || m.Type == SelectionType.ANY)
                    m.Received().Apply(Arg.Is(oldSelection), Arg.Is(newSelection), Arg.Any<ISelectable[]>(), type);
                else
                {
                    m.DidNotReceive().Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Is(SelectionType.AREA));
                    m.DidNotReceive().Apply(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Any<ISelectable[]>(), Arg.Is(SelectionType.GROUP));
                }
            }
        }


    }
}
