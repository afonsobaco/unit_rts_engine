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
            modifier.GroupRestrictionHelper = modifierHelper;
        }

        public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
        {
            return this.modifier.Apply(actualSelection);
        }

        public class Modifier
        {
            public IModifier GroupRestrictionHelper { get; set; }

            public ISelectable[] Apply(ISelectable[] actualSelection)
            {
                if (GroupRestrictionHelper != null)
                    return GroupRestrictionHelper.Apply(actualSelection);
                return actualSelection;
            }

        }
    }
}
