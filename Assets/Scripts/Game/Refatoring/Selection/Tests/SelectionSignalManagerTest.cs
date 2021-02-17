using UnityEngine;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Refactoring;

namespace Tests
{
    [TestFixture]
    public class SelectionSignalManagerTest
    {
        private Selection _selection;
        private SelectionManager _selectionManager;
        private SelectionSignalManager _selectionSignalManager;

        [SetUp]
        public void SetUp()
        {
            _selectionManager = Substitute.ForPartsOf<SelectionManager>(new object[] { default, default, default });
            _selection = Substitute.ForPartsOf<Selection>(new object[] { default, default });
            _selectionSignalManager = Substitute.ForPartsOf<SelectionSignalManager>(new object[] { _selection, _selectionManager });

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
            var mainList = SelectionTestUtils.GetSomeSelectable(amount);
            var expected = mainList.ToList().Take(3).ToArray();
            var startPoint = Vector2.zero;
            var endPoint = Vector2.zero;
            AreaSelectionSignal signal = new AreaSelectionSignal();
            signal.StartPoint = startPoint;
            signal.EndPoint = endPoint;
            _selectionManager.GetAreaSelection(Arg.Any<ISelectable[]>(), Arg.Any<Vector2>(), Arg.Any<Vector2>()).Returns(expected);
            _selectionSignalManager.GetMainList().Returns(mainList);

            _selectionSignalManager.OnAreaSignal(signal);

            _selectionManager.Received().GetAreaSelection(mainList, startPoint, endPoint);
            _selection.Received().DoSelection(expected, SelectionType.AREA);
        }

        [Test]
        public void ShouldNotCallGetSelectionOnAreaSingalIfClickedIsNotNull()
        {
            int amount = 10;
            var mainList = SelectionTestUtils.GetSomeSelectable(amount);
            var expected = mainList.ToList().Take(3).ToArray();
            var startPoint = Vector2.zero;
            var endPoint = Vector2.zero;
            AreaSelectionSignal signal = new AreaSelectionSignal();
            signal.StartPoint = startPoint;
            signal.EndPoint = endPoint;
            _selectionSignalManager.BlockAreaSelection = true;

            _selectionSignalManager.OnAreaSignal(signal);

            _selection.DidNotReceiveWithAnyArgs().DoSelection(expected, SelectionType.AREA);
            _selectionManager.DidNotReceiveWithAnyArgs().GetAreaSelection(Arg.Any<ISelectable[]>(), Arg.Any<Vector2>(), Arg.Any<Vector2>());
        }

        [Test]
        public void ShouldCallGetSelectionOnGroupSingal()
        {
            int amount = 10;
            var mainList = SelectionTestUtils.GetSomeSelectable(amount);
            var expected = mainList.ToList().Take(3).ToArray();
            var groupId = 1;
            GroupSelectionSignal signal = new GroupSelectionSignal();
            signal.GroupId = groupId;
            signal.CreateNew = false;
            _selectionManager.GetGroupSelection(Arg.Any<ISelectable[]>(), Arg.Any<object>()).Returns(expected);
            _selectionSignalManager.GetMainList().Returns(mainList);

            _selectionSignalManager.OnGroupSignal(signal);

            _selectionManager.Received().GetGroupSelection(mainList, groupId);
            _selection.Received().DoSelection(expected, SelectionType.GROUP);
        }

        [Test]
        public void ShouldCallChangeGroupOnGroupSingalWithCreateNewSignal()
        {
            int amount = 10;
            var mainList = SelectionTestUtils.GetSomeSelectable(amount);
            var expected = mainList.ToList().Take(3).ToArray();
            var groupId = 1;
            GroupSelectionSignal signal = new GroupSelectionSignal();
            signal.GroupId = groupId;
            signal.CreateNew = true;
            _selectionManager.WhenForAnyArgs(x => x.SetGroupSelection(default, default)).DoNotCallBase();
            _selection.GetCurrent().Returns(expected);

            _selectionSignalManager.OnGroupSignal(signal);

            _selectionManager.Received().SetGroupSelection(expected, groupId);
            _selection.DidNotReceiveWithAnyArgs().DoSelection(default, default);
        }

        [Test]
        public void ShouldCallGetSelectionOnIndividualSingal()
        {
            int amount = 10;
            var mainList = SelectionTestUtils.GetSomeSelectable(amount);
            var clicked = mainList.ToList().ElementAt(Random.Range(0, amount));
            var expected = new ISelectable[] { clicked };
            IndividualSelectionSignal signal = new IndividualSelectionSignal();
            signal.Clicked = clicked;

            _selectionManager.GetIndividualSelection(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable>()).Returns(expected);
            _selectionSignalManager.GetMainList().Returns(mainList);

            _selectionSignalManager.OnIndividualSignal(signal);

            _selectionManager.Received().GetIndividualSelection(mainList, clicked);
            _selection.Received().DoSelection(expected, SelectionType.INDIVIDUAL);
        }

    }
}
