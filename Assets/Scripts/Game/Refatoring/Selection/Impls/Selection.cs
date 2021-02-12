using System.Collections.Generic;
using RTSEngine.Core;
using System.Linq;
using System;

namespace RTSEngine.Refactoring
{
    public class Selection
    {
        private SelectionInterface _selectionInterface;
        private ModifiersInterface _modifiersInterface;
        private SelectionManager _selectionManager;

        public Selection(SelectionInterface selectionInterface, ModifiersInterface modifiersInterface, SelectionManager selectionManager)
        {
            _selectionInterface = selectionInterface;
            _modifiersInterface = modifiersInterface;
            _selectionManager = selectionManager;
        }

        public void OnAreaSignal(AreaSelectionSignal signal)
        {
            var selection = _selectionInterface.GetAreaSelection(_selectionManager.GetMainList(), signal.StartPoint, signal.EndPoint);
            var modified = _modifiersInterface.ApplyAll(selection, SelectionType.AREA);
            FinalizeSelection(modified);
        }

        public void OnGroupSignal(GroupSelectionSignal signal)
        {
            var selection = _selectionInterface.GetGroupSelection(_selectionManager.GetMainList(), signal.GroupId);
            var modified = _modifiersInterface.ApplyAll(selection, SelectionType.GROUP);
            FinalizeSelection(modified);
        }

        public void OnIndividualSignal(IndividualSelectionSignal signal)
        {
            var selection = _selectionInterface.GetIndividualSelection(_selectionManager.GetMainList(), signal.Clicked);
            var modified = _modifiersInterface.ApplyAll(selection, SelectionType.INDIVIDUAL);
            FinalizeSelection(modified);
        }

        public virtual void FinalizeSelection(ISelectable[] expected)
        {
            _selectionManager.SetCurrentSelection(expected);
        }
    }
}