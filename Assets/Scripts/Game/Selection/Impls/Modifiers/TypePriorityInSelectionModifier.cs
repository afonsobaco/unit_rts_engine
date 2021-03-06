using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.RTSSelection
{
    [CreateAssetMenu(fileName = "TypePriorityInSelectionModifier", menuName = "Modifiers/TypePriorityInSelectionModifier")]
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

        public override ISelectable[] Apply(SelectionInfo info)
        {
            StartVariables();
            return this._modifier.Apply(info.ActualSelection);
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
