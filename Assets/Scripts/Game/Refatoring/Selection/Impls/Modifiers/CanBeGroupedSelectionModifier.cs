using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class CanBeGroupedSelectionModifier : MonoBehaviour, ISelectionModifier
    {

        [SerializeField] private SelectionType type;

        [Space]
        [Header("Modifier attributes")]
        [SerializeField] private AbstractModifierHelper modifierHelper;

        private Modifier modifier = new Modifier();
        public SelectionType Type { get => type; set => type = value; }

        private void Start()
        {
            StartVariables();
        }

        private void OnValidate()
        {
            if (modifier != null)
            {
                StartVariables();
            }
        }

        private void StartVariables()
        {
            modifier.CanBeGroupedHelper = modifierHelper;
        }

        public ISelectable[] Apply(ref ISelectable[] oldSelection, ref ISelectable[] newSelection, ISelectable[] actualSelection)
        {
            return this.modifier.Apply(actualSelection);
        }

        public class Modifier
        {
            public IModifierHelper CanBeGroupedHelper { get; set; }

            public ISelectable[] Apply(ISelectable[] actualSelection)
            {
                if (CanBeGroupedHelper != null)
                    return CanBeGroupedHelper.Apply(actualSelection);
                return actualSelection;
            }

        }
    }
}
