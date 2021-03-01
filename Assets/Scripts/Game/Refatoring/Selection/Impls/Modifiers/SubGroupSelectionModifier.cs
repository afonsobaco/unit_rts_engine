using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Commons;
using System;

namespace RTSEngine.Refactoring
{
    public class SubGroupSelectionModifier : BaseSelectionModifier
    {

        [Space]
        [Header("Modifier attributes")]
        [SerializeField] private KeyCode _key = KeyCode.LeftControl;
        [SerializeField] private Vector2 _initialViewportPoint = Vector2.zero;
        [SerializeField] private Vector2 _finalViewportPoint = Vector2.one;
        [SerializeField] private EqualityComparerComponent _equalityComparer;
        private IRuntimeSet<ISelectable> _mainList;
        private IAreaSelectionType _areaSelectionType;

        private Modifier _modifier;

        [Inject]
        public void Constructor(IRuntimeSet<ISelectable> mainList, IAreaSelectionType areaSelectionType)
        {
            _mainList = mainList;
            _areaSelectionType = areaSelectionType;
        }

        public override void StartVariables()
        {
            if (_modifier == null)
            {
                _modifier = new Modifier();
            }
            _modifier.InitialViewportPoint = _initialViewportPoint;
            _modifier.FinalViewportPoint = _finalViewportPoint;
            _modifier.MainList = _mainList;
            _modifier.AreaSelectionType = _areaSelectionType;
            _modifier.EqualityComparer = _equalityComparer;
        }

        public override ISelectable[] Apply(SelectionInfo info)
        {
            StartVariables();
            return this._modifier.Apply(Input.GetKey(_key), info.OldSelection, info.ActualSelection, info.SelectionType);
        }

        public class Modifier
        {
            public Vector2 InitialViewportPoint { get; set; }
            public Vector2 FinalViewportPoint { get; set; }
            public IRuntimeSet<ISelectable> MainList { get; set; }
            public IAreaSelectionType AreaSelectionType { get; set; }
            public EqualityComparerComponent EqualityComparer { get; internal set; }

            public ISelectable[] Apply(bool active, ISelectable[] oldSelection, ISelectable[] actualSelection, SelectionType type)
            {
                if (active && actualSelection.Length == 1)
                {
                    ISelectable[] selectables = type == SelectionType.INDIVIDUAL_ON_SELECTION ? oldSelection : GetAllOnScreenArea();
                    return FilterBySubGroup(selectables, actualSelection.First());
                }
                return actualSelection;
            }

            public virtual ISelectable[] GetAllOnScreenArea()
            {
                return AreaSelectionType.GetAllInsideViewportArea(GetMainList(), InitialViewportPoint, FinalViewportPoint);
            }

            public virtual ISelectable[] GetMainList()
            {
                return MainList.GetAllItems().ToArray();
            }

            public virtual ISelectable[] FilterBySubGroup(ISelectable[] selectables, ISelectable selected)
            {
                return SubGroupUtil.FilterBySubGroup(selectables, selected, EqualityComparer);
            }
        }
    }
}
