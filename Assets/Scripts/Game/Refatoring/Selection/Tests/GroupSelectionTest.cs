using NUnit.Framework;
using System.Linq;
using UnityEngine;
using RTSEngine.Refactoring;
using NSubstitute;

namespace Tests
{
    public class GroupSelectionTest
    {
        private GroupSelection _groupsSelection;


        [SetUp]
        public void SetUp()
        {
            _groupsSelection = Substitute.ForPartsOf<GroupSelection>();
        }

        [Test]
        public void GroupSelectionTestSimplePasses()
        {
            Assert.NotNull(_groupsSelection);
        }

        [Test]
        public void ShouldReturnEmptySelectionByDefault()
        {
            var result = _groupsSelection.GetSelection(null, default);
            Assert.IsEmpty(result);
        }

        [Test]
        public void ShouldReturnEmptySelectionWhenGroupDoesNotExist()
        {
            const int amount = 10;
            var mainList = SelectionTestUtils.GetSomeSelectable(amount);

            var result = _groupsSelection.GetSelection(mainList, 0);
            Assert.IsEmpty(result);
        }

        [Test]
        public void ShouldReturnSelectionWhenGroupExists()
        {
            const int amount = 10;
            const int groupId = 1;

            var mainList = SelectionTestUtils.GetSomeSelectable(amount);
            var expected = mainList.ToList().Take(3).ToArray();
            _groupsSelection.ChangeGroup(groupId, expected);

            var result = _groupsSelection.GetSelection(mainList, groupId);
            Assert.IsNotEmpty(result);
            CollectionAssert.AreEquivalent(expected, result);
        }

    }
}
