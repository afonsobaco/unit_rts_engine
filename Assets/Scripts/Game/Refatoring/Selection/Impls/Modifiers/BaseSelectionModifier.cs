using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Refactoring
{
    public abstract class BaseSelectionModifier : MonoBehaviour, ISelectionModifier
    {
        [SerializeField] private SelectionType[] restrictedTypes;

        public SelectionType[] RestrictedTypes { get => restrictedTypes; set => restrictedTypes = value; }

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
