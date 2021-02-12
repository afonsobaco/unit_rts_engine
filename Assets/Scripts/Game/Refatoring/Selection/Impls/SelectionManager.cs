using System.Collections.Generic;
using System;
using System.Linq;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class SelectionManager
    {

        private Selection _selection;
        private SelectionInterface _selectionInterface;

        public SelectionManager(Selection selection, SelectionInterface selectionInterface)
        {
            this._selection = selection;
            _selectionInterface = selectionInterface;
        }

        public void OnAreaSignal(AreaSelectionSignal signal)
        {
            var result = _selectionInterface.GetAreaSelection(_selection.GetMainList(), signal.StartPoint, signal.EndPoint);
            _selection.DoSelection(result, SelectionType.AREA);
        }

        public void OnGroupSignal(GroupSelectionSignal signal)
        {
            var selection = _selectionInterface.GetGroupSelection(_selection.GetMainList(), signal.GroupId);
            _selection.DoSelection(selection, SelectionType.GROUP);
        }

        public void OnIndividualSignal(IndividualSelectionSignal signal)
        {
            var selection = _selectionInterface.GetIndividualSelection(_selection.GetMainList(), signal.Clicked);
            _selection.DoSelection(selection, SelectionType.INDIVIDUAL);
        }

        public void OnSelectableObjectCreatedSignal(SelectableObjectCreatedSignal signal)
        {
            _selection.AddToMainList(signal.Selectable);
        }

        public void OnSelectableObjectDeletedSignal(SelectableObjectDeletedSignal signal)
        {
            _selection.RemoveFromMainList(signal.Selectable);
        }
    }
}