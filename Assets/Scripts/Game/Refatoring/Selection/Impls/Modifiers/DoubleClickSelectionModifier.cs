using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Commons;
using System;

namespace RTSEngine.Refactoring
{
    public class DoubleClickSelectionModifier : BaseSelectionModifier
    {

        [Space]
        [Header("Modifier attributes")]
        [SerializeField] private float _doubleClickTime = 0.3f;
        [SerializeField] private Vector2 _initialViewportPoint = Vector2.zero;
        [SerializeField] private Vector2 _finalViewportPoint = Vector2.one;
        [SerializeField] private EqualityComparerComponent _equalityComparer;
        private IRuntimeSet<ISelectable> _mainList;
        private IAreaSelectionType _areaSelectionType;
        private LastClicked _lastClicked;

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
            var doubleClicked = GetDoubleClicked(info.NewSelection);
            UpdateLastClicked(info.NewSelection);
            if (doubleClicked != null)
            {
                return this._modifier.Apply(info.OldSelection, info.ActualSelection, doubleClicked);
            }
            return info.ActualSelection;
        }

        private void UpdateLastClicked(ISelectable[] newSelection)
        {
            _lastClicked = new LastClicked { Selectable = newSelection.Length == 1 ? newSelection.First() : null, Time = Time.time };
        }

        private ISelectable GetDoubleClicked(ISelectable[] actualSelection)
        {
            bool checkEquals = actualSelection.Length == 1 && _lastClicked != null && actualSelection.First().Equals(_lastClicked.Selectable);
            bool checkTime = _lastClicked != null ? Time.time - _lastClicked.Time <= _doubleClickTime : false;
            return checkEquals && checkTime ? actualSelection.First() : null;
        }

        public class Modifier
        {
            public Vector2 InitialViewportPoint { get; set; }
            public Vector2 FinalViewportPoint { get; set; }
            public IRuntimeSet<ISelectable> MainList { get; set; }
            public IAreaSelectionType AreaSelectionType { get; set; }
            public EqualityComparerComponent EqualityComparer { get; internal set; }

            public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] actualSelection, ISelectable doubleClicked)
            {
                var result = new List<ISelectable>(actualSelection);
                if (doubleClicked != null)
                {
                    ISelectable[] allFromSubGroup = GetAllGroupableOnScreen(doubleClicked);
                    result = result.Union(allFromSubGroup).ToList();
                    if (!oldSelection.Contains(doubleClicked))
                    {
                        result.RemoveAll(x => allFromSubGroup.Contains(x));
                    }
                    if (result.Count == 0)
                    {
                        result = result.Union(allFromSubGroup).ToList();
                    }
                }
                return result.ToArray();
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
                return SubGroupUtil.FilterBySubGroup(allOnScreen, selected, EqualityComparer);
            }

            public virtual ISelectable[] GetMainList()
            {
                return MainList.GetAllItems().ToArray();
            }
        }
    }

    internal class LastClicked
    {
        private ISelectable selectable;
        private float time;

        public ISelectable Selectable { get => selectable; set => selectable = value; }
        public float Time { get => time; set => time = value; }
    }
}
