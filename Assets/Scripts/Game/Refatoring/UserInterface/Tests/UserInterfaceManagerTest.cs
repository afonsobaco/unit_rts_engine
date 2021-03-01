using System;
using System.Linq;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using RTSEngine.Signal;
using NSubstitute;
using Tests.Utils;
using RTSEngine.Utils;
using RTSEngine.Commons;

namespace Tests
{
    [TestFixture]
    public class UserInterfaceManagerTest
    {
        private UserInterfaceManager _userInterfaceManager;
        private EqualityComparerComponent _equalityComparer;
        private GameSignalBus _signalBus;

        [SetUp]
        public void SetUp()
        {
            _signalBus = Substitute.ForPartsOf<GameSignalBus>(new object[] { default });
            _equalityComparer = Substitute.ForPartsOf<EqualityComparerComponent>();
            _userInterfaceManager = Substitute.ForPartsOf<UserInterfaceManager>(new object[] { _signalBus, _equalityComparer });
            _signalBus.WhenForAnyArgs(x => x.Fire(default)).DoNotCallBase();
        }

        [Test]
        public void UserInterfaceManagerTestSimplePasses()
        {
            Assert.NotNull(_userInterfaceManager);
        }

        [Test]
        public void ShouldReturnClickedWhenDoMiniatureClicked()
        {
            ISelectable[] selection = GetSubGroups(4);
            ISelectable clicked = selection[0];
            _userInterfaceManager.DoMiniatureClicked(clicked);
            _signalBus.Received().Fire(Arg.Is<ChangeSelectionSignal>(
                arg => CompareArrays(arg.Selection, new ISelectable[] { clicked })
            ));
        }

        [Test]
        public void ShouldRemoveClickedFromSelectionWhenDoMiniatureClicked()
        {
            ISelectable[] selection = GetSubGroups(4);
            ISelectable clicked = selection[0];
            ISelectable[] expected = selection.ToList().FindAll(x => !x.Equals(clicked)).ToArray();
            _userInterfaceManager.DoMiniatureClicked(clicked);
            _signalBus.Received().Fire(Arg.Is<ChangeSelectionSignal>(
                arg => CompareArrays(arg.Selection, expected)
            ));
        }

        [Test]
        public void ShouldReturnSubGroupFromSelectionWhenDoMiniatureClicked()
        {
            const int Amount = 4;
            ISelectable[] selection = GetSubGroups(Amount);
            ISelectable clicked = selection[0];
            ISelectable[] expected = selection.ToList().FindAll(x => x.Index < Amount / 2).ToArray();
            _userInterfaceManager.DoMiniatureClicked(clicked);
            _signalBus.Received().Fire(Arg.Is<ChangeSelectionSignal>(
                arg => CompareArrays(arg.Selection, expected)
            ));
        }

        [Test]
        public void ShouldRemoveSubGroupFromSelectionWhenDoMiniatureClicked()
        {
            const int Amount = 4;
            ISelectable[] selection = GetSubGroups(Amount);
            ISelectable clicked = selection[0];
            ISelectable[] expected = selection.ToList().FindAll(x => x.Index >= Amount / 2).ToArray();
            _userInterfaceManager.DoMiniatureClicked(clicked);
            _signalBus.Received().Fire(Arg.Is<ChangeSelectionSignal>(
                arg => CompareArrays(arg.Selection, expected)
            ));
        }

        [Test]
        public void ShouldDoNothingWhenDoPortraitClickedWithNull()
        {
            _userInterfaceManager.DoPortraitClicked(null);
            _signalBus.DidNotReceiveWithAnyArgs().Fire(default);
        }

        [Test]
        public void ShouldDoPortraitClicked()
        {
            ISelectable[] selection = GetSubGroups(1);
            ISelectable clicked = selection[0];
            _userInterfaceManager.DoPortraitClicked(clicked);
            _signalBus.Received().Fire(Arg.Is<CameraGoToPositionSignal>(
                arg => arg.Position == clicked.Position
            ));
        }
        [Test]
        public void ShouldDoNothingWhenDoBannerClickedWithNull()
        {
            _userInterfaceManager.DoBannerClicked(default);
            _signalBus.DidNotReceiveWithAnyArgs().Fire(default);
        }

        [Test]
        public void ShouldDoBannerClicked()
        {
            ISelectable[] selection = GetSubGroups(4);
            ISelectable[] group = GetSubGroups(4);
            _userInterfaceManager.DoBannerClicked(group);
            _signalBus.Received().Fire(Arg.Is<ChangeSelectionSignal>(
                arg => CompareArrays(arg.Selection, group)
            ));
        }

        [Test]
        public void ShouldRemoveGroupWhenContainsAllDoBannerClicked()
        {
            ISelectable[] selection = GetSubGroups(6);
            ISelectable[] group = new ISelectable[] { selection[0], selection[1], selection[2] };
            ISelectable[] expected = selection.ToList().FindAll(x => !group.Contains(x)).ToArray();
            _userInterfaceManager.DoBannerClicked(group);
            _signalBus.Received().Fire(Arg.Is<ChangeSelectionSignal>(
                arg => CompareArrays(arg.Selection, expected)
            ));
        }

        [Test]
        public void ShouldAddGroupWhenContainsNoneDoBannerClicked()
        {
            ISelectable[] selection = GetSubGroups(4);
            ISelectable[] group = GetSubGroups(4);
            ISelectable[] expected = selection.ToList().Union(group).ToArray();
            _userInterfaceManager.DoBannerClicked(group);
            _signalBus.Received().Fire(Arg.Is<ChangeSelectionSignal>(
                arg => CompareArrays(arg.Selection, expected)
            ));
        }

        [Test]
        public void ShouldAddGroupWhenContainsSomeDoBannerClicked()
        {
            ISelectable[] aux = GetSubGroups(4);
            ISelectable[] group = GetSubGroups(3);
            ISelectable[] selection = new ISelectable[] { group[0], aux[0] };
            ISelectable[] expected = selection.ToList().Union(group).ToArray();
            _userInterfaceManager.DoBannerClicked(group);
            _signalBus.Received().Fire(Arg.Is<ChangeSelectionSignal>(
                arg => CompareArrays(arg.Selection, expected)
            ));
        }

        private static ISelectable[] GetSubGroups(int Amount)
        {
            Type[] types = new Type[] { typeof(IGroupable) };
            var selectables = TestUtils.GetSomeObjects(Amount, types);
            int halfAmount = Amount / 2;
            for (var i = 0; i < Amount; i++)
            {
                IGroupable groupable = selectables[i] as IGroupable;
                ISelectable selectable = selectables[i] as ISelectable;
                groupable.IsCompatible(default).ReturnsForAnyArgs(x =>
                {
                    var index = (x[0] as ISelectable).Index;
                    return (selectable.Index < halfAmount && index < halfAmount) || (selectable.Index >= halfAmount && index >= halfAmount);
                });
            }
            return selectables;
        }

        private bool CompareArrays(ISelectable[] selection, ISelectable[] expected)
        {
            if (selection.Length != expected.Length)
            {
                return false;
            }
            return selection.All(expected.Contains) && expected.All(selection.Contains);
        }
    }
}
