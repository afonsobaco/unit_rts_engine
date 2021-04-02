using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Signal;
using System.Linq;
using Zenject;

namespace RTSEngine.RTSSelection
{
    public class Selection
    {
        private ModifiersInterface _modifiersInterface;
        private IRuntimeSet<ISelectable> _mainList;
        private ISelectable[] _actualSelection = new ISelectable[] { };
        private bool isSelecting;

        public Selection(ModifiersInterface modifiersInterface, IRuntimeSet<ISelectable> mainList)
        {
            _modifiersInterface = modifiersInterface;
            _mainList = mainList;
        }

        public virtual ISelectable[] DoSelection(ISelectable[] selection, SelectionType type)
        {
            var modified = _modifiersInterface.ApplyAll(_actualSelection, selection, type);
            return FinalizeSelection(modified);
        }

        public virtual ISelectable[] FinalizeSelection(ISelectable[] selection)
        {
            ChangeSelectionStatus(selection);
            this._actualSelection = selection;
            return _actualSelection;
        }

        private void ChangeSelectionStatus(ISelectable[] selection)
        {
            for (var i = 0; i < _actualSelection.Length; i++)
            {
                if (!selection.Contains(_actualSelection[i]))
                {
                    _actualSelection[i].IsSelected = false;
                }
                // _actualSelection[i].IsHighlighted = false;
            }
            for (var i = 0; i < selection.Length; i++)
            {
                selection[i].IsSelected = true;
            }
        }

        public virtual ISelectable[] GetActualSelection()
        {
            return this._actualSelection;
        }

        public void UpdateSelection()
        {
            _actualSelection = _actualSelection.Where(x => _mainList.GetMainList().Contains(x)).ToArray();
        }
    }
}