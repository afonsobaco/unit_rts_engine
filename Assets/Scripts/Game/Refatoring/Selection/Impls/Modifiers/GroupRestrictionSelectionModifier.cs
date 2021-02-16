using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class GroupRestrictionSelectionModifier : MonoBehaviour, ISelectionModifier
    {

        [SerializeField] private SelectionType type;

        [Space]
        [Header("Modifier attributes")]
        [SerializeField] private AbstractModifierComponent modifierComponent;

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
            modifier.GroupRestriction = modifierComponent;
        }

        public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
        {
            return this.modifier.Apply(oldSelection, newSelection, actualSelection, type);
        }

        public class Modifier
        {
            public IModifier GroupRestriction { get; set; }

            public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
            {
                if (GroupRestriction != null)
                    return GroupRestriction.Apply(actualSelection);
                return actualSelection;
            }

        }
    }
}
