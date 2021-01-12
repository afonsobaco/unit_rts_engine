using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Selection;
using System.Linq;
using System;

namespace RTSEngine.Selection.Mod
{
    public interface IAbstractSelectionMod<T, E>
    {
        bool Active { get; set; }
        E Type { get; set; }

        SelectionArgsXP<T, E> Apply(SelectionArgsXP<T, E> args);
    }

    public abstract class AbstractSelectionMod<T, E> : MonoBehaviour, IAbstractSelectionMod<T, E>
    {

        [SerializeField] private bool active = true;
        private E type;

        public bool Active { get => active; set => active = value; }
        public E Type { get => type; set => type = value; }

        public abstract SelectionArgsXP<T, E> Apply(SelectionArgsXP<T, E> args);


    }
}
