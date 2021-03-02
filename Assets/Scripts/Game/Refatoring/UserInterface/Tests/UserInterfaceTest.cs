using System;
using NUnit.Framework;
using RTSEngine.Core;
using RTSEngine.Commons;
using RTSEngine.Refactoring;
using NSubstitute;
using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class UserInterfaceTest
    {
        private UserInterface _userInterface;
        private EqualityTest _equalityComparer;

        [SetUp]
        public void SetUp()
        {
            _equalityComparer = new EqualityTest();
            _userInterface = Substitute.ForPartsOf<UserInterface>(new object[] { _equalityComparer });
        }

        [Test]
        public void UserInterfaceTestSimplePasses()
        {
            Assert.NotNull(_userInterface);
        }

        [Test]
        public void ShouldReturnNullHighlightedWhenSelectionIsEmpty()
        {
            _userInterface.DoSelectionUpdate(new ISelectable[] { });
            Assert.Null(_userInterface.Highlighted);
        }

        [Test]
        public void ShouldReturnHighlightedAsTheFirstItemOnSelection()
        {
            var selectables = TestUtils.GetSomeObjects(10);
            _userInterface.DoSelectionUpdate(selectables);
            Assert.NotNull(_userInterface.Highlighted);
            Assert.AreEqual(selectables[0], _userInterface.Highlighted);
        }

        [Test]
        public void ShouldGetNullHighlightedWhenDoNextSubGroupWithEmptySelection()
        {
            _userInterface.DoNextSubGroup();
            Assert.Null(_userInterface.Highlighted);
        }

        [Test]
        public void ShouldGetNextHighlightedWhenDoNextSubGroup()
        {
            const int Amount = 2;
            ISelectable[] selectables = GetSubGroups(Amount);
            _userInterface.Selection = selectables;
            _userInterface.Highlighted = selectables[0];

            _userInterface.DoNextSubGroup();
            Assert.NotNull(_userInterface.Highlighted);
            Assert.AreEqual(selectables[Amount / 2], _userInterface.Highlighted);
        }

        [Test]
        public void ShouldGoBackToFirstWhenDoNextSubGroupAtEndOfSubGroups()
        {
            const int Amount = 2;
            ISelectable[] selectables = GetSubGroups(Amount);
            _userInterface.Selection = selectables;
            _userInterface.Highlighted = selectables[Amount / 2];

            _userInterface.DoNextSubGroup();
            Assert.NotNull(_userInterface.Highlighted);
            Assert.AreEqual(selectables[0], _userInterface.Highlighted);
        }
        [Test]
        public void ShouldGetNullHighlightedWhenDoPreviousSubGroupWithEmptySelection()
        {
            _userInterface.DoPreviousSubGroup();
            Assert.Null(_userInterface.Highlighted);
        }

        [Test]
        public void ShouldGetPreviousHighlightedWhenDoPreviousSubGroup()
        {
            const int Amount = 2;
            ISelectable[] selectables = GetSubGroups(Amount);
            _userInterface.Selection = selectables;
            _userInterface.Highlighted = selectables[Amount / 2];

            _userInterface.DoPreviousSubGroup();
            Assert.NotNull(_userInterface.Highlighted);
            Assert.AreEqual(selectables[0], _userInterface.Highlighted);
        }

        [Test]
        public void ShouldGoToLastWhenDoPreviousSubGroupAtStartOfSubGroups()
        {
            const int Amount = 2;
            ISelectable[] selectables = GetSubGroups(Amount);
            _userInterface.Selection = selectables;
            _userInterface.Highlighted = selectables[0];

            _userInterface.DoPreviousSubGroup();
            Assert.NotNull(_userInterface.Highlighted);
            Assert.AreEqual(selectables[Amount / 2], _userInterface.Highlighted);
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

        class EqualityTest : EqualityComparerComponent
        {
            public override bool Equals(ISelectable x, ISelectable y)
            {
                return x.Index.Equals(y.Index);
            }

            public override int GetHashCode(ISelectable obj)
            {
                return obj.Index.GetHashCode();
            }
        }
    }
}
