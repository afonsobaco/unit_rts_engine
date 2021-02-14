using System.ComponentModel.Design;
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

    public class SelectionInterfaceTest
    {

        private SelectionInterface _selectionInterface;
        private IAreaSelection _areaSelection;
        private IGroupSelection _groupSelection;
        private IIndividualSelection _individualSelection;

        [SetUp]
        public void SetUp()
        {
            _areaSelection = Substitute.For<IAreaSelection>();
            _groupSelection = Substitute.For<IGroupSelection>();
            _individualSelection = Substitute.For<IIndividualSelection>();
            _selectionInterface = Substitute.ForPartsOf<SelectionInterface>(new object[]{
                _areaSelection, _groupSelection, _individualSelection
            });
        }

        [Test]
        public void SelectionInterfaceTestSimplePasses()
        {
            Assert.NotNull(_selectionInterface);
        }


        [Test]
        public void ShouldGetSelectionFromAreaSelectionType()
        {
            ISelectable[] result = _selectionInterface.GetAreaSelection(default, default, default);

            _areaSelection.ReceivedWithAnyArgs(1).GetSelection(default, default, default);
            _groupSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default);
            _individualSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default);
        }

        [Test]
        public void ShouldGetSelectionFromGroupSelectionType()
        {
            ISelectable[] result = _selectionInterface.GetGroupSelection(default, default);

            _groupSelection.ReceivedWithAnyArgs(1).GetSelection(default, default);
            _areaSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default, default);
            _individualSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default);
        }

        [Test]
        public void ShouldSetSelectionFromGroupSelectionType()
        {
            _selectionInterface.SetGroupSelection(default, default);

            _groupSelection.ReceivedWithAnyArgs(1).ChangeGroup(default, default);
            _groupSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default);
            _areaSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default, default);
            _individualSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default);
        }

        [Test]
        public void ShouldGetSelectionFromIndividualSelectionType()
        {
            ISelectable[] result = _selectionInterface.GetIndividualSelection(default, default);

            _individualSelection.ReceivedWithAnyArgs(1).GetSelection(default, default);
            _areaSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default, default);
            _groupSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default);
        }
    }
}
