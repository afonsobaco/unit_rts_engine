using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Selection.Util;
using UnityEngine;
using System;

namespace RTSEngine.Selection.Mod
{
    public class FindSameTypeInScreenOnClickSelectionMod : AbstracClickSelectionMod
    {

        [SerializeField] private Vector2 initialGameScreenPos = new Vector2(0, 0);
        [SerializeField] private Vector2 finalGameScreenPos = new Vector2(1, 1);
        [SerializeField] protected List<SelectableTypeEnum> canSelectSameType = new List<SelectableTypeEnum>();
        protected override List<SelectableObject> Execute(SelectionArgs args)
        {
            if (canSelectSameType.Contains(args.Clicked.type))
            {
                List<SelectableObject> list = FindAllFromSameTypeOnScreen(args);
                if (args.OldList.Contains(args.Clicked))
                {
                    args.NewList.RemoveAll(a => list.Contains(a));
                }else{
                    args.NewList = args.NewList.Union(list).ToList();
                }

                if(args.NewList.Count == 0){
                    args.NewList.AddRange(list);
                }
            }
            return args.NewList;
        }

        private List<SelectableObject> FindAllFromSameTypeOnScreen(SelectionArgs args)
        {
            var initialPos = args.Camera.ViewportToScreenPoint(initialGameScreenPos);
            var finalPos = args.Camera.ViewportToScreenPoint(finalGameScreenPos);
            var list = SelectionUtil.GetAllObjectsInsideSelectionArea<SelectableObject>(args.MainList, initialPos, finalPos, args.Camera);
            list.RemoveAll(a => isSameType(args.Clicked, a));
            return list;
        }
        private static bool isSameType(SelectableObject clicked, SelectableObject other)
        {
            return other.type != clicked.type || !other.typeStr.Equals(clicked.typeStr);
        }
    }
}