using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Utils;
using RTSEngine.Commons;
using Zenject;

namespace RTSEngine.RTSSelection
{

    [CreateAssetMenu(fileName = "SortSelectionModifier", menuName = "Modifiers/SortSelectionModifier")]

    public class SortSelectionModifier : BaseSelectionModifier
    {
        private IComparer<IGrouping<ISelectable, ISelectable>> _subGroupComparer;
        private IEqualityComparer<ISelectable> _equalityComparer;

        private Modifier _modifier;

        [Inject]
        public void Construct(IComparer<IGrouping<ISelectable, ISelectable>> subGroupComparer, IEqualityComparer<ISelectable> equalityComparer)
        {
            _subGroupComparer = subGroupComparer;
            _equalityComparer = equalityComparer;
        }

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
