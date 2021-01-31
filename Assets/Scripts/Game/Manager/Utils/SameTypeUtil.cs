using System.Collections.Generic;
using RTSEngine.Core;
using System.Linq;
using UnityEngine;
namespace RTSEngine.Manager
{
    public class SameTypeUtil
    {
        public static HashSet<ISelectableObjectBehaviour> GetAllFromSameType(ISelectableObjectBehaviour selected, HashSet<ISelectableObjectBehaviour> mainList, Vector2 initialScreenPosition, Vector2 finalScreenPosition, SameTypeSelectionModeEnum mode)
        {
            HashSet<ISelectableObjectBehaviour> list = new HashSet<ISelectableObjectBehaviour>();
            if (selected != null)
            {
                list.Add(selected);
                HashSet<ISelectableObjectBehaviour> allFromSameType = SameTypeUtil.GetFromSameTypeInScreen(selected, mainList, initialScreenPosition, finalScreenPosition);
                if (mode == SameTypeSelectionModeEnum.RANDOM)
                {
                    list.UnionWith(Shuffle(allFromSameType));
                }
                else
                {
                    list.UnionWith(SortListByDistance(allFromSameType, selected.Position));
                }
                return new HashSet<ISelectableObjectBehaviour>();
            }
            return list;
        }

        private static HashSet<ISelectableObjectBehaviour> GetFromSameTypeInScreen(ISelectableObjectBehaviour selected, HashSet<ISelectableObjectBehaviour> mainList, Vector2 initialScreenPosition, Vector2 finalScreenPosition)
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
            list.ToList().Sort((v1, v2) => (v1.Position - initialPosittion).sqrMagnitude.CompareTo((v2.Position - initialPosittion).sqrMagnitude));
            return list;
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
}
