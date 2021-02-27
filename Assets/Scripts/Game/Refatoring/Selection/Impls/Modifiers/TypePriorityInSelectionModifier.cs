using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class TypePriorityInSelectionModifier : BaseSelectionModifier
    {

        [Space]
        [Header("Modifier attributes")]
        [SerializeField] private AbstractModifierHelper _typePriorityHelper;

        private Modifier _modifier;

        public override void StartVariables()
        {
            if (_modifier == null)
            {
                _modifier = new Modifier();
            }
            _modifier.TypePriorityHelper = _typePriorityHelper;
        }

        public override ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection)
        {
            StartVariables();
            return this._modifier.Apply(actualSelection);
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
