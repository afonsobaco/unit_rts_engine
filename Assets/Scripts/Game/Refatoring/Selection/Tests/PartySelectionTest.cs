using NUnit.Framework;
using System.Linq;
using Tests.Utils;
using RTSEngine.Refactoring;
using NSubstitute;

namespace Tests
{
    public class PartySelectionTest
    {
        private PartySelection _partySelection;[SetUp]
        public void SetUp()
        {
            _partySelection = Substitute.ForPartsOf<PartySelection>();
        }

        [Test]
        public void PartySelectionTestSimplePasses()
        {
            Assert.NotNull(_partySelection);
        }

        [Test]
        public void ShouldReturnEmptySelectionByDefault()
        {
            var result = _partySelection.GetSelection(null, default);
            Assert.IsEmpty(result);
        }

        [Test]
        public void ShouldReturnEmptySelectionWhenPartyDoesNotExist()
        {
            const int amount = 10;
            var mainList = TestUtils.GetSomeObjects(amount);

            var result = _partySelection.GetSelection(mainList, 0);
            Assert.IsEmpty(result);
        }

        [Test]
        public void ShouldReturnSelectionWhenPartyExists()
        {
            const int amount = 10;
            const int partyId = 1;

            var mainList = TestUtils.GetSomeObjects(amount);
            var expected = mainList.ToList().Take(3).ToArray();
            _partySelection.ChangeParty(partyId, expected);

            var result = _partySelection.GetSelection(mainList, partyId);
            Assert.IsNotEmpty(result);
            CollectionAssert.AreEquivalent(expected, result);
        }

    }
}
