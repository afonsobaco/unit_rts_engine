using NUnit.Framework;
using UnityEngine;
using System.Linq;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using Tests.Utils;
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
            var mainList = TestUtils.GetSomeObjects(amount);
            Vector2 startPoint = Vector2.zero;
            Vector2 endPoint = Vector2.one * amount / 2;
            MockSelectionInsideArea(startPoint, endPoint);

            var result = _areaSelection.GetSelection(mainList, startPoint, endPoint);
            Assert.IsNotEmpty(result);
            var _expected = mainList.ToList().FindAll(x => x.Position.x < amount / 2).ToArray();

            CollectionAssert.AreEquivalent(_expected, result);
            var lastDifference = 0f;
            for (var i = 1; i < result.Length; i++)
            {
                float d = Vector3.Distance(result[i].Position, result[i - 1].Position);
                Assert.True(d >= lastDifference);
            }
        }

        private void MockSelectionInsideArea(Vector2 startPoint, Vector2 endPoint)
        {
            var start = new Vector2(endPoint.x - startPoint.x >= 0 ? startPoint.x : endPoint.x, endPoint.y - startPoint.y >= 0 ? startPoint.y : endPoint.y);
            var end = new Vector2(endPoint.x - startPoint.x >= 0 ? endPoint.x : startPoint.x, startPoint.y - endPoint.y > 0 ? endPoint.y : startPoint.y);

            _polyAreaSelection.IsInsideScreenPoints(
                            Arg.Any<Vector2>(),
                            Arg.Any<Vector2>(),
                            Arg.Is<ISelectable>(x =>
                                x.Position.x >= start.x &&
                                x.Position.y >= start.y &&
                                x.Position.x <= end.x &&
                                x.Position.y <= end.y
                            )).Returns(true);
        }
    }
}
