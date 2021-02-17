using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class SelectionManager
    {

        private Selection _selection;
        private SelectionInterface _selectionInterface;
        private IRuntimeSet<ISelectable> _mainList;
        private bool _blockAreaSelection;
        public bool BlockAreaSelection { get => _blockAreaSelection; set => _blockAreaSelection = value; }

        public SelectionManager(Selection selection, SelectionInterface selectionInterface, IRuntimeSet<ISelectable> mainList)
        {
            this._selection = selection;
            _selectionInterface = selectionInterface;
            _mainList = mainList;
        }

        public void OnAreaSignal(AreaSelectionSignal signal)
        {
            if (!BlockAreaSelection)
            {
                var result = _selectionInterface.GetAreaSelection(GetMainList(), signal.StartPoint, signal.EndPoint);
                _selection.DoSelection(result, SelectionType.AREA);
            }
            BlockAreaSelection = false;
        }

        public void OnGroupSignal(GroupSelectionSignal signal)
        {
            if (signal.CreateNew)
            {
                _selectionInterface.SetGroupSelection(_selection.GetCurrent(), signal.GroupId);
            }
            else
            {
                var selection = _selectionInterface.GetGroupSelection(GetMainList(), signal.GroupId);
                _selection.DoSelection(selection, SelectionType.GROUP);
            }
        }

        public void OnIndividualSignal(IndividualSelectionSignal signal)
        {
            this.BlockAreaSelection = signal.BlockAreaSelection;
            var selection = _selectionInterface.GetIndividualSelection(GetMainList(), signal.Clicked);
            _selection.DoSelection(selection, SelectionType.INDIVIDUAL);
        }

        public void OnSelectableObjectCreatedSignal(SelectableObjectCreatedSignal signal)
        {
            _mainList.Add(signal.Selectable);
        }

        public void OnSelectableObjectDeletedSignal(SelectableObjectDeletedSignal signal)
        {
            _mainList.Remove(signal.Selectable);
        }

        public virtual ISelectable[] GetMainList()
        {
            return _mainList.GetAllItems().ToArray();
        }
    }
}