using System.Collections.Generic;
using RTSEngine.Core;
using System.Linq;
using UnityEngine;
namespace RTSEngine.Manager
{
    public class SameTypeUtil
    {

        public static HashSet<ISelectableObjectBehaviour> GetFromSameTypeInScreen(ISelectableObjectBehaviour selected, HashSet<ISelectableObjectBehaviour> mainList, Vector2 initialScreenPosition, Vector2 finalScreenPosition)
        {
            if (selected == null)
            {
                return new HashSet<ISelectableObjectBehaviour>();
            }
            HashSet<ISelectableObjectBehaviour> list = SelectionUtil.GetAllObjectsInsideSelectionArea(mainList, initialScreenPosition, finalScreenPosition);
            return new HashSet<ISelectableObjectBehaviour>(list.ToList().FindAll(a => selected.IsCompatible(a)));
        }

        public static HashSet<ISelectableObjectBehaviour> SortListByDistance(HashSet<ISelectableObjectBehaviour> list, Vector3 initialPosittion)
        {
            var sorted = new SortedSet<ISelectableObjectBehaviour>(new SameTypeComparer(initialPosittion));
            sorted.UnionWith(list);
            return new HashSet<ISelectableObjectBehaviour>(sorted);
        }

        public static HashSet<ISelectableObjectBehaviour> Shuffle(HashSet<ISelectableObjectBehaviour> collection)
        {
            ISelectableObjectBehaviour[] list = collection.ToArray();
            System.Random rng = new System.Random();
            int n = list.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                ISelectableObjectBehaviour value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return new HashSet<ISelectableObjectBehaviour>(list);
        }

    }

    public class SameTypeComparer : IComparer<ISelectableObjectBehaviour>
    {
        private Vector3 initialPosittion;

        public SameTypeComparer(Vector3 initialPosittion)
        {
            this.initialPosittion = initialPosittion;
        }

        public int Compare(ISelectableObjectBehaviour v1, ISelectableObjectBehaviour v2)
        {
            return (v1.Position - initialPosittion).sqrMagnitude.CompareTo((v2.Position - initialPosittion).sqrMagnitude);
        }
    }
}
