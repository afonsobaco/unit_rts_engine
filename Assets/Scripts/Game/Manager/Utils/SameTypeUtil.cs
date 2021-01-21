using System;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core.Impls;
using RTSEngine.Manager.Impls.Deprecated;
namespace RTSEngine.Manager.Utils
{
    public class SameTypeUtil
    {
        public static List<SelectableObject> GetFromSameTypeInSelection(SelectionArgs args, Vector2 initialGameScreenPos, Vector2 finalGameScreenPos)
        {
            List<SelectableObject> list = SelectionUtil.FindAllOnScreen(args, initialGameScreenPos, finalGameScreenPos);
            list.RemoveAll(a => !isSameType(args.Clicked, a));
            return list;
        }

        public static bool isSameType(SelectableObject first, SelectableObject second)
        {
            return second.type == first.type && second.typeStr.Equals(first.typeStr);
        }
    }
}
