﻿using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class CanBeGroupedSelectionModifier : BaseSelectionModifier
    {

        [Space]
        [Header("Modifier attributes")]
        [SerializeField] private AbstractModifierHelper _modifierHelper;

        private Modifier _modifier;

        public override void StartVariables()
        {
            if (_modifier == null)
            {
                _modifier = new Modifier();
            }
            _modifier.CanBeGroupedHelper = _modifierHelper;
        }

        public override ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection)
        {
            StartVariables();
            return this._modifier.Apply(actualSelection);
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
