using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Commons;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class UserInterface
    {
        private Dictionary<object, ISelectable[]> _parties = new Dictionary<object, ISelectable[]>() { };
        private ISelectable[] _selection = new ISelectable[] { };
        private ISelectable _highlighted;

        public ISelectable Highlighted { get => _highlighted; set => _highlighted = value; }
        public ISelectable[] Selection { get => _selection; set => _selection = value; }
        public Dictionary<object, ISelectable[]> Parties { get => _parties; set => _parties = value; }

        private EqualityComparerComponent _equalityComparer;

        public UserInterface(EqualityComparerComponent equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }

        public void DoSelectionUpdate(ISelectable[] selection)
        {
            this.Selection = selection;
            this.Highlighted = null;
            if (Selection.Length > 0)
            {
                this.Highlighted = selection[0];
                UpdateAllHighlighted();
            }
        }

        public void AlternateSubGroup(bool previous)
        {
            if (previous)
                DoPreviousSubGroup();
            else
                DoNextSubGroup();
            UpdateAllHighlighted();
        }

        private void UpdateAllHighlighted()
        {
            if (_highlighted != null)
            {
                List<ISelectable> selectables = GetSubGroupOf(_highlighted);
                _selection.ToList().ForEach(x => x.IsHighlighted = selectables.Contains(x));
            }
        }

        public void DoPartyUpdate(Dictionary<object, ISelectable[]> parties)
        {
            this.Parties = parties;
        }

        public void DoNextSubGroup()
        {
            if (_selection == null)
            {
                return;
            }
            if (_selection.Length == 0)
                _highlighted = null;
            if (_highlighted != null)
            {
                var index = _selection.ToList().FindLastIndex(x =>
                {
                    Debug.Log(x);
                    Debug.Log(_highlighted);
                    return AreCompatible(x, _highlighted);

                });
                if (index < _selection.Length - 1)
                    _highlighted = _selection[index + 1];
                else
                    _highlighted = _selection[0];
            }
        }

        public void DoPreviousSubGroup()
        {
            if (_selection == null || _selection.Length == 0)
                _highlighted = null;
            if (_highlighted != null)
            {
                var index = _selection.ToList().FindIndex(x => { return AreCompatible(x, _highlighted); });
                if (index > 0)
                    _highlighted = _selection.ToList().Find(x => { return AreCompatible(x, _selection[index - 1]); });
                else
                    _highlighted = _selection.ToList().Find(x => { return AreCompatible(x, _selection[_selection.Length - 1]); });
            }
        }

        private bool AreCompatible(ISelectable x, ISelectable y)
        {
            return _equalityComparer.Equals(x, y);
        }

        private List<ISelectable> GetSubGroupOf(ISelectable selectable)
        {
            List<ISelectable> list = new List<ISelectable>();
            foreach (var item in _selection)
            {
                if (AreCompatible(item, selectable))
                {
                    list.Add(item);
                }
            }
            return list;
        }
    }
}
