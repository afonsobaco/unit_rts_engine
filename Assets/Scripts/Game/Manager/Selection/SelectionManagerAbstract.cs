using System.Collections;
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
    public abstract class SelectionManagerAbstract<T, E> : MonoBehaviour where T : ISelectableObject
    {

        private SelectionManagerXP<T, E> manager;
        private List<T> selection;
        public static SelectionManagerAbstract<T, E> Instance { get; private set; }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PerformSelection()
        {
            selection = manager.PerformSelection(selection, GetNewSelection(), GetSelectionType());
        }

        public abstract E GetSelectionType();
        public abstract List<T> GetNewSelection();
    }

    public class SelectionManagerXP<T, E> where T : ISelectableObject
    {

        public List<T> PerformSelection(List<T> selection, List<T> newSelection, E selectionType)
        {
            var args = StartSelection(selection, newSelection, selectionType);
            args = ApplyModifiers(args);
            return FinalizeSelection(args);
        }
        public SelectionArgsXP<T, E> StartSelection(List<T> selection, List<T> newSelection, E selectionType)
        {
            SelectionArgsXP<T, E> args = GetSelectionArgs(selection, newSelection, selectionType);
            return args;
        }
        public List<T> FinalizeSelection(SelectionArgsXP<T, E> args)
        {
            List<T> list = new List<T>();
            list.AddRange(args.NewSelection);
            List<T> toAddList = UpdateSelectionStatus(args.ToBeAdded, true);
            List<T> toRemoveList = UpdateSelectionStatus(args.ToBeRemoved, false);
            list.Union(toAddList);
            list.RemoveAll(a => toRemoveList.Contains(a));
            return list;
        }
        private List<T> UpdateSelectionStatus(List<T> list, bool selected)
        {            
           foreach (var item in list)
            {
                item.IsSelected = selected;
            }
            return list;
        }

        public SelectionArgsXP<T, E> GetSelectionArgs(List<T> selection, List<T> newSelection, E selectionType)
        {
            SelectionArgsXP<T, E> args = new SelectionArgsXP<T, E>();
            args.OldSelection = selection != null ? selection : new List<T>();
            args.NewSelection = newSelection != null ? newSelection : new List<T>();
            args.SelectionType = selectionType;
            return args;
        }

        public SelectionArgsXP<T, E> ApplyModifiers(SelectionArgsXP<T, E> args)
        {
            foreach (var item in GetModsBySelectionType(args.Settings.Mods, args.SelectionType))
            {
                args = item.Apply(args);
            }
            return args;
        }

        public List<IAbstractSelectionMod<T, E>> GetModsBySelectionType(List<IAbstractSelectionMod<T, E>> mods, E type)
        {
            return mods.FindAll(a => a.Type.Equals(type));
        }

    }
}
