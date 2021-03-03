using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Commons;
using System;

namespace RTSEngine.Refactoring
{

    [CreateAssetMenu(fileName = "DoubleClickSelectionModifier", menuName = "Modifiers/DoubleClickSelectionModifier")]

    public class DoubleClickSelectionModifier : BaseSelectionModifier
    {

        [Space]
        [Header("Modifier attributes")]
        [SerializeField] private float _doubleClickTime = 0.3f;
        private IEqualityComparer<ISelectable> _equalityComparer;
        private IAreaSelectionType _areaSelectionType;
        private IRuntimeSet<ISelectable> _mainList;
        private IViewportHelper _viewportHelper;
        private LastClicked _lastClicked;
        private Modifier _modifier;

        [Inject]
        public void Construct(IEqualityComparer<ISelectable> equalityComparer, IAreaSelectionType areaSelectionType, IRuntimeSet<ISelectable> mainList, IViewportHelper viewportHelper)
        {
            _areaSelectionType = areaSelectionType;
            _equalityComparer = equalityComparer;
            _mainList = mainList;
            _viewportHelper = viewportHelper;
        }

        public override void StartVariables()
        {
            if (_modifier == null)
            {
                _modifier = new Modifier();
            }
            _modifier.ViewportHelper = _viewportHelper;
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

            public IEqualityComparer<ISelectable> EqualityComparer { get; internal set; }
            public IAreaSelectionType AreaSelectionType { get; set; }
            public IRuntimeSet<ISelectable> MainList { get; set; }
            public IViewportHelper ViewportHelper { get; set; }

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
                var allOnScreen = AreaSelectionType.GetAllInsideViewportArea(GetMainList(), this.ViewportHelper.InitialViewportPoint, this.ViewportHelper.FinalViewportPoint);
                ISelectable[] result = SubGroupUtil.FilterBySubGroup(allOnScreen, selected, EqualityComparer);
                return DistanceHelper.SortWorldSpace(result, selected.Position);
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
