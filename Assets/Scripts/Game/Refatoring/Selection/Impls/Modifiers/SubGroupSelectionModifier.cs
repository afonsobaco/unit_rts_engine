using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using RTSEngine.Core;
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

        public override ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection)
        {
            StartVariables();
            return this._modifier.Apply(Input.GetKey(_key), actualSelection);
        }

        public class Modifier
        {
            public Vector2 InitialViewportPoint { get; set; }
            public Vector2 FinalViewportPoint { get; set; }
            public IRuntimeSet<ISelectable> MainList { get; set; }
            public IAreaSelectionType AreaSelectionType { get; set; }
            public EqualityComparerComponent EqualityComparer { get; internal set; }

            public ISelectable[] Apply(bool active, ISelectable[] actualSelection)
            {
                if (active && actualSelection.Length == 1)
                    return GetAllGroupableOnScreen(actualSelection.First());
                return actualSelection;
            }

            public virtual ISelectable[] GetAllGroupableOnScreen(ISelectable selected)
            {
                List<ISelectable> list = new List<ISelectable>();
                if (selected != null)
                {
                    list.Add(selected);
                    ISelectable[] allFromSubGroup = GetFromSubGroupOnScreen(selected);
                    list = list.Union(allFromSubGroup).ToList();
                }
                return list.ToArray();
            }

            public virtual ISelectable[] GetFromSubGroupOnScreen(ISelectable selected)
            {
                var allOnScreen = AreaSelectionType.GetAllInsideViewportArea(GetMainList(), InitialViewportPoint, FinalViewportPoint);
                return SubGroupUtil.GetFromSubGroupOnScreen(allOnScreen, selected, EqualityComparer);
            }

            public virtual ISelectable[] GetMainList()
            {
                return MainList.GetAllItems().ToArray();
            }
        }
    }
}
