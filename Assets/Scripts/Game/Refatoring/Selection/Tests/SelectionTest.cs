using UnityEngine;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Refactoring;

namespace Tests
{
    [TestFixture]
    public class SelectionTest
    {
        private Selection _selection;
        private SelectionInterface _selectionInterface;
        private ModifiersInterface _modifiersInterface;
        private SelectionManager _selectionManager;

        [SetUp]
        public void SetUp()
        {
            _selectionInterface = Substitute.ForPartsOf<SelectionInterface>(new object[] { default, default, default });
            _modifiersInterface = Substitute.ForPartsOf<ModifiersInterface>();
            _selectionManager = Substitute.ForPartsOf<SelectionManager>();

            _selection = Substitute.ForPartsOf<Selection>(new object[] { _selectionInterface, _modifiersInterface, _selectionManager });

            _selectionInterface.When(x => x.GetIndividualSelection(Arg.Any<ISelectable[]>(), Arg.Any<ISelectable>())).DoNotCallBase();
            _selectionInterface.When(x => x.GetGroupSelection(Arg.Any<ISelectable[]>(), Arg.Any<object>())).DoNotCallBase();
            _selectionInterface.When(x => x.GetAreaSelection(Arg.Any<ISelectable[]>(), Arg.Any<Vector2>(), Arg.Any<Vector2>())).DoNotCallBase();
            _modifiersInterface.When(x => x.ApplyAll(Arg.Any<ISelectable[]>(), Arg.Any<SelectionType>())).DoNotCallBase();
            _modifiersInterface.ApplyAll(Arg.Any<ISelectable[]>(), Arg.Any<SelectionType>()).Returns(x => x[0]);
            _selectionManager.When(x => x.GetMainList()).DoNotCallBase();
            _selectionManager.When(x => x.SetCurrentSelection(Arg.Any<ISelectable[]>())).DoNotCallBase();
        }

        [Test]
        public void SelectionTestSimplePasses()
        {
            Assert.NotNull(_selection);
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
            _selection.When(x => x.FinalizeSelection(Arg.Any<ISelectable[]>())).DoNotCallBase();
            _selectionManager.GetMainList().Returns(mainList);

            _selection.OnIndividualSignal(signal);

            _selectionInterface.Received().GetIndividualSelection(mainList, clicked);
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
            _selection.When(x => x.FinalizeSelection(Arg.Any<ISelectable[]>())).DoNotCallBase();
            _selectionManager.GetMainList().Returns(mainList);

            _selection.OnGroupSignal(signal);

            _selectionInterface.Received().GetGroupSelection(mainList, groupId);
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
            _selection.When(x => x.FinalizeSelection(Arg.Any<ISelectable[]>())).DoNotCallBase();
            _selectionManager.GetMainList().Returns(mainList);

            _selection.OnAreaSignal(signal);

            _selectionInterface.Received().GetAreaSelection(mainList, startPoint, endPoint);
        }

        [Test]
        public void ShouldFinalizeSelection()
        {
            int amount = 3;
            var expected = SelectionTestUtils.GetSomeSelectable(amount);

            _selection.FinalizeSelection(expected);

            _selectionManager.Received().SetCurrentSelection(expected);
        }


    }
}
