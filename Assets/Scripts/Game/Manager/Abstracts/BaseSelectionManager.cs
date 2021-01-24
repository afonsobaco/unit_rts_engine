using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace RTSEngine.Manager
{
    public abstract class BaseSelectionManager
    {

        public virtual List<SelectableObject> PerformSelection(List<SelectableObject> currentSelection, List<SelectableObject> newSelection, SelectionTypeEnum selectionType)
        {
            return DoSelection(currentSelection, newSelection, selectionType, false);
        }

        public virtual List<SelectableObject> PerformPreSelection(List<SelectableObject> currentSelection, List<SelectableObject> newSelection, SelectionTypeEnum selectionType)
        {
            return DoSelection(currentSelection, newSelection, selectionType, true);
        }

        private List<SelectableObject> DoSelection(List<SelectableObject> currentSelection, List<SelectableObject> newSelection, SelectionTypeEnum selectionType, bool isPreSelection)
        {
            var args = StartSelection(currentSelection, newSelection, selectionType);
            args.IsPreSelection = isPreSelection;
            //args = ApplyModifiers(args);
            return FinalizeSelection(args);
        }
        public virtual SelectionArgsXP StartSelection(List<SelectableObject> currentSelection, List<SelectableObject> newSelection, SelectionTypeEnum selectionType)
        {
            SelectionArgsXP args = GetSelectionArgs(currentSelection, newSelection, selectionType);
            return args;
        }
        public virtual List<SelectableObject> FinalizeSelection(SelectionArgsXP args)
        {
            List<SelectableObject> list = new List<SelectableObject>();
            List<SelectableObject> toAddList = UpdateSelectionStatus(args.ToBeAdded, true);
            List<SelectableObject> toRemoveList = UpdateSelectionStatus(args.ToBeRemoved, false);
            list = list.Union(toAddList).ToList();
            list.RemoveAll(a => toRemoveList.Contains(a));
            return list;
        }
        public List<SelectableObject> UpdateSelectionStatus(List<SelectableObject> list, bool selected)
        {
            foreach (var item in list)
            {
                item.IsSelected = selected;
            }
            return list;
        }

        public List<SelectableObject> UpdatePreSelectionStatus(List<SelectableObject> list, bool preSelected)
        {
            foreach (var item in list)
            {
                item.IsPreSelected = preSelected;
            }
            return list;
        }

        public virtual SelectionArgsXP GetSelectionArgs(List<SelectableObject> currentSelection, List<SelectableObject> newSelection, SelectionTypeEnum selectionType)
        {
            SelectionArgsXP args = new SelectionArgsXP();
            args.OldSelection = currentSelection != null ? currentSelection : new List<SelectableObject>();
            args.NewSelection = newSelection != null ? newSelection : new List<SelectableObject>();
            args.ToBeAdded = args.NewSelection;
            args.ToBeRemoved = args.OldSelection;
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
