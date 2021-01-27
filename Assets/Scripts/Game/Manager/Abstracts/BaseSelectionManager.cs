using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public abstract class BaseSelectionManager
    {

        public virtual List<ISelectable> PerformSelection(List<ISelectable> currentSelection, List<ISelectable> newSelection, SelectionTypeEnum selectionType)
        {
            return DoSelection(currentSelection, newSelection, selectionType, false);
        }

        public virtual List<ISelectable> PerformPreSelection(List<ISelectable> currentSelection, List<ISelectable> newSelection, SelectionTypeEnum selectionType)
        {
            return DoSelection(currentSelection, newSelection, selectionType, true);
        }

        private List<ISelectable> DoSelection(List<ISelectable> currentSelection, List<ISelectable> newSelection, SelectionTypeEnum selectionType, bool isPreSelection)
        {
            var args = StartSelection(currentSelection, newSelection, selectionType, isPreSelection);
            args = ApplyModifiers(args);
            return FinalizeSelection(args);
        }
        public virtual SelectionArgsXP StartSelection(List<ISelectable> currentSelection, List<ISelectable> newSelection, SelectionTypeEnum selectionType, bool isPreSelection)
        {
            SelectionArgsXP args = GetSelectionArgs(currentSelection, newSelection, selectionType, isPreSelection);
            return args;
        }
        public virtual List<ISelectable> FinalizeSelection(SelectionArgsXP args)
        {
            List<ISelectable> toAddList = new List<ISelectable>();
            List<ISelectable> toRemoveList = new List<ISelectable>();

            if (args.Arguments.IsPreSelection)
            {
                toAddList = UpdatePreSelectionStatus(args.Result.ToBeAdded, true);
            }
            else
            {
                toRemoveList = UpdateSelectionStatus(args.Result.ToBeRemoved, false);
                toAddList = UpdateSelectionStatus(args.Result.ToBeAdded, true);
            }

            List<ISelectable> list = new List<ISelectable>(toAddList);
            list.RemoveAll(a => toRemoveList.FindAll(x => !toAddList.Contains(x)).Contains(a));
            return list;
        }
        public List<ISelectable> UpdateSelectionStatus(List<ISelectable> list, bool selected)
        {
            foreach (var item in list)
            {
                item.IsSelected = selected;
            }
            return list;
        }

        public virtual List<ISelectable> UpdatePreSelectionStatus(List<ISelectable> list, bool preSelected)
        {
            foreach (var item in list)
            {
                item.IsPreSelected = preSelected;
            }
            return list;
        }


        public virtual List<IBaseSelectionMod> GetModsBySelectionType(List<IBaseSelectionMod> mods, SelectionTypeEnum type)
        {
            if (mods != null)
                return mods.FindAll(a => a.Type.Equals(type));
            else
                return new List<IBaseSelectionMod>();
        }

        public abstract SelectionArgsXP ApplyModifiers(SelectionArgsXP args);
        public abstract SelectionArgsXP GetSelectionArgs(List<ISelectable> currentSelection, List<ISelectable> newSelection, SelectionTypeEnum selectionType, bool isPreSelection);

    }
}
