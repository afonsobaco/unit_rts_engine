using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using NSubstitute;

namespace Tests
{
    public static class SelectionTestUtils
    {
        //TODO remove
        public static ISelectable[] GetSomeSelectable(int qtt, int groupableQtt)
        {
            var list = new ISelectable[qtt];
            for (var i = 0; i < list.Length; i++)
            {
                if (i < groupableQtt)
                    list[i] = Substitute.For<ISelectable, IGroupable>();
                else
                    list[i] = Substitute.For<ISelectable>();
                list[i].Index = i;
                int minValue = 0;
                int maxValue = qtt;
                if (i < qtt / 2)
                {
                    maxValue = qtt / 2;
                }
                else
                {
                    minValue = (qtt / 2) + 1;
                }
                list[i].Position = new Vector3(Random.Range(minValue, maxValue), 0, Random.Range(minValue, maxValue));

            }
            return list;
        }

        public static ISelectable[] GetSomeSelectable(int qtt)
        {
            return GetSomeSelectable(qtt, 0);
        }
    }
}
