using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using NSubstitute;

namespace Tests
{
    [TestFixture]

    public class SelectionManagerTest
    {

        private SelectionManager _selectionInterface;
        private IAreaSelection _areaSelection;
        private IPartySelection _partySelection;
        private IIndividualSelection _individualSelection;

        [SetUp]
        public void SetUp()
        {
            _areaSelection = Substitute.For<IAreaSelection>();
            _partySelection = Substitute.For<IPartySelection>();
            _individualSelection = Substitute.For<IIndividualSelection>();
            _selectionInterface = Substitute.ForPartsOf<SelectionManager>(new object[]{
                _areaSelection, _partySelection, _individualSelection
            });
        }

        [Test]
        public void SelectionManagerTestSimplePasses()
        {
            Assert.NotNull(_selectionInterface);
        }


        [Test]
        public void ShouldGetSelectionFromAreaSelectionType()
        {
            ISelectable[] result = _selectionInterface.GetAreaSelection(default, default, default);

            _areaSelection.ReceivedWithAnyArgs(1).GetSelection(default, default, default);
            _partySelection.DidNotReceiveWithAnyArgs().GetSelection(default, default);
            _individualSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default);
        }

        [Test]
        public void ShouldGetSelectionFromPartySelectionType()
        {
            ISelectable[] result = _selectionInterface.GetPartySelection(default, default);

            _partySelection.ReceivedWithAnyArgs(1).GetSelection(default, default);
            _areaSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default, default);
            _individualSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default);
        }

        [Test]
        public void ShouldSetSelectionFromPartySelectionType()
        {
            _selectionInterface.SetPartySelection(default, default);

            _partySelection.ReceivedWithAnyArgs(1).ChangeParty(default, default);
            _partySelection.DidNotReceiveWithAnyArgs().GetSelection(default, default);
            _areaSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default, default);
            _individualSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default);
        }

        [Test]
        public void ShouldGetSelectionFromIndividualSelectionType()
        {
            ISelectable[] result = _selectionInterface.GetIndividualSelection(default, default);

            _individualSelection.ReceivedWithAnyArgs(1).GetSelection(default, default);
            _areaSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default, default);
            _partySelection.DidNotReceiveWithAnyArgs().GetSelection(default, default);
        }
    }
}
