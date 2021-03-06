using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;
using UnityEditor;
using Zenject;

namespace RTSEngine.RTSSelection
{
    public abstract class BaseSelectionModifier : ScriptableObject, ISelectionModifier
    {
        [SerializeField] private bool active = true;
        [SerializeField] private SelectionType[] restrictedTypes;
        public SelectionType[] RestrictedTypes { get => restrictedTypes; set => restrictedTypes = value; }
        public bool Active { get => active; set => active = value; }

        public abstract ISelectable[] Apply(SelectionInfo info);

        private void Start()
        {
            StartVariables();
        }

        private void OnValidate()
        {
            StartVariables();
        }

        public virtual void StartVariables() { }

    }
}
