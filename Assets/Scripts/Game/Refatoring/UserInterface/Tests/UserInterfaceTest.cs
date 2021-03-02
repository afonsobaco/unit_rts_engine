using System.Collections.Generic;
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
        private const int Amount = 4;

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
            ISelectable[] selectables = TestUtils.GetSomeObjects(Amount);
            _userInterface.Selection = selectables;
            _userInterface.Highlighted = selectables[0];

            _userInterface.DoNextSubGroup();

            Assert.NotNull(_userInterface.Highlighted);
            Assert.AreEqual(selectables[Amount / 2], _userInterface.Highlighted);
        }

        [Test]
        public void ShouldGoBackToFirstWhenDoNextSubGroupAtEndOfSubGroups()
        {
            ISelectable[] selectables = TestUtils.GetSomeObjects(Amount);
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
            ISelectable[] selectables = TestUtils.GetSomeObjects(Amount);
            _userInterface.Selection = selectables;
            _userInterface.Highlighted = selectables[Amount / 2];

            _userInterface.DoPreviousSubGroup();
            Assert.NotNull(_userInterface.Highlighted);
            Assert.AreEqual(selectables[0], _userInterface.Highlighted);
        }

        [Test]
        public void ShouldGoToLastWhenDoPreviousSubGroupAtStartOfSubGroups()
        {
            ISelectable[] selectables = TestUtils.GetSomeObjects(Amount);
            _userInterface.Selection = selectables;
            _userInterface.Highlighted = selectables[0];

            _userInterface.DoPreviousSubGroup();
            Assert.NotNull(_userInterface.Highlighted);
            Assert.AreEqual(selectables[Amount / 2], _userInterface.Highlighted);
        }

        class EqualityTest : IEqualityComparer<ISelectable>
        {
            public bool Equals(ISelectable x, ISelectable y)
            {
                return (x.Index < Amount / 2 && y.Index < Amount / 2) || (x.Index >= Amount / 2 && y.Index >= Amount / 2);
            }

            public int GetHashCode(ISelectable obj)
            {
                return obj.Index.GetHashCode();
            }
        }


    }
}
