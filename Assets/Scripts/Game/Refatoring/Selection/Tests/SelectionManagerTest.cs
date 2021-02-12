using UnityEngine;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Refactoring;

namespace Tests
{
    [TestFixture]
    public class SelectionManagerTest
    {
        private Selection _selection;
        private SelectionInterface _selectionInterface;
        private SelectionManager _selectionManager;

        [SetUp]
        public void SetUp()
        {
            _selectionInterface = Substitute.ForPartsOf<SelectionInterface>(new object[] { default, default, default });
            _selection = Substitute.ForPartsOf<Selection>(new object[] { default, default });
            _selectionManager = Substitute.ForPartsOf<SelectionManager>(new object[] { _selection, _selectionInterface });

            _selection.When(x => x.DoSelection(Arg.Any<ISelectable[]>(), Arg.Any<SelectionType>())).DoNotCallBase();
            _selectionManager.When(x => x.GetMainList()).DoNotCallBase();
        }

        [Test]
        public void SelectionTestSimplePasses()
        {
            Assert.NotNull(_selectionManager);
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
            _selectionInterface.GetAreaSelection(Arg.Any<ISelectable[]>(), Arg.Any<Vector2>(), Arg.Any<Vector2>()).Returns(expected);
            _selectionManager.GetMainList().Returns(mainList);

            _selectionManager.OnAreaSignal(signal);

            _selectionInterface.Received().GetAreaSelection(mainList, startPoint, endPoint);
            _selection.Received().DoSelection(expected, SelectionType.AREA);
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
            _selectionInterface.GetGroupSelection(Arg.Any<ISelectable[]>(), Arg.Any<object>()).Returns(expected);
            _selectionManager.GetMainList().Returns(mainList);

            _selectionManager.OnGroupSignal(signal);

            _selectionInterface.Received().GetGroupSelection(mainList, groupId);
            _selection.Received().DoSelection(expected, SelectionType.GROUP);
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

            _selectionInterface.GetIndividualSelection(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable>()).Returns(expected);
            _selectionManager.GetMainList().Returns(mainList);

            _selectionManager.OnIndividualSignal(signal);

            _selectionInterface.Received().GetIndividualSelection(mainList, clicked);
            _selection.Received().DoSelection(expected, SelectionType.INDIVIDUAL);
        }

    }
}
