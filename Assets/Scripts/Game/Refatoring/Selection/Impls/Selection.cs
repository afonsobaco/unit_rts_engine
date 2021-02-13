using RTSEngine.Core;
using System.Linq;
using UnityEngine;

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
            var modified = _modifiersInterface.ApplyAll(_current, selection, type);
            FinalizeSelection(modified);
        }

        public virtual IRuntimeSet<ISelectable> GetMainList()
        {
            return _mainList;
        }

        private void FinalizeSelection(ISelectable[] selection)
        {
            for (var i = 0; i < _current.Length; i++)
            {
                if (!selection.Contains(_current[i]))
                {
                    _current[i].IsSelected = false;
                }
            }
            for (var i = 0; i < selection.Length; i++)
            {
                selection[i].IsSelected = true;
            }
            this._current = selection;
        }

        public virtual ISelectable[] GetCurrent()
        {
            return this._current;
        }

    }
}