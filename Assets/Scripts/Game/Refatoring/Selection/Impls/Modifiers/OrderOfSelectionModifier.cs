using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class OrderOfSelectionModifier : MonoBehaviour, ISelectionModifier
    {

        [SerializeField] private SelectionType type;

        [Space]
        [Header("Modifier attributes")]
        [SerializeField] private AbstractModifierHelper orderOfSelection;

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
            modifier.OrderOfSelection = orderOfSelection;
        }

        public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
        {
            return this.modifier.Apply(oldSelection, newSelection, actualSelection, type);
        }

        public class Modifier
        {
            public IModifier OrderOfSelection { get; set; }

            public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
            {
                if (OrderOfSelection != null)
                    return OrderOfSelection.Apply(actualSelection);
                return actualSelection;
            }

        }
    }
}
