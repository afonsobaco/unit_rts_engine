using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Selection.Util;
using UnityEngine;
using System;

namespace RTSEngine.Selection.Mod
{
    public class SameTypeSelectionMod : AbstractSelectionMod
    {


        [SerializeField] private Vector2 initialGameScreenPos = new Vector2(0, 0);
        [SerializeField] private Vector2 finalGameScreenPos = new Vector2(1, 1);
        [SerializeField] protected List<SelectableTypeEnum> canSelectSameType = new List<SelectableTypeEnum>();
        protected override List<SelectableObject> Apply(SelectionArgs args)
        {
            if (args.IsPreSelection)
            {
                return args.NewList;
            }
            return DoSameTypeSelection(args);
        }

        private List<SelectableObject> DoSameTypeSelection(SelectionArgs args)
        {
            List<SelectableObject> result = new List<SelectableObject>();
            if (args.IsSameType)
            {
                if (args.Clicked)
                {
                    result = AddOrRemoveAll(args, GetListOfSameType(args));
                }
                else
                {
                    result = new List<SelectableObject>(args.OldList);
                }
            }
            else
            {
                result = new List<SelectableObject>(args.NewList);
            }
            return result;
        }

        private List<SelectableObject> GetListOfSameType(SelectionArgs args)
        {
            List<SelectableObject> list = new List<SelectableObject>();
            if (canSelectSameType.Contains(args.Clicked.type))
            {
                list = FindAllFromSameTypeOnScreen(args);
            }
            else
            {
                list.Add(args.Clicked);
            }

            return list;
        }

        private List<SelectableObject> AddOrRemoveAll(SelectionArgs args, List<SelectableObject> list)
        {
            if (args.IsAditive)
            {
                List<SelectableObject> result = new List<SelectableObject>(args.OldList);
                if (args.OldList.Contains(args.Clicked))
                {
                    result.RemoveAll(a => list.Contains(a));
                }
                else
                {
                    result = result.Union(list).ToList();
                }
                return result;
            }
            else
            {
                return new List<SelectableObject>(list);
            }
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