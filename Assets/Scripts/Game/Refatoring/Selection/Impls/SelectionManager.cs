using System.Collections.Generic;
using System;
using System.Linq;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class SelectionManager
    {
        private HashSet<ISelectable> _mainList = new HashSet<ISelectable>();
        private ISelectable[] _currentSelection = new ISelectable[] { };

        public virtual ISelectable[] GetMainList()
        {
            return _mainList.ToArray();
        }

        public void AddToMainList(ISelectable selectable)
        {
            if (!_mainList.Contains(selectable))
            {
                _mainList.Add(selectable);
            }
        }

        public void RemoveFromMainList(ISelectable selectable)
        {
            if (_mainList.Contains(selectable))
            {
                _mainList.Remove(selectable);
            }
        }

        public virtual void SetCurrentSelection(ISelectable[] selection)
        {
            this._currentSelection = selection;
        }

        public virtual ISelectable[] GetCurrentSelection()
        {
            return this._currentSelection;
        }
    }
}