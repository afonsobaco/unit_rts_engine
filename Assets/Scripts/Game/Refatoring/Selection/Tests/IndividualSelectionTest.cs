using UnityEngine;
using NUnit.Framework;
using System.Linq;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using NSubstitute;

namespace Tests
{
    public class IndividualSelectionTest
    {
        private IndividualSelection _individualSelection;

        [SetUp]
        public void SetUp()
        {
            _individualSelection = Substitute.ForPartsOf<IndividualSelection>();
        }

        [Test]
        public void IndividualSelectionTestSimplePasses()
        {
            Assert.NotNull(_individualSelection);
        }

        [Test]
        public void ShouldReturnEmptySelectionByDefault()
        {
            var result = _individualSelection.GetSelection(null, default);
            Assert.NotNull(result);
        }

        [Test]
        public void ShouldReturnEmptySelectionWhenNothingWasSelected()
        {
            const int amount = 10;
            var mainList = SelectionTestUtils.GetSomeSelectable(amount);
            var result = _individualSelection.GetSelection(mainList, null);
            Assert.IsEmpty(result);
        }

        [Test]
        public void ShouldReturnSelectionWhenClicked()
        {
            const int amount = 10;

            var mainList = SelectionTestUtils.GetSomeSelectable(amount);
            var expected = mainList.ToList().ElementAt(Random.Range(0, amount));
            var result = _individualSelection.GetSelection(mainList, expected);

            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(expected, result[0]);
        }
    }
}
