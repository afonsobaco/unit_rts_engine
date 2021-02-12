using NUnit.Framework;
using UnityEngine;
using System.Linq;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using NSubstitute;
using System;

namespace Tests
{
    [TestFixture]
    public class AreaSelectionTest
    {
        private IAreaSelectionType _polyAreaSelection;
        private AreaSelection _areaSelection;

        [SetUp]
        public void SetUp()
        {
            _polyAreaSelection = Substitute.For<IAreaSelectionType>();
            _areaSelection = Substitute.ForPartsOf<AreaSelection>(new object[] { _polyAreaSelection });
        }

        [Test]
        public void AreaSelectionTestSimplePasses()
        {
            Assert.NotNull(_areaSelection);
        }

        [Test]
        public void ShouldReturnEmptySelectionByDefault()
        {
            var result = _areaSelection.GetSelection(null, default, default);
            Assert.IsEmpty(result);
        }

        [Test]
        public void ShouldReturnSelectionInsideArea()
        {
            const int amount = 10;
            var mainList = SelectionTestUtils.GetSomeSelectable(amount);
            Vector2 startPoint = Vector2.zero;
            Vector2 endPoint = Vector2.one * amount / 2;
            MockSelectionInsideArea(startPoint, endPoint);

            var result = _areaSelection.GetSelection(mainList, startPoint, endPoint);
            Assert.IsNotEmpty(result);
            var _expected = mainList.ToList().FindAll(x => x.Position.x < amount / 2).ToArray();
            CollectionAssert.AreEquivalent(_expected, result);
        }

        private void MockSelectionInsideArea(Vector2 startPoint, Vector2 endPoint)
        {
            _polyAreaSelection.IsInsideSelectionArea(
                            Arg.Any<Vector2>(),
                            Arg.Any<Vector2>(),
                            Arg.Is<ISelectable>(x =>
                                x.Position.x >= startPoint.x &&
                                x.Position.y >= startPoint.y &&
                                x.Position.x <= endPoint.x &&
                                x.Position.y <= endPoint.y
                            )).Returns(true);
        }
    }
}
