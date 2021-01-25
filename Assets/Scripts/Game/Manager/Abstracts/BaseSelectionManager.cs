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
            var args = StartSelection(currentSelection, newSelection, selectionType);
            args.IsPreSelection = isPreSelection;
            args = ApplyModifiers(args);
            return FinalizeSelection(args);
        }
        public virtual SelectionArgsXP StartSelection(List<ISelectable> currentSelection, List<ISelectable> newSelection, SelectionTypeEnum selectionType)
        {
            SelectionArgsXP args = GetSelectionArgs(currentSelection, newSelection, selectionType);
            return args;
        }
        public virtual List<ISelectable> FinalizeSelection(SelectionArgsXP args)
        {
            //TODO Tests (flickering and Selection shift)
            List<ISelectable> list = new List<ISelectable>();
            List<ISelectable> toAddList = new List<ISelectable>();
            List<ISelectable> toRemoveList = new List<ISelectable>();

            if (args.IsPreSelection)
            {
                toAddList = UpdatePreSelectionStatus(args.ToBeAdded, true);
            }
            else
            {
                toAddList = UpdateSelectionStatus(args.ToBeAdded, true);
                toRemoveList = UpdateSelectionStatus(args.ToBeRemoved, false);
            }

            list.AddRange(toAddList);
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

        public List<ISelectable> UpdatePreSelectionStatus(List<ISelectable> list, bool preSelected)
        {
            foreach (var item in list)
            {
                item.IsPreSelected = preSelected;
            }
            return list;
        }

        public virtual SelectionArgsXP GetSelectionArgs(List<ISelectable> currentSelection, List<ISelectable> newSelection, SelectionTypeEnum selectionType)
        {
            SelectionArgsXP args = new SelectionArgsXP();
            if (currentSelection != null) args.OldSelection = new List<ISelectable>(currentSelection);
            if (newSelection != null) args.NewSelection = new List<ISelectable>(newSelection);
            args.ToBeAdded = new List<ISelectable>(args.NewSelection);
            args.ToBeRemoved = new List<ISelectable>(args.OldSelection);
            args.SelectionType = selectionType;
            return args;
        }

        public virtual List<IBaseSelectionMod> GetModsBySelectionType(List<IBaseSelectionMod> mods, SelectionTypeEnum type)
        {
            if (mods != null)
                return mods.FindAll(a => a.Type.Equals(type));
            else
                return new List<IBaseSelectionMod>();
        }

        public abstract SelectionArgsXP ApplyModifiers(SelectionArgsXP args);

    }
}
