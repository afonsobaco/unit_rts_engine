using UnityEngine;
using RTSEngine.Core;
using NSubstitute;

namespace Tests
{
    public static class SelectionTestUtils
    {
        public static ISelectable[] GetSomeSelectable(int value)
        {
            var list = new ISelectable[value];
            for (var i = 0; i < list.Length; i++)
            {
                list[i] = Substitute.For<ISelectable>();
                list[i].Index = i;
                int minValue = 0;
                int maxValue = value;
                if (i < value / 2)
                {
                    maxValue = value / 2;
                }
                else
                {
                    minValue = (value / 2) + 1;
                }
                list[i].Position = new Vector3(Random.Range(minValue, maxValue), 0, Random.Range(minValue, maxValue));

            }
            return list;
        }
    }
}
