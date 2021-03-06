using NUnit.Framework;
using RTSEngine.Core;
using NSubstitute;
using RTSEngine.RTSSelection;

namespace Tests
{
    [TestFixture]
    public class SelectionManagerTest
    {

        private SelectionManager _selectionInterface;
        private IAreaSelection _areaSelection;
        private IIndividualSelection _individualSelection;

        [SetUp]
        public void SetUp()
        {
            _areaSelection = Substitute.For<IAreaSelection>();
            _individualSelection = Substitute.For<IIndividualSelection>();
            _selectionInterface = Substitute.ForPartsOf<SelectionManager>(new object[]{
                _areaSelection, _individualSelection
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
            _individualSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default);
        }

        [Test]
        public void ShouldGetSelectionFromIndividualSelectionType()
        {
            ISelectable[] result = _selectionInterface.GetIndividualSelection(default, default);

            _individualSelection.ReceivedWithAnyArgs(1).GetSelection(default, default);
            _areaSelection.DidNotReceiveWithAnyArgs().GetSelection(default, default, default);
        }
    }
}
