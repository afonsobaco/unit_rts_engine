using System;
using System.Collections;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class SelectionInfo
    {
        private ISelectable[] _oldSelection;
        private ISelectable[] _newSelection;
        private ISelectable[] _actualSelection;
        private SelectionType _selectionType;

        public ISelectable[] OldSelection { get => _oldSelection; set => _oldSelection = value; }
        public ISelectable[] NewSelection { get => _newSelection; set => _newSelection = value; }
        public ISelectable[] ActualSelection { get => _actualSelection; set => _actualSelection = value; }
        public SelectionType SelectionType { get => _selectionType; set => _selectionType = value; }
    }
}