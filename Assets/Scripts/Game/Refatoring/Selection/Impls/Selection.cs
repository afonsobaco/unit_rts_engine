using System.Collections.Generic;
using RTSEngine.Core;
using System.Linq;
using System;

namespace RTSEngine.Refactoring
{
    public class Selection
    {
        private ModifiersInterface _modifiersInterface;
        private HashSet<ISelectable> _mainList = new HashSet<ISelectable>();
        private ISelectable[] _current = new ISelectable[] { };

        public Selection(ModifiersInterface modifiersInterface)
        {
            _modifiersInterface = modifiersInterface;
        }

        public virtual void DoSelection(ISelectable[] selection, SelectionType type)
        {
            var modified = _modifiersInterface.ApplyAll(selection, type);
            FinalizeSelection(modified);
        }

        private void FinalizeSelection(ISelectable[] expected)
        {
            this._current = expected;
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

        public virtual ISelectable[] GetMainList()
        {
            return _mainList.ToArray();
        }

        public virtual ISelectable[] GetCurrent()
        {
            return this._current;
        }

    }
}