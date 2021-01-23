using RTSEngine.Core;
using System.Collections.Generic;
using System.Linq;

namespace RTSEngine.Manager
{
    public abstract class AbstractSelectionManager<T, ST> where T : ISelectable
    {
        public virtual List<T> PerformSelection(List<T> currentSelection, List<T> newSelection, ST selectionType)
        {
            var args = StartSelection(currentSelection, newSelection, selectionType);
            args = ApplyModifiers(args);
            return FinalizeSelection(args);
        }
        public virtual ISelectionArgsXP<T, ST> StartSelection(List<T> currentSelection, List<T> newSelection, ST selectionType)
        {
            ISelectionArgsXP<T, ST> args = GetSelectionArgs(currentSelection, newSelection, selectionType);
            return args;
        }
        public virtual List<T> FinalizeSelection(ISelectionArgsXP<T, ST> args)
        {
            List<T> list = new List<T>();
            list.AddRange(args.NewSelection);
            List<T> toAddList = UpdateSelectionStatus(args.ToBeAdded, true);
            List<T> toRemoveList = UpdateSelectionStatus(args.ToBeRemoved, false);
            list.Union(toAddList);
            list.RemoveAll(a => toRemoveList.Contains(a));
            return list;
        }
        public List<T> UpdateSelectionStatus(List<T> list, bool selected)
        {
            foreach (var item in list)
            {
                item.IsSelected = selected;
            }
            return list;
        }

        public List<T> UpdatePreSelectionStatus(List<T> list, bool preSelected)
        {
            foreach (var item in list)
            {
                item.IsPreSelected = preSelected;
            }
            return list;
        }

        public virtual ISelectionArgsXP<T, ST> GetSelectionArgs(List<T> currentSelection, List<T> newSelection, ST selectionType)
        {
            ISelectionArgsXP<T, ST> args = new SelectionArgsXP<T, ST>();
            args.OldSelection = currentSelection != null ? currentSelection : new List<T>();
            args.NewSelection = newSelection != null ? newSelection : new List<T>();
            args.SelectionType = selectionType;
            args.SelectionModifiers = GetModifiersToBeApplied(selectionType);
            return args;
        }

        public virtual List<ISelectionMod<T, ST>> GetModsBySelectionType(List<ISelectionMod<T, ST>> mods, ST type)
        {
            if (mods != null)
                return mods.FindAll(a => a.Type.Equals(type));
            else
                return new List<ISelectionMod<T, ST>>();
        }

        public abstract ISelectionArgsXP<T, ST> ApplyModifiers(ISelectionArgsXP<T, ST> args);

        public abstract List<ISelectionMod<T, ST>> GetModifiersToBeApplied(ST selectionType);

    }
}
