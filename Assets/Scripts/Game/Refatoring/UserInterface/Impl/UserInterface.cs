using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using System;

namespace RTSEngine.Refactoring
{
    public class UserInterface
    {
        private Dictionary<object, ISelectable[]> _groups = new Dictionary<object, ISelectable[]>() { };
        private ISelectable[] _selection = new ISelectable[] { };
        private ISelectable _highlighted;

        public ISelectable Highlighted { get => _highlighted; set => _highlighted = value; }
        public ISelectable[] Selection { get => _selection; set => _selection = value; }
        public Dictionary<object, ISelectable[]> Groups { get => _groups; set => _groups = value; }

        public void DoSelectionUpdate(ISelectable[] selection)
        {
            this.Selection = selection;
            this.Highlighted = null;
            if (Selection.Length > 0)
            {
                this.Highlighted = selection[0];
                UpdateAllHighlighted();
            }
        }

        public void AlternateSubGroup(bool previous)
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
                foreach (var item in _selection)
                {
                    if (item is IGroupable)
                    {
                        IGroupable groupable = (item as IGroupable);
                        item.IsHighlighted = groupable.IsCompatible(_highlighted);
                    }
                }
            }
        }

        public void DoGroupUpdate(Dictionary<object, ISelectable[]> groups)
        {
            this.Groups = groups;
        }

        public void DoNextSubGroup()
        {
            if (_selection == null || _selection.Length == 0)
                _highlighted = null;
            if (_highlighted != null)
            {
                var index = _selection.ToList().FindLastIndex(x => { return (x as IGroupable).IsCompatible(_highlighted); });
                if (index < _selection.Length - 1)
                    _highlighted = _selection[index + 1];
                else
                    _highlighted = _selection[0];
            }
        }

        public void DoPreviousSubGroup()
        {

            if (_selection == null || _selection.Length == 0)
                _highlighted = null;
            if (_highlighted != null)
            {
                var index = _selection.ToList().FindIndex(x => { return (x as IGroupable).IsCompatible(_highlighted); });
                if (index > 0)
                    _highlighted = _selection.ToList().Find(x => { return (x as IGroupable).IsCompatible(_selection[index - 1]); });
                else
                    _highlighted = _selection.ToList().Find(x => { return (x as IGroupable).IsCompatible(_selection[_selection.Length - 1]); });
            }
        }
    }
}
