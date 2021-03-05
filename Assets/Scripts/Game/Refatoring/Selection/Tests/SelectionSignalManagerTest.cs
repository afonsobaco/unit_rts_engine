using UnityEngine;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using RTSEngine.Signal;
using RTSEngine.Utils;
using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class SelectionSignalManagerTest
    {
        private Selection _selection;
        private SelectionManager _selectionManager;
        private IRuntimeSet<ISelectable> _mainList;
        private SelectionSignalManager _selectionSignalManager;

        private GameSignalBus _signalBus;

        [SetUp]
        public void SetUp()
        {
            _selectionManager = Substitute.ForPartsOf<SelectionManager>(new object[] { default, default });
            _mainList = Substitute.For<IRuntimeSet<ISelectable>>();
            _selection = Substitute.ForPartsOf<Selection>(new object[] { default, _mainList });
            _signalBus = Substitute.ForPartsOf<GameSignalBus>(new object[] { default });
            _signalBus.WhenForAnyArgs(x => x.Fire(default)).DoNotCallBase();
            _selectionSignalManager = Substitute.ForPartsOf<SelectionSignalManager>(new object[] { _selection, _selectionManager, _mainList, _signalBus });

            _selection.When(x => x.DoSelection(Arg.Any<ISelectable[]>(), Arg.Any<SelectionType>())).DoNotCallBase();
            _selectionSignalManager.When(x => x.GetMainList()).DoNotCallBase();
        }

        [Test]
        public void SelectionTestSimplePasses()
        {
            Assert.NotNull(_selectionSignalManager);
        }

        [Test]
        public void ShouldCallGetSelectionOnAreaSingal()
        {
            int amount = 10;
            var mainList = TestUtils.GetSomeObjects(amount);
            var expected = mainList.ToList().Take(3).ToArray();
            var startPoint = Vector2.zero;
            var endPoint = Vector2.zero;
            AreaSelectionSignal signal = new AreaSelectionSignal();
            signal.StartPoint = startPoint;
            signal.EndPoint = endPoint;
            _selectionManager.GetAreaSelection(Arg.Any<ISelectable[]>(), Arg.Any<Vector2>(), Arg.Any<Vector2>()).Returns(expected);
            _selectionSignalManager.GetMainList().Returns(mainList);

            _selectionSignalManager.OnAreaSelectionSignal(signal);

            _selectionManager.Received().GetAreaSelection(mainList, startPoint, endPoint);
            _selection.Received().DoSelection(expected, SelectionType.MULTIPLE);
        }

        [Test]
        public void ShouldNotCallGetSelectionOnAreaSingalIfClickedIsNotNull()
        {
            int amount = 10;
            var mainList = TestUtils.GetSomeObjects(amount);
            var expected = mainList.ToList().Take(3).ToArray();
            var startPoint = Vector2.zero;
            var endPoint = Vector2.zero;
            AreaSelectionSignal signal = new AreaSelectionSignal();
            signal.StartPoint = startPoint;
            signal.EndPoint = endPoint;
            _selectionSignalManager.BlockAreaSelection = true;

            _selectionSignalManager.OnAreaSelectionSignal(signal);

            _selection.DidNotReceiveWithAnyArgs().DoSelection(expected, SelectionType.MULTIPLE);
            _selectionManager.DidNotReceiveWithAnyArgs().GetAreaSelection(Arg.Any<ISelectable[]>(), Arg.Any<Vector2>(), Arg.Any<Vector2>());
        }

        [Test]
        public void ShouldCallGetSelectionOnIndividualSingal()
        {
            int amount = 10;
            var mainList = TestUtils.GetSomeObjects(amount);
            var clicked = mainList.ToList().ElementAt(Random.Range(0, amount));
            var expected = new ISelectable[] { clicked };
            IndividualSelectionSignal signal = new IndividualSelectionSignal();
            signal.Clicked = clicked;

            _selectionManager.GetIndividualSelection(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable>()).Returns(expected);
            _selectionSignalManager.GetMainList().Returns(mainList);

            _selectionSignalManager.OnIndividualSelectionSignal(signal);

            _selectionManager.Received().GetIndividualSelection(mainList, clicked);
            _selection.Received().DoSelection(expected, SelectionType.INDIVIDUAL);
        }

        [Test]
        public void ShouldCallGetSelectionOnChangeSelectionSingal()
        {
            int amount = 10;
            var mainList = TestUtils.GetSomeObjects(amount);
            var selection = mainList.ToList().Take(Random.Range(1, amount)).ToArray();
            var expected = selection;
            ChangeSelectionSignal signal = new ChangeSelectionSignal();
            signal.Selection = selection;

            _selectionSignalManager.OnChangeSelectionSignal(signal);

            _selection.Received().DoSelection(expected, SelectionType.UI_SELECTION);
        }

    }
}
