﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Selection;
using RTSEngine.Selection.Util;
using RTSEngine.Selection.Mod;
using System.Linq;
using System;

namespace RTSEngine.Manager
{
    public abstract class AbstractSelectionManager<T, E> where T : ISelectableObject
    {
        public virtual List<T> PerformSelection(List<T> currentSelection, List<T> newSelection, E selectionType)
        {
            var args = StartSelection(currentSelection, newSelection, selectionType);
            args = ApplyModifiers(args);
            return FinalizeSelection(args);
        }
        public virtual SelectionArgsXP<T, E> StartSelection(List<T> currentSelection, List<T> newSelection, E selectionType)
        {
            SelectionArgsXP<T, E> args = GetSelectionArgs(currentSelection, newSelection, selectionType);
            return args;
        }
        public virtual List<T> FinalizeSelection(SelectionArgsXP<T, E> args)
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

        public virtual SelectionArgsXP<T, E> GetSelectionArgs(List<T> currentSelection, List<T> newSelection, E selectionType)
        {
            SelectionArgsXP<T, E> args = new SelectionArgsXP<T, E>();
            args.OldSelection = currentSelection != null ? currentSelection : new List<T>();
            args.NewSelection = newSelection != null ? newSelection : new List<T>();
            args.SelectionType = selectionType;
            return args;
        }

        public virtual SelectionArgsXP<T, E> ApplyModifiers(SelectionArgsXP<T, E> args)
        {
            foreach (var item in GetModsBySelectionType(args.Settings.Mods, args.SelectionType))
            {
                args = item.Apply(args);
            }
            return args;
        }

        public virtual List<IAbstractSelectionMod<T, E>> GetModsBySelectionType(List<IAbstractSelectionMod<T, E>> mods, E type)
        {
            if (mods != null)
                return mods.FindAll(a => a.Type.Equals(type));
            else
                return new List<IAbstractSelectionMod<T, E>>();
        }

    }
}