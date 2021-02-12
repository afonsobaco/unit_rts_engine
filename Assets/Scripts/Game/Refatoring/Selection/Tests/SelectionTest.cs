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
        private ModifiersInterface _modifiersInterface;

        [SetUp]
        public void SetUp()
        {
            _modifiersInterface = Substitute.For<ModifiersInterface>();

            _selection = Substitute.ForPartsOf<Selection>(new object[] { _modifiersInterface });

            _modifiersInterface.ApplyAll(Arg.Any<ISelectable[]>(), Arg.Any<SelectionType>()).Returns(x => x[0]);
        }

        [Test]
        public void SelectionTestSimplePasses()
        {
            Assert.NotNull(_selection);
        }

        [Test]
        public void ShouldCallDoSelection()
        {
            int amount = 10;
            var expected = SelectionTestUtils.GetSomeSelectable(amount);

            _selection.DoSelection(expected, default);

            CollectionAssert.AreEquivalent(expected, _selection.GetCurrent());

        }



    }
}
