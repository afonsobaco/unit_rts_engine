using System.Collections.Generic;
using RTSEngine.Core;
using System.Linq;
using UnityEngine;
namespace RTSEngine.Manager
{
    public class SameTypeUtil
    {

        public static HashSet<ISelectableObject> GetFromSameTypeInScreen(ISelectableObject selected, HashSet<ISelectableObject> mainList, Vector2 initialScreenPosition, Vector2 finalScreenPosition)
        {
            if (selected == null)
            {
                return new HashSet<ISelectableObject>();
            }
            HashSet<ISelectableObject> list = SelectionUtil.GetAllObjectsInsideSelectionArea(mainList, initialScreenPosition, finalScreenPosition);
            return new HashSet<ISelectableObject>(list.ToList().FindAll(a => selected.IsCompatible(a)));
        }

        public static HashSet<ISelectableObject> SortListByDistance(HashSet<ISelectableObject> list, Vector3 initialPosittion)
        {
            var sorted = new List<ISelectableObject>(list);
            sorted.Sort(new SameTypeComparer(initialPosittion));
            return new HashSet<ISelectableObject>(sorted);
        }

        public static HashSet<ISelectableObject> Shuffle(HashSet<ISelectableObject> collection)
        {
            ISelectableObject[] list = collection.ToArray();
            System.Random rng = new System.Random();
            int n = list.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                ISelectableObject value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return new HashSet<ISelectableObject>(list);
        }

    }

    public class SameTypeComparer : IComparer<ISelectableObject>
    {
        private Vector3 initialPosittion;

        public SameTypeComparer(Vector3 initialPosittion)
        {
            this.initialPosittion = initialPosittion;
        }

        public int Compare(ISelectableObject v1, ISelectableObject v2)
        {
            return (v1.Position - initialPosittion).sqrMagnitude.CompareTo((v2.Position - initialPosittion).sqrMagnitude);
        }
    }
}
