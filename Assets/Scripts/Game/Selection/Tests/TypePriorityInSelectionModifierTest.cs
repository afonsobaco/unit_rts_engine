using NUnit.Framework;
using RTSEngine.Core;
using Tests.Utils;
using System.Collections.Generic;
using NSubstitute;
using System.Linq;
using RTSEngine.RTSSelection;

namespace Tests
{
    [TestFixture]
    public class TypePriorityInSelectionModifierTest
    {

        private TypePriorityInSelectionModifier.Modifier modifier;
        private IModifierHelper typePriorityHelper;
        [SetUp]
        public void SetUp()
        {
            modifier = Substitute.ForPartsOf<TypePriorityInSelectionModifier.Modifier>();
            typePriorityHelper = Substitute.For<IModifierHelper>();
            modifier.TypePriorityHelper = typePriorityHelper;
            typePriorityHelper.Apply(Arg.Any<ISelectable[]>()).Returns(args =>
            {
                var a = new List<ISelectable>();
                var b = new List<ISelectable>();
                var c = new List<ISelectable>();
                (args[0] as ISelectable[]).ToList().ForEach(x =>
                {
                    if (x.Index < 4)
                    {
                        a.Add(x);
                    }
                    else if (x.Index < 7)
                    {
                        b.Add(x);
                    }
                    else
                    {
                        c.Add(x);
                    }
                });

                if (a.Count > 0)
                {
                    return a.ToArray();
                }
                else if (b.Count > 0)
                {
                    return new ISelectable[] { b.First() };
                }
                else if (c.Count > 0)
                {
                    return new ISelectable[] { c.First() };
                }
                return new ISelectable[] { };
            });
        }

        [Test]
        public void SelectionLimitModifierTestSimplePasses()
        {
            Assert.IsNotNull(modifier);
        }

        [TestCaseSource(nameof(Scenarios))]
        public void ShouldApplyModifier(int amount, int[] newSelectionIndexes, int[] expectedSelection)
        {
            ISelectable[] mainList = TestUtils.GetSomeObjects(amount);
            ISelectable[] newSelection = TestUtils.GetListByIndex(newSelectionIndexes, mainList);
            ISelectable[] expected = TestUtils.GetListByIndex(expectedSelection, mainList);

            var result = modifier.Apply(newSelection);

            CollectionAssert.AreEquivalent(expected, result);
        }

        private static IEnumerable<TestCaseData> Scenarios
        {
            get
            {
                foreach (var item in TestUtils.GetDefaultCases())
                {
                    List<int> expected = new List<int>();
                    List<int> aux = new List<int>();
                    foreach (var a in item.newSelection)
                    {
                        if (a < 4)
                        {
                            aux.Add(a);
                        }
                    }
                    if (aux.Count == 0)
                    {
                        foreach (var a in item.newSelection)
                        {
                            if (a < 7)
                            {
                                aux.Add(a);
                            }
                        }
                        if (aux.Count > 0)
                        {
                            expected.Add(aux[0]);
                        }
                    }
                    else
                    {
                        expected = aux;
                    }

                    yield return new TestCaseData(item.amount, item.newSelection, expected.ToArray()).SetName(TestUtils.GetCaseName(item));
                }
                yield return new TestCaseData(10, new int[] { 4, 5, 6 }, new int[] { 4 }).SetName("EMPTY OLD, MULTIPLE NEW, All on secondary list, first choice, get by secondary order");
                yield return new TestCaseData(10, new int[] { 7, 9, 4, 5, 6 }, new int[] { 4 }).SetName("EMPTY OLD, MULTIPLE NEW, All on secondary list, shuffled, first choice, get by secondary order");
            }
        }
    }
}
