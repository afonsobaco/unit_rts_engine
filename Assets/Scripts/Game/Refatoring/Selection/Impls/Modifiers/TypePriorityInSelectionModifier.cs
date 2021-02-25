using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class TypePriorityInSelectionModifier : MonoBehaviour, ISelectionModifier
    {

        [SerializeField] private SelectionType type;

        [Space]
        [Header("Modifier attributes")]
        [SerializeField] private AbstractModifierHelper typePriorityHelper;

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
            modifier.TypePriorityHelper = typePriorityHelper;
        }

        public ISelectable[] Apply(ref ISelectable[] oldSelection, ref ISelectable[] newSelection, ISelectable[] actualSelection)
        {
            return this.modifier.Apply(actualSelection);
        }

        public class Modifier
        {
            public IModifierHelper TypePriorityHelper { get; set; }

            public ISelectable[] Apply(ISelectable[] actualSelection)
            {
                if (TypePriorityHelper != null)
                    return TypePriorityHelper.Apply(actualSelection);
                return actualSelection;
            }

        }
    }
}
