using System;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Selection.Mod;
namespace RTSEngine.Selection.Util
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
