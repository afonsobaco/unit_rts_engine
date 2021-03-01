using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Utils;
using RTSEngine.Commons;
using System;

namespace RTSEngine.Refactoring
{
    public class OrderSelectionModifier : BaseSelectionModifier
    {

        [SerializeField] private SelectionType _type;

        [Space]
        [Header("Modifier attributes")]

        [SerializeField] private GroupingComparerComponent _subGroupComparer;
        [SerializeField] private EqualityComparerComponent _equalityComparer;

        private Modifier _modifier;

        public override void StartVariables()
        {
            if (_modifier == null)
            {
                _modifier = new Modifier();
            }
            _modifier.SubGroupComparer = _subGroupComparer;
            _modifier.EqualityComparer = _equalityComparer;
        }

        public override ISelectable[] Apply(SelectionInfo info)
        {
            StartVariables();
            return this._modifier.Apply(info.ActualSelection);
        }

        public class Modifier
        {

            public IEqualityComparer<ISelectable> EqualityComparer { get; set; }
            public IComparer<IGrouping<ISelectable, ISelectable>> SubGroupComparer { get; set; }

            public ISelectable[] Apply(ISelectable[] actualSelection)
            {
                if (actualSelection.Length <= 1)
                {
                    return actualSelection;
                }
                return OrderSubGroups(actualSelection);
            }

            public virtual ISelectable[] OrderSubGroups(ISelectable[] actualSelection)
            {
                return GameUtils.GetOrderedSelection(actualSelection, EqualityComparer, SubGroupComparer);
            }
        }
    }
}
