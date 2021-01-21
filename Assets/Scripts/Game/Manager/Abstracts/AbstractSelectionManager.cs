using RTSEngine.Core.Interfaces;
using RTSEngine.Manager.Impls;
using RTSEngine.Manager.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RTSEngine.Manager.Abstracts
{
    public abstract class AbstractSelectionManager<T, ST, OT> where T : ISelectableObject
    {
        public virtual List<T> PerformSelection(List<T> currentSelection, List<T> newSelection, ST selectionType)
        {
            var args = StartSelection(currentSelection, newSelection, selectionType);
            args = ApplyModifiers(args);
            return FinalizeSelection(args);
        }
        public virtual SelectionArgsXP<T, ST, OT> StartSelection(List<T> currentSelection, List<T> newSelection, ST selectionType)
        {
            SelectionArgsXP<T, ST, OT> args = GetSelectionArgs(currentSelection, newSelection, selectionType);
            return args;
        }
        public virtual List<T> FinalizeSelection(SelectionArgsXP<T, ST, OT> args)
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

        public virtual SelectionArgsXP<T, ST, OT> GetSelectionArgs(List<T> currentSelection, List<T> newSelection, ST selectionType)
        {
            SelectionArgsXP<T, ST, OT> args = new SelectionArgsXP<T, ST, OT>();
            args.OldSelection = currentSelection != null ? currentSelection : new List<T>();
            args.NewSelection = newSelection != null ? newSelection : new List<T>();
            args.SelectionType = selectionType;
            args.Settings = GetSettings();
            return args;
        }
        public abstract ISelectionSettings<T, ST, OT> GetSettings();

        public virtual SelectionArgsXP<T, ST, OT> ApplyModifiers(SelectionArgsXP<T, ST, OT> args)
        {
            foreach (var item in GetModsBySelectionType(args.Settings.Mods, args.SelectionType))
            {
                args = item.Apply(args);
            }
            return args;
        }

        public virtual List<IAbstractSelectionMod<T, ST, OT>> GetModsBySelectionType(List<IAbstractSelectionMod<T, ST, OT>> mods, ST type)
        {
            if (mods != null)
                return mods.FindAll(a => a.Type.Equals(type));
            else
                return new List<IAbstractSelectionMod<T, ST, OT>>();
        }

    }
}
