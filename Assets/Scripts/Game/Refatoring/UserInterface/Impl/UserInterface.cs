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
        private ISelectable[] _actualSelection = new ISelectable[] { };
        private ISelectable _highlighted;

        public ISelectable Highlighted { get => _highlighted; set => _highlighted = value; }

        public ISelectable[] GetActualSelection()
        {
            return _actualSelection;
        }
        public void SetActualSelection(ISelectable[] value)
        {
            _actualSelection = value;
        }

        public Dictionary<object, ISelectable[]> GetParties()
        {
            return _parties;
        }

        public void SetParties(Dictionary<object, ISelectable[]> value)
        {
            _parties = value;
        }

        private IEqualityComparer<ISelectable> _equalityComparer;

        public UserInterface(IEqualityComparer<ISelectable> equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }

        public void DoSelectionUpdate(ISelectable[] selection, bool isUISelection)
        {
            this._actualSelection = selection;
            if (!isUISelection || this.Highlighted == null || !this._actualSelection.Contains(this.Highlighted))
            {
                this.Highlighted = null;
                if (_actualSelection.Length > 0)
                {
                    this.Highlighted = selection[0];
                }
            }
            UpdateAllHighlighted();
        }

        public virtual void AlternateSubGroup(bool previous)
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
                _actualSelection.ToList().ForEach(x => x.IsHighlighted = selectables.Contains(x));
            }
        }

        public virtual ISelectable[] GetParty(object partyId)
        {
            ISelectable[] selectables;
            if (!GetParties().TryGetValue(partyId, out selectables))
            {
                selectables = new ISelectable[0];
            }
            return selectables;
        }

        public virtual void DoPartyUpdate(object partyId)
        {
            if (_actualSelection.Length > 0)
                this.GetParties()[partyId] = _actualSelection;
            else
                this.GetParties().Remove(partyId);
        }

        public void DoNextSubGroup()
        {
            if (_actualSelection == null)
            {
                return;
            }
            if (_actualSelection.Length == 0)
                _highlighted = null;
            if (_highlighted != null)
            {
                var index = _actualSelection.ToList().FindLastIndex(x =>
                {
                    return AreCompatible(x, _highlighted);

                });
                if (index < _actualSelection.Length - 1)
                    _highlighted = _actualSelection[index + 1];
                else
                    _highlighted = _actualSelection[0];
            }
        }

        public void DoPreviousSubGroup()
        {
            if (_actualSelection == null || _actualSelection.Length == 0)
                _highlighted = null;
            if (_highlighted != null)
            {
                var index = _actualSelection.ToList().FindIndex(x => { return AreCompatible(x, _highlighted); });
                if (index > 0)
                    _highlighted = _actualSelection.ToList().Find(x => { return AreCompatible(x, _actualSelection[index - 1]); });
                else
                    _highlighted = _actualSelection.ToList().Find(x => { return AreCompatible(x, _actualSelection[_actualSelection.Length - 1]); });
            }
        }

        private bool AreCompatible(ISelectable x, ISelectable y)
        {
            return _equalityComparer.Equals(x, y);
        }

        private List<ISelectable> GetSubGroupOf(ISelectable selectable)
        {
            List<ISelectable> list = new List<ISelectable>();
            foreach (var item in _actualSelection)
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
