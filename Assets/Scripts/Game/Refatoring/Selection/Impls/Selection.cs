using System.Collections.Generic;
using RTSEngine.Core;
using System.Linq;
using System;

namespace RTSEngine.Refactoring
{
    public class Selection
    {
        private ModifiersInterface _modifiersInterface;
        private IRuntimeSet<ISelectable> _mainList;
        private ISelectable[] _current = new ISelectable[] { };

        public Selection(ModifiersInterface modifiersInterface, IRuntimeSet<ISelectable> mainList)
        {
            _modifiersInterface = modifiersInterface;
            _mainList = mainList;
        }

        public virtual void DoSelection(ISelectable[] selection, SelectionType type)
        {
            var modified = _modifiersInterface.ApplyAll(selection, type);
            FinalizeSelection(modified);
        }

        public virtual IRuntimeSet<ISelectable> GetMainList()
        {
            return _mainList;
        }

        private void FinalizeSelection(ISelectable[] expected)
        {
            this._current = expected;
        }

        public virtual ISelectable[] GetCurrent()
        {
            return this._current;
        }

    }
}